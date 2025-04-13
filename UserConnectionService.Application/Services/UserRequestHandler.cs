using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;
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

public class UserRequestHandler : IUserRequestHandler
{
    private readonly ILogger<UserRequestHandler> _logger;
    private readonly IIpAddressValidator _ipAddressValidator;
    private readonly IErrorEventRepository _errorEventRepository;
    private readonly IUserConnectionEventRepository _userConnectionEventRepository;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public UserRequestHandler(
        ILogger<UserRequestHandler> logger,
        IIpAddressValidator ipAddressValidator,
        IErrorEventRepository errorEventRepository,
        IUserConnectionEventRepository userConnectionEventRepository,
        IBackgroundTaskQueue backgroundTaskQueue
        )
    {
        _logger = logger;
        _ipAddressValidator = ipAddressValidator;
        _errorEventRepository = errorEventRepository;
        _userConnectionEventRepository = userConnectionEventRepository;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    public async Task<IpAddressListResponse> GetUserIpAddressesAsync(long userId)
    {
        var ipAddresses = await _userConnectionEventRepository.GetIpAddressesByUserId(userId);

        if (!ipAddresses?.Any() ?? false)
        {
            return new()
            {
                ResultMessage = string.Format(Constants.NoEventsForUserIdFormat, userId)
            };
        }

        return new()
        {
            IsSuccess = true,
            IpAddressList = ipAddresses,
            ResultMessage = Constants.SuccessResultMessage
        };
    }

    public async Task<UserEventResponse> GetUserLastConectionInfoAsync(long userId)
    {
        var latestEvent = await _userConnectionEventRepository.GetLatestEventByUserIdAsync(userId);

        if (latestEvent == null)
        {
            return new()
            {
                ResultMessage = string.Format(Constants.NoEventsForUserIdFormat, userId)
            };
        }

        return new()
        {
            IsSuccess = true,
            UserEvent = new UserEvent
            {
                UserId = userId,
                IpAddress = latestEvent!.IpAddress,
                Timestamp = latestEvent!.TimeStamp
            },
            ResultMessage = Constants.SuccessResultMessage
        };
    }

    public async Task<UserListResponse> GetUsersByIpStartsWithAsync(string ipAddressSubstring)
    {
        if (string.IsNullOrWhiteSpace(ipAddressSubstring))
        {
            return new()
            {
                ResultMessage = Constants.EmptyOrNulRequest
            };
        }

        var userIds = await _userConnectionEventRepository.GetUsersWithIpStartsWith(ipAddressSubstring);

        if (!userIds?.Any() ?? false)
        {
            return new()
            {
                ResultMessage = string.Format(Constants.NotIpAddressesFormat, ipAddressSubstring)
            };
        }

        return new()
        {
            IsSuccess = true,
            UserIds = userIds,
            ResultMessage = Constants.SuccessResultMessage
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
                Data = JsonConvert.SerializeObject(request),
                TimeStamp = DateTimeOffset.UtcNow
            });

            await _errorEventRepository.SaveAsync();

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
        return _userConnectionEventRepository
            .GetFirstOrDefault(u => u.UserId == userId && u.IpAddress.Equals(ipAddress, StringComparison.OrdinalIgnoreCase));
    }
}
