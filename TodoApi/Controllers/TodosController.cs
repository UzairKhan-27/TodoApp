using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoLibrary.DataAccess;
using TodoLibrary.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
    private readonly ITodoData _data;
    private readonly ILogger<TodosController> _logger;

    public TodosController(ITodoData data, ILogger<TodosController> logger)
    {
        _data = data;
        _logger = logger;
    }
    // GET: api/<TodosController>
    [HttpGet]
    public async Task<ActionResult<List<TodoModel>>> Get()
    {
        _logger.LogInformation("Calling all todos");
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var output = await _data.GetAllAssigned(int.Parse(userId));
            return Ok(output);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Get all todos failed");
            return BadRequest();
        }
    }

    // GET api/<TodosController>/5
    [HttpGet("{todoId}")]
    public async Task<ActionResult<TodoModel>> Get(int todoId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var output = await _data.GetOneAssigned(int.Parse(userId), todoId);
        return Ok(output);
    }

    // POST api/<TodosController>
    [HttpPost]
    public async Task<ActionResult<TodoModel>> Post([FromBody] string task)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var output = await _data.Create(int.Parse(userId),task);
        return Ok(output);

    }

    // PUT api/<TodosController>/5
    [HttpPut("{todoId}")]
    public async Task<ActionResult> Put(int todoId, [FromBody] string task)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _data.UpdateTask(int.Parse(userId), todoId, task);
        return Ok();
    }
    [HttpPut("{todoId}/Complete")]
    public async Task<IActionResult> Complete(int todoId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _data.CompleteTodo(int.Parse(userId), todoId); 
        return Ok();
    }

    // DELETE api/<TodosController>/5
    [HttpDelete("{todoId}")]
    public async Task<IActionResult> Delete(int todoId)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        await _data.DeleteTodo(int.Parse(userId),todoId);
        return Ok();

    }
}
