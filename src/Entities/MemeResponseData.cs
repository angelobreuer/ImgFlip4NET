namespace ImgFlip4NET
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed class MemeResponseData
    {
        /// <summary>
        ///     Gets the list of memes returned in the response.
        /// </summary>
        [JsonRequired, JsonProperty("memes")]
        public IReadOnlyList<Meme> Memes { get; internal set; }
    }
}