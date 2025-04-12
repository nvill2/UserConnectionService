using Microsoft.AspNetCore.Mvc;
using UserConnectionService.Application.Interfaces;
using UserConnectionService.Application.Requests;

namespace UserConnectionService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ConnectionsController : ControllerBase
{
    private readonly IUserEventHandler _userEventHandler;
    private readonly ILogger<ConnectionsController> _logger;

    public ConnectionsController(
        IUserEventHandler userEventHandler,
        ILogger<ConnectionsController> logger)
    {
        _userEventHandler = userEventHandler;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserEventRequest request)
    {
        try
        {
            var processResult = await _userEventHandler.ProcessNewEventAsync(request);

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
