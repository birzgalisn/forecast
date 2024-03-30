using Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public interface IForecastHub
{
    public Task NewForecast(WeatherForecast forecast);

    public Task ForecastsDeleted();
}

public class ForecastHub : Hub<IForecastHub>
{
    public async Task NewForecast(WeatherForecast forecast)
    {
        await Clients.All.NewForecast(forecast);
    }

    public async Task ForecastsDeleted()
    {
        await Clients.All.ForecastsDeleted();
    }
}
