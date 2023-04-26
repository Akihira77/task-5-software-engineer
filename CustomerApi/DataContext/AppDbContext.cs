using CustomerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.DataContext;

public class AppDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public AppDbContext(DbContextOptions options) : base(options)
	{
	}
}
