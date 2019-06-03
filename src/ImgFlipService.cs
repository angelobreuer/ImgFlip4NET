namespace ImgFlip4NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    ///     The service for making requests to the ImgFlip (https://api.imgflip.com) RESTful HTTP api.
    /// </summary>
    public sealed class ImgFlipService
    {
        private readonly Guid _boundary;
        private readonly HttpClient _httpClient;
        private readonly ImgFlipOptions _options;
        private readonly Random _random;
        private IReadOnlyList<Meme> _memes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImgFlipService"/> class.
        /// </summary>
        /// <param name="options">the options for the service</param>
        public ImgFlipService(ImgFlipOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ImgFlip4NET");
            _httpClient.BaseAddress = options.BaseAddress;

            _boundary = Guid.NewGuid();
            _random = new Random();
        }

        /// <summary>
        ///     Creates a new meme image asynchronously.
        /// </summary>
        /// <param name="templateId">the meme template id</param>
        /// <param name="topText">the top caption text for the meme</param>
        /// <param name="bottomText">the bottom caption text for the meme</param>
        /// <param name="fontFamily">the font family to use for the caption texts.</param>
        /// <param name="maxFontSize">
        ///     the max font size in pixels for texts written in the meme (default: 50px)
        /// </param>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the created
        ///     meme image info.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the API authentication credentials are not set in the options specified in
        ///     the constructor.
        /// </exception>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        public async Task<CreateResponseData> CreateMemeAsync(int templateId, string topText, string bottomText,
            ImageFontFamily fontFamily = ImageFontFamily.Default, int maxFontSize = 50,
            CancellationToken cancellationToken = default)
        {
            EnsureCredentialsAvailable();

            cancellationToken.ThrowIfCancellationRequested();

            // the request content is automatically disposed by the HTTP client after the request.
            var httpContent = new MultipartFormDataContent(_boundary.ToString())
            {
                { new StringContent(templateId.ToString()), "template_id" },
                { new StringContent(_options.Username),"username" },
                { new StringContent(_options.Password), "password" },
                { new StringContent(topText), "text0" },
                { new StringContent(bottomText), "text1" }
            };

            // add the font family parameter to the query string if not default
            if (fontFamily != ImageFontFamily.Default)
            {
                httpContent.Add(new StringContent(fontFamily.ToString()), "font");
            }

            // add the max font size parameter to the query string if not default
            if (maxFontSize != 50)
            {
                httpContent.Add(new StringContent(maxFontSize.ToString()), "max_font_size");
            }

            using (var response = await _httpClient.PostAsync("caption_image", httpContent, cancellationToken))
            {
                return await ProcessResponseAsync<CreateResponseData>(response);
            }
        }

        /// <summary>
        ///     Finds a meme by the specified <paramref name="predicate"/> asynchronously.
        /// </summary>
        /// <param name="predicate">the meme search predicate</param>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the meme found
        ///     by the specified <paramref name="predicate"/>; or <see langword="null"/> if no meme
        ///     was found.
        /// </returns>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        public async Task<Meme> FindMemeTemplateAsync(Func<Meme, bool> predicate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var memes = await GetMemeTemplatesAsync(cancellationToken);

            if (memes == null)
            {
                return default;
            }

            return memes.FirstOrDefault(predicate);
        }

        /// <summary>
        ///     Finds a meme by the specified <paramref name="name"/> asynchronously.
        /// </summary>
        /// <param name="name">the meme name</param>
        /// <param name="comparison">the string comparison type</param>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the meme found
        ///     by the specified <paramref name="name"/>; or <see langword="null"/> if no meme was found.
        /// </returns>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        public Task<Meme> GetMemeTemplateAsync(string name, CancellationToken cancellationToken = default,
            StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
            => FindMemeTemplateAsync(m => m.Name.Equals(name, comparison), cancellationToken);

        /// <summary>
        ///     Finds a meme by the specified <paramref name="id"/> asynchronously.
        /// </summary>
        /// <param name="id">the meme id</param>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the meme found
        ///     by the specified <paramref name="id"/>; or <see langword="null"/> if no meme was found.
        /// </returns>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        public Task<Meme> GetMemeTemplateAsync(int id, CancellationToken cancellationToken = default)
            => FindMemeTemplateAsync(m => m.Id == id, cancellationToken);

        /// <summary>
        ///     Gets a random meme template asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the meme template.
        /// </returns>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        public async Task<Meme> GetRandomMemeTemplateAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var memes = await GetMemeTemplatesAsync(cancellationToken);

            if (memes == null)
            {
                return default;
            }

            return memes[_random.Next(memes.Count)];
        }

        /// <summary>
        ///     Gets the most-captioned memes in the last 30 days ordered by how many times they were
        ///     captioned asynchronously. (cached for 2 hours).
        /// </summary>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the meme list
        ///     that was returned by the HTTP response.
        /// </returns>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        public async Task<IReadOnlyList<Meme>> GetMemeTemplatesAsync(CancellationToken cancellationToken = default)
            => _memes ?? (_memes = await DownloadMemeTemplatesAsync(cancellationToken));

        /// <summary>
        ///     Gets the most-captioned memes in the last 30 days ordered by how many times they were
        ///     captioned asynchronously.
        /// </summary>
        /// <param name="cancellationToken">
        ///     a cancellation token used to propagate notification that the asynchronous operation
        ///     should be canceled.
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the meme list
        ///     that was returned by the HTTP response.
        /// </returns>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        private async Task<IReadOnlyList<Meme>> DownloadMemeTemplatesAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var response = await _httpClient.GetAsync("get_memes", cancellationToken))
            {
                return (await ProcessResponseAsync<MemeResponseData>(response))?.Memes;
            }
        }

        /// <summary>
        ///     Ensures that the credentials for the ImgFlip api are set in the options specified in
        ///     the constructor.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     thrown if the API authentication credentials are not set in the options specified in
        ///     the constructor.
        /// </exception>
        private void EnsureCredentialsAvailable()
        {
            if (string.IsNullOrWhiteSpace(_options.Username) || string.IsNullOrWhiteSpace(_options.Password))
            {
                throw new InvalidOperationException("This resource needs an username and password specified in the options.");
            }
        }

        /// <summary>
        ///     Processes the specified <paramref name="response"/> asynchronously.
        /// </summary>
        /// <typeparam name="TData">the type of the response data</typeparam>
        /// <param name="response">the response</param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the
        ///     deserialized response data.
        /// </returns>
        /// <exception cref="Exception">thrown if the request to the ImgFlip api failed.</exception>
        private async Task<TData> ProcessResponseAsync<TData>(HttpResponseMessage response) where TData : class, new()
        {
            var content = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<ImgFlipResponse<TData>>(content);

            if (!payload.IsSuccess)
            {
                // exception suppression is enabled, return NULL instead.
                if (_options.SuppressExceptions)
                {
                    return null;
                }

                throw new Exception($"A request to the ImgFlip api failed: {response.StatusCode} (HTTP ERROR {(int)response.StatusCode})" +
                    $"\n\tError Message: {payload.ErrorMessage}\n\tPayload Content: {content}");
            }

            return payload.Data;
        }
    }
}