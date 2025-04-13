using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserConnectionService.Infrastructure.Services;

namespace UserConnectionService.Application.Services;

public class QueuedProcessorBackgroundService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<QueuedProcessorBackgroundService> _logger;

    public QueuedProcessorBackgroundService(
        IBackgroundTaskQueue taskQueue,
        IServiceProvider serviceProvider,
        ILogger<QueuedProcessorBackgroundService> logger)
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
                await Task.Delay(100);
            }
        }

        _logger.LogInformation("Queued Processor Background Service is stopping.");
    }
}