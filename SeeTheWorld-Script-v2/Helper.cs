using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2
{
    public class Helper
    {
        private Uri BingBase => new("https://cn.bing.com/");

        private string BingApi => "HPImageArchive.aspx?format=js&n=1&pid=hp";

        private readonly string _storagePath;

        private readonly Uri _apiBase;


        private readonly JsonSerializerOptions _serializerOptions
            = new()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

        private HttpClient _httpClient;

        public HttpClient HttpClient
            => _httpClient ??= new HttpClient();


        public Helper(string storagePath, Uri apiBase)
        {
            _storagePath = storagePath
                          ?? throw new ArgumentNullException(nameof(storagePath));
            _apiBase = apiBase
                       ?? throw new ArgumentNullException(nameof(storagePath));
        }

        public async Task<Image> GetPictureInfoAsync()
        {
            Console.WriteLine("Start get image info from bing.");

            var url = new Uri(_bingBase, _bingApi);

            var resp = await HttpClient.GetFromJsonAsync<Model>(url, _serializerOptions);

            return resp?.Images?[0];
        }

        public async Task SavePictureAsync(Image picture, string path)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));
            var pictureResp
                = await HttpClient.GetAsync(new Uri(_bingBase, picture.Url));

            await File.WriteAllBytesAsync(
                path,
                await pictureResp.Content.ReadAsByteArrayAsync());
        }

        public async Task AddPictureToApiAsync(string fileName)
        {

        }

        public async Task FlushAliCdn(IEnumerable<string> urls)
        {

        }
    }
}
