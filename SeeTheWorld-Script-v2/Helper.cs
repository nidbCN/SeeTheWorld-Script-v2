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

        /// <summary>
        /// Get picture info from bing api.
        /// </summary>
        /// <returns>Picture info</returns>
        public async Task<Image> GetPictureInfoAsync()
        {
            Console.WriteLine("Start get image info from bing.");

            var url = new Uri(BingBase, BingApi);

            var resp = await HttpClient.GetFromJsonAsync<Model>(url, _serializerOptions);

            return resp?.Images?[0];
        }

        /// <summary>
        /// Save picture file from bing to local.
        /// </summary>
        /// <param name="picture">picture info</param>
        /// <param name="path">path to save the picture, include the file name.</param>
        /// <returns></returns>
        public async Task SavePictureAsync(Image picture, string path)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));
            var pictureResp
                = await HttpClient.GetAsync(new Uri(BingBase, picture.Url));

            await File.WriteAllBytesAsync(
                path,
                await pictureResp.Content.ReadAsByteArrayAsync());
        }

        /// <summary>
        /// Add the picture to database var api.
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task AddPictureToApiAsync(Image picture, string fileName)
            => await HttpClient.PostAsync(_apiBase,
                    new StringContent(
                        JsonSerializer.Serialize(new
                        {
                            title = picture.Copyright,
                            url = new Uri(_apiBase, fileName)
                        })
                    )
                );

        public async Task FlushAliCdn(IEnumerable<string> urls)
        {

        }
    }
}
