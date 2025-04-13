using Microsoft.AspNetCore.Mvc;
using UserConnectionService.Application;
using UserConnectionService.Application.Interfaces;
using UserConnectionService.Application.Requests;
using UserConnectionService.Infrastructure.Services;

namespace UserConnectionService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ConnectionsController : ControllerBase
{
    private readonly IUserRequestHandler _userEventHandler;
    private readonly ILogger<ConnectionsController> _logger;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public ConnectionsController(
        IUserRequestHandler userEventHandler,
        ILogger<ConnectionsController> logger,
        IBackgroundTaskQueue backgroundTaskQueue)
    {
        _userEventHandler = userEventHandler;
        _logger = logger;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    [HttpPost]
    public IActionResult Post([FromBody] UserEventRequest request)
    {
        _backgroundTaskQueue.QueueBackgroundWorkItem(async (token) =>
        {
            try
            {
                var processResult = await _userEventHandler.ProcessNewEventAsync(request);

                if (processResult.IsSuccess)
                {
                    _logger.LogInformation(processResult.ResultMessage);
                    return;
                }

                _logger.LogError(processResult.ResultMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        });

        _logger.LogInformation(Constants.JobEnqueued);
        return Ok();
    }

    [HttpGet]
    [Route("usersbyip")]
    public async Task<IActionResult> GetUsersByIp(string ipAddressSubstring)
    {
        try
        {
            var processResult = await _userEventHandler.GetUsersByIpStartsWithAsync(ipAddressSubstring);

            if (processResult.IsSuccess)
            {
                return Ok(processResult);
            }

            _logger.LogError(processResult.ResultMessage);

            return BadRequest(processResult.ResultMessage);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }

    [HttpGet]
    [Route("getuseraddresses")]
    public async Task<IActionResult> GetUserAddresses(long userId)
    {
        try
        {
            var processResult = await _userEventHandler.GetUserIpAddressesAsync(userId);

            if (processResult.IsSuccess)
            {
                return Ok(processResult);
            }

            _logger.LogError(processResult.ResultMessage);

            return BadRequest(processResult.ResultMessage);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }

    [HttpGet]
    [Route("getlastevent")]
    public async Task<IActionResult> GetLastUserEvent(long userId)
    {
        try
        {
            var processResult = await _userEventHandler.GetUserLastConectionInfoAsync(userId);

            if (processResult.IsSuccess)
            {
                return Ok(processResult);
            }

            _logger.LogError(processResult.ResultMessage);

            return BadRequest(processResult.ResultMessage);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }
}
