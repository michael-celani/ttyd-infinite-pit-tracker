﻿using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace Celani.TTYD.Randomizer.API.Converters
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            if (value.TotalHours <= 0)
            {
                writer.WriteStringValue("00:00:00.00");
                return;
            }

            if (value.TotalHours >= 100)
            {
                writer.WriteStringValue("99:59:59.99");
                return;
            }
            
            Span<char> arr = stackalloc char[11];
            value.TotalHours.TryFormat(arr[0..2], out var charsWritten, "00");
            value.TryFormat(arr[2..], out charsWritten, @"\:mm\:ss\.ff");
            writer.WriteStringValue(arr);
        }
    }
}
