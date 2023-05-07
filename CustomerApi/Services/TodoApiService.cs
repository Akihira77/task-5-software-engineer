using CustomerApi.Models;

namespace CustomerApi.Services;

public class TodoApiService
{
	private readonly HttpClient _http;

	public TodoApiService(HttpClient http)
    {
		_http = http;
		_http.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
	}

	public async Task<IEnumerable<Todo>?> GetTodosAsync()
	{
		var todos = await _http.GetFromJsonAsync<IEnumerable<Todo>>("todos");

		return todos;
	}

	public Todo? GetTodo(int todoId)
	{
		var todo = _http.GetFromJsonAsync<Todo>($"todos/{todoId}").Result;

		return todo;
	}
}
