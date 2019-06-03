namespace ImgFlip4NET
{
    using System;
    using System.Drawing;
    using Newtonsoft.Json;

    internal sealed class HexColorConverter : JsonConverter<Color>
    {
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
            => throw new NotSupportedException("This Json Converter does not support reading.");

        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
            => writer.WriteValue($"#{value.R.ToString("X2")}{value.G.ToString("X2")}{value.B.ToString("X2")}");
    }
}