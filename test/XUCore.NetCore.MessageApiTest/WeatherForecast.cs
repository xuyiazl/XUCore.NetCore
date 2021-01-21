using MessagePack;
using System;

namespace XUCore.NetCore.MessageApiTest
{
    [MessagePackObject]
    public class WeatherForecast
    {
        [Key(0)]
        public DateTime? Date { get; set; }

        [Key(1)]
        public int TemperatureC { get; set; }

        [Key(2)]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Key(3)]
        public string Summary { get; set; }
    }
}
