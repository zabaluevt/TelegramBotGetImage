namespace TelegramBotGetImage
{
    public class WeatherResponse
    {
        public string Name { get; set; }

        public TemperatureInfo Main { get; set; }
    }

    public class TemperatureInfo
    {
        public string temp { get; set; }

        public string temp_min { get; set; }

        public string temp_max { get; set; }
    }
}

