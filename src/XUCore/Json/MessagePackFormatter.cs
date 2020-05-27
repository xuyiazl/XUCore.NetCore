using MessagePack;
using MessagePack.Formatters;
using XUCore.Helpers;
using XUCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Timing;
using MessagePack.Resolvers;

namespace XUCore.Json
{
    public static class MessagePackSerializerResolver
    {
        public static IFormatterResolver DateTimeFormatter
        {
            get
            {
                return CompositeResolver.Create(
                        new[] { new DurableDateTimeFormatter() },
                        new[] { ContractlessStandardResolver.Instance });
            }
        }

        public static MessagePackSerializerOptions DateTimeOptions
        {
            get
            {
                return ContractlessStandardResolver.Options.WithResolver(DateTimeFormatter);
            }

        }

        public static IFormatterResolver UnixDateTimeFormatter
        {
            get
            {
                return CompositeResolver.Create(
                        new[] { new DurableUnixDateTimeFormatter() },
                        new[] { ContractlessStandardResolver.Instance });

            }
        }

        public static MessagePackSerializerOptions UnixDateTimeOptions
        {
            get
            {
                return ContractlessStandardResolver.Options.WithResolver(UnixDateTimeFormatter);
            }
        }
    }

    public class DurableDateTimeFormatter : IMessagePackFormatter<DateTime>
    {
        public DateTime Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.NextMessagePackType == MessagePackType.String)
            {
                var str = reader.ReadString();

                return DateTime.Parse(str).ToUniversalTime();
            }
            else
            {
                return reader.ReadDateTime().ToUniversalTime();
            }
        }

        public void Serialize(ref MessagePackWriter writer, DateTime value, MessagePackSerializerOptions options)
        {
            writer.Write(value.ToUniversalTime());
        }
    }

    public class DurableUnixDateTimeFormatter : IMessagePackFormatter<DateTime>
    {
        public DateTime Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.NextMessagePackType == MessagePackType.Integer)
            {
                var d = reader.ReadInt64();

                return d.ToDateTime();
            }
            else
            {
                return reader.ReadDateTime();
            }
        }

        public void Serialize(ref MessagePackWriter writer, DateTime value, MessagePackSerializerOptions options)
        {
            writer.Write(value.ToTimeStamp());
        }
    }

}
