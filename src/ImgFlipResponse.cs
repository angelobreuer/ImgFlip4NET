namespace ImgFlip4NET
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed class ImgFlipResponse<TData>
        where TData : class, new()
    {
        /// <summary>
        ///     Gets a value indicating whether the request succeed.
        /// </summary>
        [JsonRequired, JsonProperty("success")]
        public bool IsSuccess { get; internal set; }

        /// <summary>
        ///     Gets the response data.
        /// </summary>
        [JsonProperty("data")]
        public TData Data { get; internal set; }

        /// <summary>
        ///     Gets the response error message with an error why the response failed. (Only
        ///     available is <see cref="IsSuccess"/> is <see langword="false"/>.)
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; internal set; }
    }
}