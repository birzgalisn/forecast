using Api.Contexts;
using Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class FahrenheitWorkerService : BackgroundService
{
    private readonly ILogger<FahrenheitWorkerService> _logger;
    private readonly IServiceScope _scope;
    private readonly ForecastDbContext _dbContext;
    private readonly IHubContext<ForecastHub, IForecastHub> _hub;
    private readonly FahrenheitSignalerService _signaler;

    public FahrenheitWorkerService(ILogger<FahrenheitWorkerService> logger,
                                   IServiceProvider serviceProvider,
                                   IHubContext<ForecastHub, IForecastHub> hub,
                                   FahrenheitSignalerService signaler)
    {
        _logger = logger;
        _scope = serviceProvider.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ForecastDbContext>();
        _hub = hub;
        _signaler = signaler;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Run(() => _signaler.WaitSignal(stoppingToken), stoppingToken);

            while (_signaler.TryDequeueForecastId(out var forecastId))
            {
                Thread.Sleep(TimeSpan.FromSeconds(15));

                _logger.LogInformation($"🟩 Processing weather forecast with ID: {forecastId}");

                var forecast = await _dbContext.WeatherForecasts.SingleOrDefaultAsync(wf => wf.Id == forecastId, stoppingToken);

                if (forecast != null && forecast.OpenWeatherTemperatureC != null && forecast.WeatherApiTemperatureC != null)
                {
                    var averageTemperatureC = (forecast.TemperatureC + forecast.OpenWeatherTemperatureC + forecast.WeatherApiTemperatureC) / 3;

                    forecast.AverageTemperatureF = 32 + (int)(averageTemperatureC / 0.5556);

                    await _dbContext.SaveChangesAsync(stoppingToken);

                    await _hub.Clients.All.FahrenheitCalculated(forecast.Id, forecast.AverageTemperatureF);
                }
            }

            _signaler.ResetSignal();
        }
    }
}
