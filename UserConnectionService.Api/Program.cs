using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using UserConnectionService.Application.Interfaces;
using UserConnectionService.Data;
using UserConnectionService.Data.Repositories;
using UserConnectionService.Infrastructure.Core.Interfaces;
using UserConnectionService.Infrastructure.Services;
using UserConnectionService.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var postgresConnectionString = builder.Configuration.GetConnectionString("UserMonitoringConnectionString");

builder.Services.AddDbContext<UserMonitoringContext>(options => options.UseNpgsql(postgresConnectionString));

builder.Services.AddScoped<IUserConnectionEventRepository, UserConnectionEventRepository>();
builder.Services.AddScoped<IErrorEventRepository, ErrorEventRepository>();
builder.Services.AddScoped<IIpAddressValidator, IpAddressValidator>();

builder.Services.AddScoped<IUserEventHandler, UserEventHandler>();

builder.Services.AddNpgsql<UserMonitoringContext>(postgresConnectionString);
builder.Services.AddEntityFrameworkNpgsql();

builder.Services.AddLogging();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<UserMonitoringContext>();

context.Database.EnsureDeleted();
context.Database.EnsureCreated();

app.Run();
