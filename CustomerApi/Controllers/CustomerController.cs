using CustomerApi.Models;
using CustomerApi.Repositories.CustomerRepos;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
	private readonly ICustomerRepository _customerRepository;
	private readonly ILogger<CustomerController> _logger;


	public CustomerController(
		ICustomerRepository customerRepository,
		ILogger<CustomerController> logger)
	{
		_customerRepository = customerRepository;
		_logger = logger;
	}

	[HttpGet("get-all-customers")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
	{
		var customers = await _customerRepository.GetCustomers();
		return Ok(customers);
	}

	[HttpGet("get-customer/{id:int}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Customer>> GetCustomerById(int id)
	{
		var customer = await _customerRepository.GetCustomerById(id);

		if(customer == null)
		{
			_logger.LogError($"Customer Controller - Customer id '{id}' does not exists");
			return NotFound();
		}
		return Ok(customer);
	}

	[HttpPost("add-customer")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AddCustomer(Customer customer)
	{

		try
		{
			await _customerRepository.Add(customer);
			await _customerRepository.Save();
			return NoContent();
		} catch(Exception ex)
		{
			_logger.LogError($"Customer Controller - Add - Error: {ex.Message}");
		}

		return BadRequest();
	}

	[HttpPut("update-customer")]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> UpdateCustomer(Customer customer)
	{
		var customerDb = await _customerRepository.GetCustomerById(customer.Id, true);
		if(customerDb == null)
		{
			_logger.LogError($"Customer Controller - Customer id '{customer.Id}' does not exists");
			return NotFound(customer);
		}

		_customerRepository.Update(customer);

		try
		{
			await _customerRepository.Save();
			return NoContent();
		} catch(Exception ex)
		{
			_logger.LogError($"Customer Controller - Update - Error: {ex.Message}");
		}

		return BadRequest();
	}

	[HttpDelete("delete-customer/{id:int}")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> DeleteCustomer(int id)
	{
		var customer = await _customerRepository.GetCustomerById(id);

		if(customer == null)
		{
			_logger.LogError($"Customer Controller - Customer id '{id}' does not exists");
			return NotFound();
		}

		_customerRepository.Delete(customer);

		try
		{
			await _customerRepository.Save();
			return NoContent();
		} catch(Exception ex)
		{
			_logger.LogError($"Customer Controller - Delete - Error: {ex.Message}");
		}

		return BadRequest();
	}
}
