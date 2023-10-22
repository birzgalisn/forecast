using System.Text.Json;
using Api.Contexts;
using Api.Models;

namespace Api.Clients;

public interface IForecastAggregationClient
{
    T? GetForecast<T>(ForecastProvider provider, string url);
}

public class ForecastAggregationClient : IForecastAggregationClient
{
    private readonly HttpClient _client;
    private readonly ILogger<ForecastAggregationClient> _logger;
    private readonly IServiceScope _scope;
    private readonly ForecastDbContext _dbContext;

    public ForecastAggregationClient(ILogger<ForecastAggregationClient> logger, IServiceProvider serviceProvider)
    {
        _client = new();
        _logger = logger;
        _scope = serviceProvider.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ForecastDbContext>();
    }

    public T? GetForecast<T>(ForecastProvider provider, string url)
    {
        T? weatherResponse = default;

        try
        {
            var fetchThread = new Thread(() =>
            {
                var response = _client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    weatherResponse = JsonSerializer.Deserialize<T>(responseBody);
                }
            });

            fetchThread.Start();

            _dbContext.ExternalForecastProviders.Add(new ExternalForecastProvider()
            {
                ForecastProvider = provider,
                Url = url
            });

            fetchThread.Join();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Forecast aggregation client failed to fetch {url}: {ex.Message}");
        }
        finally
        {
            _dbContext.SaveChanges();
        }

        return weatherResponse;
    }
}
