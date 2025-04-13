using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Net.Http.Json;
using UserConnectionService.Data;
using UserConnectionService.Data.Entities;
using UserConnectionService.Infrastructure.Core.Interfaces;
using UserConnectionService.Infrastructure.Services;

namespace UserConnectionService.Tests;

public class UnitTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _applicationFactory = new();

    public UnitTests()
    {
        _httpClient = _applicationFactory.CreateClient();
    }

    [Fact]
    public void IpAddressValidationTests()
    {
        var address1 = "1.2.3.4";
        var address2 = "123:asd:qwe:123";

        var scope = _applicationFactory.Services.CreateScope();
        var validator = scope.ServiceProvider.GetRequiredService<IIpAddressValidator>();

        Assert.True(validator.Validate(address1));
        Assert.False(validator.Validate(address2));
    }

    [Fact]
    public async Task TestEnqueueEvent()
    {
        var userConnectionEvent = new UserConnectionEvent
        {
            IpAddress = "2.3.4.5",
            UserId = 12345
        };

        var response = await _httpClient.PostAsync("/api/connections", JsonContent.Create(userConnectionEvent));
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task TestPostFailedEvent()
    {
        var userConnectionEvent = new UserConnectionEvent
        {
            IpAddress = "2.three.4.5",
            UserId = 12345
        };

        var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<UserMonitoringContext>();
        var countBefore = await context.ErrorEvents.CountAsync();

        var response = await _httpClient.PostAsync("/api/connections", JsonContent.Create(userConnectionEvent));
        Assert.True(response.IsSuccessStatusCode);

        await Task.Delay(2000);

        var countAfter = await context.ErrorEvents.CountAsync();
        Assert.Equal(countAfter, countBefore + 1);
    }

    [Fact]
    public async Task TestHighLoad()
    {
        var userConnectionEvent = new UserConnectionEvent
        {
            IpAddress = "2.3.4.5",
            UserId = 12345
        };

        var scope = _applicationFactory.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<UserMonitoringContext>();
        var backgroundQueue = _applicationFactory.Services.GetRequiredService<IBackgroundTaskQueue>();

        Stopwatch stopwatch = Stopwatch.StartNew();
        int eventsCount = 5000;

        for (int i = 0; i < eventsCount; i++)
        {
            var response = await _httpClient.PostAsync("/api/connections", JsonContent.Create(userConnectionEvent));
            Assert.True(response.IsSuccessStatusCode);
        }

        stopwatch.Stop();

        Debug.WriteLine($"{eventsCount} requeests have been enqueued for processing in {stopwatch.Elapsed.TotalSeconds} seconds.");

        stopwatch = Stopwatch.StartNew();

        while (backgroundQueue.QueueSize > 0)
        {
            await Task.Delay(2000);
        }

        stopwatch.Stop();

        Debug.WriteLine($"{eventsCount} new events have been processed in {stopwatch.Elapsed.TotalSeconds} seconds.");
    }
}