using WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Contexts;

public class ForecastDbContext : DbContext
{
    public ForecastDbContext(DbContextOptions<ForecastDbContext> options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}
