using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserConnectionService.Application;
using UserConnectionService.Application.Interfaces;
using UserConnectionService.Application.Requests;
using UserConnectionService.Application.Responses;
using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories;
using UserConnectionService.Domain.Models;
using UserConnectionService.Infrastructure.Core.Interfaces;
using UserConnectionService.Infrastructure.Exceptions;

namespace UserConnectionService.Infrastructure.Services;

public class UserEventHandler : IUserEventHandler
{
    private readonly ILogger<UserEventHandler> _logger;
    private readonly IIpAddressValidator _ipAddressValidator;
    private readonly IErrorEventRepository _errorEventRepository;
    private readonly IUserConnectionEventRepository _userConnectionEventRepository;

    public UserEventHandler(
        ILogger<UserEventHandler> logger,
        IIpAddressValidator ipAddressValidator,
        IErrorEventRepository errorEventRepository,
        IUserConnectionEventRepository userConnectionEventRepository)
    {
        _logger = logger;
        _ipAddressValidator = ipAddressValidator;
        _errorEventRepository = errorEventRepository;
        _userConnectionEventRepository = userConnectionEventRepository;
    }

    public async Task<IpAddressListResponse> GetUserIpAddressesAsync(long userId)
    {
        var eventList = await _userConnectionEventRepository.GetAsync(e => e.UserId == userId);

        if (!eventList?.Any() ?? false)
        {
            return new();
        }

        return new()
        {
            IsSuccess = true,
            IpAddressList = eventList!.Select(e => e.IpAddress)
        };
    }

    //TODO add search to repository
    public async Task<UserEventResponse> GetUserLastConectionInfoAsync(long userId)
    {
        var events = await _userConnectionEventRepository.GetAsync(u => u.UserId == userId);
        var latestEvent = events.OrderByDescending(e => e.TimeStamp).FirstOrDefault();

        return new()
        {
            IsSuccess = true,
            UserEvent = new UserEvent
            {
                UserId = userId,
                IpAddress = latestEvent!.IpAddress,
                Timestamp = latestEvent!.TimeStamp
            }
        };
    }

    public async Task<UserListResponse> GetUsersByIpStartsWithAsync(string ipAddressSubstring)
    {
        if (string.IsNullOrWhiteSpace(ipAddressSubstring))
        {
            return new();
        }

        var events = await _userConnectionEventRepository.GetAsync(e => e.IpAddress.StartsWith(ipAddressSubstring));

        if (!events?.Any() ?? false)
        {
            return new();
        }

        return new()
        {
            IsSuccess = true,
            UserIds = events!.Select(e => e.UserId).Distinct()
        };
    }

    public async Task<UserEventProcessResponse> ProcessNewEventAsync(UserEventRequest? request)
    {
        var result = new UserEventProcessResponse();

        try
        {
            if (request is null)
            {
                throw new BadEventRequestException();
            }

            if (!_ipAddressValidator.Validate(request.IpAddress))
            {
                throw new IpAddressParseException(request.IpAddress);
            }

            var existentEvent = GetExistentEvent(request.UserId, request.IpAddress!);

            if (existentEvent == null)
            {
                await _userConnectionEventRepository.AddAsync(new UserConnectionEvent
                {
                    IpAddress = request.IpAddress!,
                    UserId = request.UserId,
                    VisitCount = 1,
                    TimeStamp = DateTimeOffset.UtcNow
                });
            }
            else
            {
                existentEvent.VisitCount++;
                existentEvent.TimeStamp = DateTimeOffset.UtcNow;
            }

            await _userConnectionEventRepository.SaveAsync();

            return new()
            {
                IsSuccess = true,
                ResultMessage = Constants.SuccessResultMessage
            };
        }
        catch (UserConnectionServiceException ex)
        {
            await _errorEventRepository.AddAsync(new ErrorEvent
            {
                ErrorMessage = ex.Message,
                Data = JsonConvert.SerializeObject(request)
            });

            _logger.LogError(ex, ex.Message);

            return new()
            {
                ResultMessage = ex.Message
            }; ;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private UserConnectionEvent? GetExistentEvent(long userId, string ipAddress)
    {
        return _userConnectionEventRepository.GetFirstOrDefault(u => u.UserId == userId && u.IpAddress.Equals(ipAddress, StringComparison.OrdinalIgnoreCase));
    }
}
