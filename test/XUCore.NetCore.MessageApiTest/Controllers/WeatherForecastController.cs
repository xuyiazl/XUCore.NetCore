using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.Serializer;
using XUCore.NetCore;
using XUCore.NetCore.HttpFactory;
using XUCore.Webs;

namespace XUCore.NetCore.MessageApiTest.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(ILogger<WeatherForecastController> logger) : base(logger)
        {

        }
        [HttpGet]
        public Result<IList<WeatherForecast>> Get()
        {
            var rng = new Random();

            var list = new Result<IList<WeatherForecast>>
            {
                Code = 0,
                ElapsedTime = 12,
                SubCode = "0000001",
                Message = "成功啦",
                Data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
            .ToList()
            };

            //var reProps = new Dictionary<string, string> { { "code", "_code" },
            //        { "subCode", "_subCode" },
            //        { "bodyMessage", "data" },
            //        { "TemperatureC", "c" },
            //        { "Summary", "s" } };

            //var props = new string[] { "code", "subCode", "bodyMessage", "date", "TemperatureC", "Summary" };

            //var jsonSerializerSettings = new JsonSerializerSettings()
            //{
            //    DateTimeZoneHandling = DateTimeZoneHandling.Local,
            //    ContractResolver = new LimitPropsCamelCaseContractResolver(props, LimitPropsMode.Contains)
            //};

            //jsonSerializerSettings.Converters.Add(new DateTimeToUnixConverter());
            //jsonSerializerSettings.Converters.Add(new DateTimeNullToUnixConverter());

            //var json = list.ToJson(jsonSerializerSettings);

            //var rrr = json.ToObject<ReturnModel<IList<WeatherForecast>>>(jsonSerializerSettings);

            return list;
        }

        [HttpGet]
        public async Task<Result<List<WeatherForecast>>> GetDemo()
        {
            var url = UrlBuilder.Create("msgpack", "api/WeatherForecast/GetWeather");

            var res = await HttpRemote.Service.GetAsync<Result<List<WeatherForecast>>>(url,
                HttpMediaType.MessagePack, MessagePackSerializerResolver.UnixDateTimeOptions);

            return Success("0000001", "查询成功", res.Data);
        }

        [HttpGet]
        public Result<List<WeatherForecast>> GetWeather()
        {
            var rng = new Random();
            var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.UtcNow.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToList();

            return Success("0000001", "查询成功", res);
        }

        [HttpGet]
        public async Task<Result<WeatherForecast>> PostDemo()
        {
            var url = UrlBuilder.Create("msgpack", "api/WeatherForecast/PostWeather");

            var weather = new WeatherForecast
            {
                Date = DateTime.UtcNow,
                Summary = "测试POST",
                TemperatureC = 33
            };

            var res = await HttpRemote.Service.PostAsync<WeatherForecast, Result<WeatherForecast>>(url, weather,
                HttpMediaType.MessagePack, MessagePackSerializerResolver.UnixDateTimeOptions);

            return Success("0000001", "POST成功", res.Data);
        }

        [HttpPost]
        public Result<WeatherForecast> PostWeather([FromBody] WeatherForecast weatherForecast)
        {
            weatherForecast.Summary = "POST成功，接收后修改的Summary";

            return Success("0000001", "POST成功", weatherForecast);
        }

        [HttpGet]
        public async Task<Result<WeatherForecast>> PutDemo()
        {
            var url = UrlBuilder.Create("msgpack", "api/WeatherForecast/PutWeather");

            var weather = new WeatherForecast
            {
                Date = DateTime.UtcNow,
                Summary = "测试POST",
                TemperatureC = 33
            };

            var res = await HttpRemote.Service.PutAsync<WeatherForecast, Result<WeatherForecast>>(url, weather,
                HttpMediaType.MessagePack, MessagePackSerializerResolver.UnixDateTimeOptions);

            return Success("0000001", "POST成功", res.Data);
        }

        [HttpPut]
        public Result<WeatherForecast> PutWeather([FromBody] WeatherForecast weatherForecast)
        {
            weatherForecast.Summary = "PUT成功，接收后修改的Summary";

            return Success("0000001", "PUT成功", weatherForecast);
        }

        [HttpGet]
        public async Task<Result<WeatherForecast>> PatchDemo()
        {
            var url = UrlBuilder.Create("msgpack", "api/WeatherForecast/PatchWeather");

            var weather = new WeatherForecast
            {
                Date = DateTime.UtcNow,
                Summary = "测试patch",
                TemperatureC = 33
            };

            var res = await HttpRemote.Service.PatchAsync<WeatherForecast, Result<WeatherForecast>>(url, weather,
                HttpMediaType.MessagePack, MessagePackSerializerResolver.UnixDateTimeOptions);

            return Success("0000001", "PATCH成功", res.Data);
        }

        [HttpPatch]
        public Result<WeatherForecast> PatchWeather([FromBody] WeatherForecast weatherForecast)
        {
            weatherForecast.Summary = "Patch成功，接收后修改的Summary";

            return Success("0000001", "Patch成功", weatherForecast);
        }

        [HttpGet]
        public async Task<Result<WeatherForecast>> DeleteDemo()
        {
            var url = UrlBuilder.Create("msgpack", "api/WeatherForecast/DeleteWeather");

            var res = await HttpRemote.Service.DeleteAsync<Result<WeatherForecast>>(url,
                HttpMediaType.MessagePack, MessagePackSerializerResolver.UnixDateTimeOptions);

            return Success("0000001", "POST成功", res.Data);
        }

        [HttpDelete]
        public Result<WeatherForecast> DeleteWeather()
        {
            var weather = new WeatherForecast
            {
                Date = DateTime.UtcNow,
                Summary = "我是删除后返回的数据",
                TemperatureC = 11
            };

            return Success("0000001", "DELETE成功", weather);
        }
    }
}
