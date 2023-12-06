using Api.Clients;
using Api.Models;
using Api.JSONSerializators;

namespace Api.Services;

public class ForecastData
{
    public OpenWeather? OpenWeather { get; set; }
}

public interface IForecastAggregationService : IDisposable
{
    ForecastData? AggregateWeatherData(double lat, double lng);
}

public class ForecastAggregationService : IForecastAggregationService
{
    private readonly int _numberOfApiAggregations;
    private readonly CountdownEvent _aggregationEvent;
    private readonly ILogger<ForecastAggregationService> _logger;
    private readonly IForecastAggregationClient _forecastAggregationClient;

    public ForecastAggregationService(ILogger<ForecastAggregationService> logger, IForecastAggregationClient forecastAggregationClient)
    {
        _logger = logger;
        _numberOfApiAggregations = 1;
        _aggregationEvent = new(_numberOfApiAggregations);
        _forecastAggregationClient = forecastAggregationClient;
    }

    public ForecastData AggregateWeatherData(double lat, double lng)
    {
        var startTime = DateTime.UtcNow;
        OpenWeather? openWeather = null;

        var openWeatherThread = new Thread(() =>
        {
            openWeather = FetchOpenWeather(lat, lng);
            _aggregationEvent.Signal();
        })
        {
            Priority = ThreadPriority.AboveNormal
        };

        openWeatherThread.Start();

        _aggregationEvent.Wait();

        var endTime = DateTime.UtcNow;
        _logger.LogInformation($"⏭️ Weather data aggregation completed in {(endTime - startTime).TotalSeconds} seconds");

        _aggregationEvent.Reset();

        return new ForecastData() { OpenWeather = openWeather };
    }

    private OpenWeather? FetchOpenWeather(double lat, double lng)
    {
        var openWeatherUrl = "https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lng + "&units=metric&appid=" + Environment.GetEnvironmentVariable("OPEN_WEATHER_API_KEY");
        return _forecastAggregationClient.GetForecast<OpenWeather>(ForecastProvider.OpenWeather, openWeatherUrl);
    }

    public void Dispose()
    {
        _aggregationEvent.Dispose();
    }
}
