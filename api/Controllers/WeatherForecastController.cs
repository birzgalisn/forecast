using Api.Contexts;
using Api.Hubs;
using Api.Models;
using Api.DTOs;
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

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                     ForecastDbContext dbContext,
                                     IHubContext<ForecastHub, IForecastHub> hub)
    {
        _logger = logger;
        _dbContext = dbContext;
        _hub = hub;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<List<WeatherForecast>> Get()
    {
        var forecasts = await _dbContext.WeatherForecasts.ToListAsync();

        return forecasts;
    }

    [HttpPost(Name = "SaveWeatherForecast")]
    public async Task<WeatherForecast> Post([FromBody] SaveWeatherForecast saveWeather)
    {
        var forecast = new WeatherForecast()
        {
            Location = saveWeather.Location,
            Lat = saveWeather.Lat,
            Lng = saveWeather.Lng,
            TemperatureC = saveWeather.TemperatureC,
        };

        _dbContext.WeatherForecasts.Add(forecast);

        await _dbContext.SaveChangesAsync();

        await _hub.Clients.All.NewForecast(forecast);

        return forecast;
    }

    [HttpDelete(Name = "DeleteWeatherForecasts")]
    public async Task<IActionResult> Delete()
    {
        await _dbContext.WeatherForecasts.ExecuteDeleteAsync();

        return Ok();
    }
}
