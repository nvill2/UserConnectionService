using Microsoft.AspNetCore.Mvc;
using UserConnectionService.Application.Interfaces;

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
    public void Post([FromBody] string value)
    {
    }
}
