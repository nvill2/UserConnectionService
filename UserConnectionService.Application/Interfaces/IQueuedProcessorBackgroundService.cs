namespace UserConnectionService.Application.Interfaces;


public interface IQueuedProcessorBackgroundService
{
    Task ExecuteAsync(CancellationToken cancellationToken);

    int TaskQueueSize { get; }
}
