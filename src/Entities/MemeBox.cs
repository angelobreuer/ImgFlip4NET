namespace ImgFlip4NET
{
    using System.Drawing;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed class MemeBox
    {
        [JsonRequired, JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("x", NullValueHandling = NullValueHandling.Ignore)]
        public int? X { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public int? Y { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public int? Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public int? Height { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(HexColorConverter))]
        public Color? Color { get; set; }

        [JsonProperty("outline_color", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(HexColorConverter))]
        public Color? OutlineColor { get; set; }
    }
}