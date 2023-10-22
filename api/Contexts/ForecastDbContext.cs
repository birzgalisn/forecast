using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Contexts;

public class ForecastDbContext : DbContext
{
    public ForecastDbContext(DbContextOptions<ForecastDbContext> options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public DbSet<ExternalForecastProvider> ExternalForecastProviders { get; set; }
}
