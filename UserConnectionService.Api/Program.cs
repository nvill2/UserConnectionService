using Microsoft.EntityFrameworkCore;
using UserConnectionService.Data;
using UserConnectionService.Data.Repositories;
using UserConnectionService.Infrastructure.Core.Interfaces;
using UserConnectionService.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var postgresConnectionString = builder.Configuration.GetConnectionString("UserMonitoringConnectionString");

builder.Services.AddDbContext<UserMonitoringContext>(options => options.UseNpgsql(postgresConnectionString));

builder.Services.AddScoped<IUserConnectionEventRepository, UserConnectionEventRepository>();
builder.Services.AddScoped<IErrorEventRepository, ErrorEventRepository>();
builder.Services.AddScoped<IIpAddressValidator, IpAddressValidator>();

builder.Services.AddNpgsql<UserMonitoringContext>(postgresConnectionString);
builder.Services.AddEntityFrameworkNpgsql();

builder.Logging.AddConsole();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
