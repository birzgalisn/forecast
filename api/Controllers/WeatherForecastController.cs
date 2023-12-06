using Api.Contexts;
using Api.Hubs;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Forecast.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ForecastDbContext _dbContext;
    private readonly IHubContext<ForecastHub, IForecastHub> _hub;
    private readonly IForecastAggregationService _forecastAggregationService;
    private readonly FahrenheitSignalerService _fahrenheitSignalerService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                     ForecastDbContext dbContext,
                                     IHubContext<ForecastHub, IForecastHub> hub,
                                     IForecastAggregationService forecastAggregationService,
                                     FahrenheitSignalerService fahrenheitSignalerService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _hub = hub;
        _forecastAggregationService = forecastAggregationService;
        _fahrenheitSignalerService = fahrenheitSignalerService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<List<WeatherForecast>> Get()
    {
        var forecasts = await _dbContext.WeatherForecasts.ToListAsync();

        return forecasts;
    }

    public class SaveWeatherForecast
    {
        public string Location { get; set; } = "";
        public double TemperatureC { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    [HttpPost(Name = "SaveWeatherForecast")]
    public async Task<WeatherForecast> Post([FromBody] SaveWeatherForecast saveWeather)
    {
        var weatherData = _forecastAggregationService.AggregateWeatherData(saveWeather.Lat, saveWeather.Lng);

        var forecast = new WeatherForecast()
        {
            Location = saveWeather.Location,
            Lat = saveWeather.Lat,
            Lng = saveWeather.Lng,
            TemperatureC = saveWeather.TemperatureC,
            WeatherApiTemperatureC = 0,
            OpenWeatherTemperatureC = weatherData?.OpenWeather?.Main?.Temp ?? 0,
            Icon = "",
            Condition = "",
        };

        _dbContext.WeatherForecasts.Add(forecast);

        await _dbContext.SaveChangesAsync();

        _fahrenheitSignalerService.Signal(forecast.Id);

        await _hub.Clients.All.NewForecast(forecast);

        return forecast;
    }

    [HttpDelete(Name = "DeleteWeatherForecast")]
    public async Task<IActionResult> Delete()
    {
        await _dbContext.WeatherForecasts.ExecuteDeleteAsync();

        return Ok();
    }
}
