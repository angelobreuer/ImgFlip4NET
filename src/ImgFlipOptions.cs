namespace ImgFlip4NET
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    ///     The service options for the <see cref="ImgFlipService"/>.
    /// </summary>
    public sealed class ImgFlipOptions
    {
        /// <summary>
        ///     Gets or sets the account username for requests to the ImgFlip api. (Note this is only
        ///     required for <see cref="ImgFlipService.CreateImageAsync(int, string, string, ImageFontFamily, int, CancellationToken)"/>.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the account password for requests to the ImgFlip api. (Note this is only
        ///     required for <see cref="ImgFlipService.CreateImageAsync(int, string, string, ImageFontFamily, int, CancellationToken)"/>.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Gets or sets the base address of the ImgFlip RESTful HTTP api.
        /// </summary>
        public Uri BaseAddress { get; set; } = new Uri("https://api.imgflip.com/");

        /// <summary>
        ///     Gets or sets a value indicating whether API request failures should be suppressed and
        ///     <see langword="null"/> should be returned instead.
        /// </summary>
        public bool SuppressExceptions { get; set; }
    }
}