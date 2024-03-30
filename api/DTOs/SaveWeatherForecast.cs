namespace Api.DTOs;

public class SaveWeatherForecast
{
    public string Location { get; set; } = "";

    public double TemperatureC { get; set; }

    public double Lat { get; set; }

    public double Lng { get; set; }
}
