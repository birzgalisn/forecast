using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Models;

[Index(nameof(Id))]
public class WeatherForecast
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = "";

    [Required]
    public double Lat { get; set; }

    [Required]
    public double Lng { get; set; }

    [Required]
    public double TemperatureC { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
