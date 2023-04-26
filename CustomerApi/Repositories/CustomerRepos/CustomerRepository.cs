using CustomerApi.DataContext;
using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Repositories.CustomerRepos;

public class CustomerRepository : ICustomerRepository
{
	private readonly AppDbContext _db;

	public CustomerRepository(AppDbContext db)
	{
		_db = db;
	}

	public async Task Add(Customer customer)
	{
		await _db.Customers.AddAsync(customer);
	}

	public void Delete(Customer customer)
	{
		_db.Customers.Remove(customer);
	}

	public async Task<Customer> GetCustomerById(int id, bool noTrack = false)
	{
		if (noTrack)
		{
			return await _db.Customers
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.Id == id);
		}
		return await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);
	}

	public async Task<IEnumerable<Customer>> GetCustomers()
	{
		return await _db.Customers.ToListAsync();
	}

	public async Task Save()
	{
		await _db.SaveChangesAsync();
	}

	public void Update(Customer customer)
	{
		_db.Customers.Update(customer);
	}
}
