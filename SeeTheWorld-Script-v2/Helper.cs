using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2
{
    public class Helper
    {
        private readonly Uri _bingBase
            = new("https://cn.bing.com/");
        private readonly Uri _bingApi
            = new("HPImageAchive.aspx?format=js&n=1&pid=hp");


        private readonly JsonSerializerOptions _serializerOptions
            = new()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

        private HttpClient _httpClient;

        public HttpClient HttpClient => _httpClient ??= new HttpClient();

        public async Task<Image> GetPictureInfoAsync()
        {
            Console.WriteLine("Start get image info from bing.");

            var resp = await HttpClient.GetFromJsonAsync<Model>(new Uri(_bingBase, _bingApi));

            return resp?.Images?[0];
        }

        public async Task SavePictureAsync(Image picture)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));
            var pictureBlob = HttpClient.GetByteArrayAsync(new Uri(_bingBase, picture.Url));
        }
    }
}
