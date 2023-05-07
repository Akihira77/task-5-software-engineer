using CustomerApi.Models;
using CustomerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
	private readonly ILogger<TodoController> _logger;
	private readonly TodoApiService _todoApiService;

	public TodoController(ILogger<TodoController> logger, TodoApiService todoApiService)
	{
		_logger = logger;
		_todoApiService = todoApiService;
	}

	[HttpGet("get-todos")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<ICollection<Todo>>> GetAllTodos()
	{
		try
		{
			var todos = await _todoApiService.GetTodosAsync();
			if(todos == null)
			{
				_logger.LogInformation($"Todo Controller - Get All - return null");
				return NotFound(todos);
			} else
			{
				_logger.LogInformation($"Todo Controller - Get All - return {todos.Count()} data");
				return Ok(todos);
			}
		} catch(Exception ex)
		{
			_logger.LogError(ex.Message);
			return BadRequest(ex);
		}
	}

	[HttpGet("get-todo/{id:int}")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Todo> GetTodoById(int id)
	{
		try
		{
			var todo = _todoApiService.GetTodo(id);
			if(todo == null)
			{
				_logger.LogInformation($"Todo Controller - Get by Id - return null");
				return NotFound(todo);
			} else
			{
				_logger.LogInformation($"Todo Controller - Get by Id - return {todo.Title}");
				return Ok(todo);
			}
		} catch(Exception ex)
		{
			_logger.LogError(ex.Message);
			return BadRequest(ex);
		}
	}
}
