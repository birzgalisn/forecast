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
    public async Task<ActionResult<List<WeatherForecast>>> Get()
    {
        var forecasts = await _dbContext.WeatherForecasts
                                        .AsNoTracking()
                                        .ToListAsync()
                                        .ConfigureAwait(false);

        return Ok(forecasts);
    }

    [HttpPost(Name = "SaveWeatherForecast")]
    public async Task<ActionResult<WeatherForecast>> Post([FromBody] SaveWeatherForecast saveWeather)
    {
        var forecast = new WeatherForecast()
        {
            Location = saveWeather.Location,
            Lat = saveWeather.Lat,
            Lng = saveWeather.Lng,
            TemperatureC = saveWeather.TemperatureC,
        };

        _dbContext.WeatherForecasts.Add(forecast);

        var saveTask = _dbContext.SaveChangesAsync();
        var broadcastNewForecast = _hub.Clients.All.NewForecast(forecast);

        await Task.WhenAll(saveTask, broadcastNewForecast).ConfigureAwait(false);

        return CreatedAtAction(nameof(Get), new { forecast.Id }, forecast);
    }

    [HttpDelete(Name = "DeleteWeatherForecasts")]
    public async Task<IActionResult> Delete()
    {
        var deleteTask = _dbContext.WeatherForecasts.ExecuteDeleteAsync();
        var broadcastForecastsDeleted = _hub.Clients.All.ForecastsDeleted();

        await Task.WhenAll(deleteTask, broadcastForecastsDeleted).ConfigureAwait(false);

        return Ok();
    }
}
