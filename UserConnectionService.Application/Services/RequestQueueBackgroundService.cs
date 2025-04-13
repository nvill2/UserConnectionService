using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserConnectionService.Infrastructure.Services;

namespace UserConnectionService.Application.Services;

// this will handle our operations on background
public class RequestQueueBackgroundService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RequestQueueBackgroundService> _logger;

    public int TaskQueueSize => _taskQueue.QueueSize;

    public RequestQueueBackgroundService(
        IBackgroundTaskQueue taskQueue,
        IServiceProvider serviceProvider,
        ILogger<RequestQueueBackgroundService> logger)
    {
        _taskQueue = taskQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Queued Processor Background Service is starting.");

        while (!cancellationToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(cancellationToken);

            try
            {
                await workItem(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred executing {nameof(workItem)}.");
            }
            finally
            {
                await Task.Delay(100); // just to decrease CPU load
            }
        }

        _logger.LogInformation("Queued Processor Background Service is stopping.");
    }
}