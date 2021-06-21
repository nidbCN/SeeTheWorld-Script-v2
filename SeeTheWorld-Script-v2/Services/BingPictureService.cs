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

        public BingPictureService(IOptions<BingPictureOption> options, IHttpClientFactory httpClientFactory)
        {
            _options = options
                ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory
                ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public BingPicture GetBingPictureAsync()
        {
            const string url = @"https://cn.bing.com/HPImageArchive.aspx?format=js&n=1&pid=hp";
            var httpClient = _httpClientFactory.CreateClient();
            var picture = (httpClient.GetFromJsonAsync<BingDeSerializeModel>(url)).Result.Images?[0];

            return picture;
        }

        public void StorageBingPictureAsync(BingPicture pictureInfo)
        {
            var fileName = $"{pictureInfo.StartDate}.jpg";
            var path = Path.Combine(_options.Value.StoragePath, fileName);

            var httpClient = _httpClientFactory.CreateClient();

            // ReSharper disable once ConvertToUsingDeclaration
            using (var stream = httpClient.GetStreamAsync($"https://cn.bing.com/{pictureInfo.Url}").Result)
            {
                // ReSharper disable once ConvertToUsingDeclaration
                using (var fileStream = File.Open(path, FileMode.Create))
                {
                    stream.CopyToAsync(fileStream);
                }
            }
        }
    }
}
