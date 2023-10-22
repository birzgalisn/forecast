using System.Collections.Concurrent;

namespace Api.Services;

public class FahrenheitSignalerService
{
    private readonly ILogger<FahrenheitSignalerService> _logger;
    private readonly ConcurrentQueue<int> _forecastIdQueue;
    private readonly ManualResetEventSlim _queueEvent;
    private readonly object _lock;

    public FahrenheitSignalerService(ILogger<FahrenheitSignalerService> logger)
    {
        _logger = logger;
        _forecastIdQueue = new();
        _queueEvent = new(false);
        _lock = new();
    }

    public void Signal(int forecastId)
    {
        lock (_lock)
        {
            _forecastIdQueue.Enqueue(forecastId);
            _queueEvent.Set();
            _logger.LogInformation($"🟨 Enqueued a weather forecast with ID: {forecastId}");
        }
    }

    public bool TryDequeueForecastId(out int forecastId)
    {
        lock (_lock)
        {
            return _forecastIdQueue.TryDequeue(out forecastId);
        }
    }

    public void WaitSignal(CancellationToken cancellationToken)
    {
        _queueEvent.Wait(cancellationToken);
    }

    public void ResetSignal()
    {
        _queueEvent.Reset();
    }
}
