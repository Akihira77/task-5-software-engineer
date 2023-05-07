using CustomerApi.Controllers;
using CustomerApi.DataContext;
using CustomerApi.Repositories.CustomerRepos;
using CustomerApi.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

//Log.Logger = new LoggerConfiguration()
//	.MinimumLevel.Information()
//	.WriteTo.Console()
//	.CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// logging setup
builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, services, configuration) =>
{
	configuration
		.WriteTo.File(builder.Configuration["Logging:LogFilePath"].ToString(), rollingInterval: RollingInterval.Day)
		.WriteTo.Console();
});

// Add services to the container.

// cors setup
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		if(builder.Environment.IsDevelopment())
		{
			policy.AllowAnyOrigin();
		} else
		{
			policy.WithOrigins("http://localhost:8080");
		}

	});
});

// database setup
builder.Services.AddDbContext<AppDbContext>(opt =>
{
	opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddHttpClient<TodoApiService>();

var app = builder.Build();

//var loggerFactory = app.Services.GetService<ILoggerFactory>();
//loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
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
