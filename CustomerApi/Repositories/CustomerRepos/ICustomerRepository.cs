using CustomerApi.Models;

namespace CustomerApi.Repositories.CustomerRepos;

public interface ICustomerRepository
{
	Task<IEnumerable<Customer>> GetCustomers();
	Task<Customer> GetCustomerById(int id, bool noTrack = false);
	Task Add(Customer customer);
	void Update(Customer customer);
	void Delete(Customer customer);
	Task Save();
}
