using System.Collections.Concurrent;

namespace UserConnectionService.Infrastructure.Services;

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly SemaphoreSlim _semaphore = new(0);
    private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();

    public int QueueSize => _workItems.Count;

    public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
    {
        if (workItem == null) throw new ArgumentNullException(nameof(workItem));

        _workItems.Enqueue(workItem);
        _semaphore.Release();
    }

    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        _workItems.TryDequeue(out var workItem);

        return workItem!;
    }
}