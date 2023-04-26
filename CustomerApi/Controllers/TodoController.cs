using CustomerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace CustomerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
	private readonly string baseUrl = "https://jsonplaceholder.typicode.com/todos";
	private readonly ILogger<TodoController> _logger;

	public TodoController(ILogger<TodoController> logger)
	{
		_logger = logger;
	}

	[HttpGet("get-todos")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<ICollection<Todo>>> GetAllTodos()
	{
		try
		{
			// Kita gunakan HttpClient untuk dapat mengakses sebuah URL
			using HttpClient http = new HttpClient();

			// Kita mengambil data dari API dengan menggunakan method GET
			// HttpResponseMessage akan mengembalikan StatusCode dan Data
			using HttpResponseMessage res = await http.GetAsync(baseUrl);

			if(!res.IsSuccessStatusCode)
			{
				return BadRequest(res);
			}

			// Kita ambil data dari HttpResponseMessage
			using HttpContent content = res.Content;

			// Membacanya dalam bentuk string
			var data = await content.ReadAsStringAsync();
			// Jika data yang kita ambil dari API kosong/null
			if(data == null)
			{
				_logger.LogInformation($"API Activity {DateTime.Now.ToString("f")} - Get Data From {baseUrl} - return null");
				return Ok(data);
			} else
			{

				// parsing data and output to Todo List/Collections
				var dataArr = JArray.Parse(data);
				var todoList = new Collection<Todo>();
				foreach(JObject obj in dataArr.Children<JObject>())
				{
					todoList.Add(new Todo
					{
						Id = Convert.ToInt32(obj["id"]),
						UserId = Convert.ToInt32(obj["userId"]),
						Title = obj["title"].ToString(),
						Completed = Convert.ToBoolean(obj["completed"])
					});
				}
				_logger.LogInformation($"API Activity {DateTime.Now.ToString("f")} - Get All Data From {baseUrl} - {todoList.Count}");
				return Ok(todoList);
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
		string url = baseUrl + $"/{id}";
		try
		{
			// Kita gunakan HttpClient untuk dapat mengakses sebuah URL
			using HttpClient http = new HttpClient();

			// Kita mengambil data dari API dengan menggunakan method GET
			// HttpResponseMessage akan mengembalikan StatusCode dan Data
			using HttpResponseMessage res = http.GetAsync(baseUrl).GetAwaiter().GetResult();

			if (!res.IsSuccessStatusCode)
			{
				return BadRequest(res);
			}

			// Kita ambil data dari HttpResponseMessage
			using HttpContent content = res.Content;

			// Membacanya dalam bentuk string
			var data = content.ReadAsStringAsync().GetAwaiter().GetResult();

			// Jika data yang kita ambil dari API kosong/null
			if(data == null)
			{
				_logger.LogInformation($"API Activity {DateTime.Now.ToString("f")} - Get Data From {url} - return null");
				return Ok(data);
			} else
			{
				//Parse data into a object.
				var dataObj = JObject.Parse(data);

				Todo todo = new Todo
				{
					Id = Convert.ToInt32(dataObj["id"]),
					UserId = Convert.ToInt32(dataObj["userId"]),
					Title = dataObj["title"].ToString(),
					Completed = Convert.ToBoolean(dataObj["completed"])
				};

				_logger.LogInformation($"API Activity {DateTime.Now.ToString("f")} - Get Data From {url} - Title '{todo.Title}'");
				return Ok(todo);
			}
		} catch(Exception ex)
		{
			_logger.LogError(ex.Message);
			return BadRequest(ex);
		}
	}
}
