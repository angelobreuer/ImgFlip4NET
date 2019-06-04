namespace ImgFlip4NET
{
    using Newtonsoft.Json;

    /// <summary>
    ///     Represents a meme.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed class Meme
    {
        /// <summary>
        ///     Gets the unique identifier of the meme.
        /// </summary>
        [JsonRequired, JsonProperty("id")]
        public int Id { get; internal set; }

        /// <summary>
        ///     Gets the name of the meme.
        /// </summary>
        [JsonRequired, JsonProperty("name")]
        public string Name { get; internal set; }

        /// <summary>
        ///     Gets the URL to the meme.
        /// </summary>
        [JsonRequired, JsonProperty("url")]
        public string Url { get; internal set; }

        /// <summary>
        ///     Gets the width of the meme image.
        /// </summary>
        [JsonRequired, JsonProperty("width")]
        public int Width { get; internal set; }

        /// <summary>
        ///     Gets the height of the meme image.
        /// </summary>
        [JsonRequired, JsonProperty("height")]
        public int Height { get; internal set; }

        /// <summary>
        ///     Gets the number of the meme boxes.
        /// </summary>
        [JsonProperty("box_count")]
        public int BoxCount { get; internal set; }
    }
}