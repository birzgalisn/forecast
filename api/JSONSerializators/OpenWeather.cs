using System.Text.Json.Serialization;

namespace Api.JSONSerializators;

public class OpenWeather
{
    [JsonPropertyName("weather")]
    public List<WeatherInfo>? Weather { get; set; }

    [JsonPropertyName("main")]
    public MainInfo? Main { get; set; }

    [JsonPropertyName("wind")]
    public WindInfo? Wind { get; set; }

    [JsonPropertyName("clouds")]
    public CloudInfo? Clouds { get; set; }

    public class WeatherInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string? Main { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Deg { get; set; }

        [JsonPropertyName("gust")]
        public double Gust { get; set; }
    }

    public class CloudInfo
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }
}
