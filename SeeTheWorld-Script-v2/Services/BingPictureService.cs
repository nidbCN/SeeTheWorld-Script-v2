using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SeeTheWorld_Script_v2.Models;
using SeeTheWorld_Script_v2.Models.Options;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2.Services
{
    public class BingPictureService : IBingPictureService
    {
        private readonly IOptions<BingPictureOption> _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BingPictureService> _logger;

        public BingPictureService(
            IOptions<BingPictureOption> options,
            IHttpClientFactory httpClientFactory,
            ILogger<BingPictureService> logger
            )
        {
            _options = options
                ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory
                ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public BingPicture GetBingPicture()
        {
            const string url = @"https://cn.bing.com/HPImageArchive.aspx?format=js&n=1&pid=hp";
            var httpClient = _httpClientFactory.CreateClient();

            BingPicture resp;
            try
            {
                resp = httpClient.GetFromJsonAsync<BingDeSerializeModel>(url).Result.Images?[0];
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Cannot get picture information from Bing:{ex.Message}");
                throw ex;
            }

            return resp;
        }

        public async Task StorageBingPictureAsync(BingPicture pictureInfo)
        {
            var path = Path.Combine(
                _options.Value.StoragePath,
                pictureInfo.FileName
            );

            var httpClient = _httpClientFactory.CreateClient();

            using var stream = await httpClient.GetStreamAsync($"https://cn.bing.com/{pictureInfo.Url}");
            using var fileStream = new FileStream(path, FileMode.OpenOrCreate);

            await stream.CopyToAsync(fileStream);
        }
    }
}
