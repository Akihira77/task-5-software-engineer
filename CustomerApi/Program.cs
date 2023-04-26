using CustomerApi.DataContext;
using CustomerApi.Repositories.CustomerRepos;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.WriteTo.Console()
	.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddEntityFrameworkNpgsql()
//				.AddDbContext<AppDbContext>(opt =>
//				{
//					opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//				});
builder.Services.AddDbContext<AppDbContext>(opt =>
{
	opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

var loggerFactory = app.Services.GetService<ILoggerFactory>();
loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
