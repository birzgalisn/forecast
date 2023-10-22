using Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public interface IForecastHub
{
    public Task NewForecast(WeatherForecast forecast);

    public Task FahrenheitCalculated(int forecastId, double? fahrenheit);
}

public class ForecastHub : Hub<IForecastHub>
{
    public async Task NewForecast(WeatherForecast forecast)
    {
        await Clients.All.NewForecast(forecast);
    }

    public async Task FahrenheitCalculated(int forecastId, double fahrenheit)
    {
        await Clients.All.FahrenheitCalculated(forecastId, fahrenheit);
    }
}
