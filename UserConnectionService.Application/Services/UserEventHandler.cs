using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UserConnectionService.Application;
using UserConnectionService.Application.Interfaces;
using UserConnectionService.Application.Requests;
using UserConnectionService.Application.Responses;
using UserConnectionService.Data.Entities;
using UserConnectionService.Data.Repositories;
using UserConnectionService.Infrastructure.Core.Interfaces;
using UserConnectionService.Infrastructure.Exceptions;

namespace UserConnectionService.Infrastructure.Services;

public class UserEventHandler : IUserEventHandler
{
    private readonly ILogger<UserEventHandler> _logger;
    private readonly IIpAddressValidator _ipAddressValidator;
    private readonly IErrorEventRepository _errorEventRepository;
    private readonly IUserConnectionEventRepository _userConnectionEventRepository;

    UserEventHandler(
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

    public Task<IpAddressListResponse> GetUserIpAddresses(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserEventResponse> GetUserLastConectionInfo(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserListResponse> GetUsersByIpStartsWith(string ipAddressSubstring)
    {
        throw new NotImplementedException();
    }

    public async Task<UserEventProcessResponse> ProcessEvent(UserEventRequest? request)
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

            var existentEvent = await GetExistentEvent(request.UserId, request.IpAddress!);

            if (existentEvent == null)
            {
                await _userConnectionEventRepository.AddAsync(new UserConnectionEvent
                {
                    IpAddress = request.IpAddress!,
                    UserId = request.UserId,
                    VisitCount = 1
                });
            }
            else
            {
                existentEvent.VisitCount++;
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
                ResultMessage = Constants.FailedResultMessage
            }; ;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<UserConnectionEvent?> GetExistentEvent(long userId, string ipAddress)
    {
        return await _userConnectionEventRepository.GetFirstOrDefaultAsync(u => u.UserId == userId && u.IpAddress.Equals(ipAddress, StringComparison.OrdinalIgnoreCase));
    }

    Task<UserEventProcessResponse> IUserEventHandler.ProcessEvent(UserEventRequest? request)
    {
        throw new NotImplementedException();
    }
}
