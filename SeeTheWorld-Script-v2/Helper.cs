using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AliCDNRefresher;

namespace SeeTheWorld_Script_v2
{
    public class Helper
    {
        public Uri FilePath { get; set; }
        public Uri BingUrl => new(@"https://cn.bing.com/HPImageArchive.aspx?format=js&n=1&pid=hp");


        private readonly string _storagePath;

        private readonly Uri _apiBase;

        private readonly Uri _cdnBase;

        private readonly JsonSerializerOptions _serializerOptions
            = new()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

        private HttpClient _httpClient;

        public HttpClient HttpClient
            => _httpClient ??= new HttpClient();


        public Helper(Uri storagePath, Uri apiBase, Uri cdnBase)
        {
            FilePath = new Uri(storagePath, $"{DateTime.Today:yyyyMMdd}.jpg")
                          ?? throw new ArgumentNullException(nameof(storagePath));
            _apiBase = apiBase
                       ?? throw new ArgumentNullException(nameof(storagePath));
            _cdnBase = cdnBase
                ?? throw new ArgumentNullException(nameof(cdnBase));
        }

        /// <summary>
        /// Get picture info from bing api.
        /// </summary>
        /// <returns>Picture info</returns>
        public async Task<Image> GetPictureInfoAsync()
        {
            var resp = await HttpClient.GetFromJsonAsync<Model>(BingUrl, _serializerOptions);

            return resp?.Images?[0];
        }

        /// <summary>
        /// Save picture file from bing to local.
        /// </summary>
        /// <param name="picture">picture info</param>
        /// <param name="fileName">the file name</param>
        /// <returns></returns>
        public async Task SavePictureAsync(Image picture, string fileName)
        {
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var pictureResp
                = await HttpClient.GetAsync(new Uri(BingBase, picture.Url));

            await File.WriteAllBytesAsync(
                Path.Combine(_storagePath, fileName),
                await pictureResp.Content.ReadAsByteArrayAsync());
        }

        /// <summary>
        /// Add the picture to database var api.
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="fileName"></param>
        public async Task<HttpResponseMessage> AddPictureToApiAsync(Image picture, string fileName)
            => await HttpClient.PostAsync(
                new Uri(_apiBase, "Pictures"),
                JsonContent.Create(new
                {
                    title = picture.Copyright,
                    url = new Uri(_cdnBase, fileName)
                })
                );

        /// <summary>
        /// Fresh
        /// </summary>
        /// <param name="fileName"></param>
        public void FreshAliCdn(string fileName)
            => new AliCdnReFresher(
                Util.ReadConfig()
            ).Refresh(new Uri(_cdnBase, fileName).AbsoluteUri);
    }

}
