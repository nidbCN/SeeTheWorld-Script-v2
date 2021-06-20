using Microsoft.Extensions.Options;
using SeeTheWorld_Script_v2.Models;
using SeeTheWorld_Script_v2.Models.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2.Services
{
    public class PictureApiService : IPictureApiService
    {
        private readonly IOptions<PictureApiOption> _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public PictureApiService(IOptions<PictureApiOption> options, IHttpClientFactory httpClientFactory)
        {
            _options = options
                       ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory
                                 ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task AddPictureAsync(BingPicture picture)
        {
            var dto = new
            {
                title = picture.Copyright,
                url = new Uri(new Uri(_options.Value.CdnBase), picture.StartDate).AbsoluteUri
            };

            var client = _httpClientFactory.CreateClient();

            await client.PostAsJsonAsync("",dto);

        }
    }
}
