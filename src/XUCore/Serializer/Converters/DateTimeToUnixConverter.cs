using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.Timing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace XUCore.Serializer.Converters
{
    public class DateTimeToUnixConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, [AllowNull] DateTime existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null)
                return Timing.DateTimeExtensions.Date1970;
            if (reader.Value.GetType() == typeof(string))
                return DateTime.Parse(reader.Value.SafeString());
            else
                return Conv.ToLong(reader.Value).ToDateTime(DateTimeKind.Utc);
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] DateTime value, Newtonsoft.Json.JsonSerializer serializer)
            => writer.WriteValue(value.ToTimeStamp());
    }
}
