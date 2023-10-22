using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

public enum ForecastProvider
{
    OpenWeather = 0,
    WeatherApi = 1
}

[Index(nameof(Id), nameof(ForecastProvider))]
public class ExternalForecastProvider
{
    public int Id { get; set; }

    [Required]
    public ForecastProvider ForecastProvider { get; set; }

    [Required]
    public string Url { get; set; } = "";

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CalledAt { get; set; } = DateTime.UtcNow;
}
