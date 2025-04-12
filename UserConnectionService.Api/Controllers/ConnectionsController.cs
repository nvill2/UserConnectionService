using Microsoft.AspNetCore.Mvc;
using UserConnectionService.Application.Interfaces;
using UserConnectionService.Application.Requests;

namespace UserConnectionService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ConnectionsController : ControllerBase
{
    private readonly IUserEventHandler _userEventHandler;

    public ConnectionsController(IUserEventHandler userEventHandler)
    {
        _userEventHandler = userEventHandler;
    }

    // GET: api/<ConnectionsController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<ConnectionsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ConnectionsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserEventRequest request)
    {
        try
        {
            var processResult = await _userEventHandler.ProcessNewEventAsync(request);

            return Ok(processResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.ToString());
        }
    }
}
