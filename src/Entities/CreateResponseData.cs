namespace ImgFlip4NET
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed class CreateResponseData
    {
        /// <summary>
        ///     Gets the URL to the meme page.
        /// </summary>
        [JsonRequired, JsonProperty("page_url")]
        public string PageUrl { get; internal set; }

        /// <summary>
        ///     Gets the URL to the meme image (JPG).
        /// </summary>
        [JsonRequired, JsonProperty("url")]
        public string ImageUrl { get; internal set; }
    }
}