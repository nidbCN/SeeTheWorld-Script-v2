using AliCDNRefresher;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2
{
    public class Helper
    {
        public Uri FilePath { get; set; }
        public Uri ApiUrl { get; set; }
        public Uri CdnUrl { get; set; }
        public static Uri BingUrl => new(@"https://cn.bing.com/HPImageArchive.aspx?format=js&n=1&pid=hp");

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
            if (storagePath is null) throw new ArgumentNullException(nameof(storagePath));
            if (apiBase is null) throw new ArgumentNullException(nameof(apiBase));
            if (cdnBase is null) throw new ArgumentNullException(nameof(cdnBase));

            var fileName = $"{DateTime.Today:yyyyMMdd}.jpg";

            FilePath = new Uri(storagePath, fileName);

            ApiUrl = new Uri(apiBase, "Pictures");

            CdnUrl = new Uri(cdnBase, fileName);
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
        /// <returns></returns>
        public async Task SavePictureAsync(Image picture)
        {
            if (picture is null)
                throw new ArgumentNullException(nameof(picture));

            var pictureResp
                = await HttpClient.GetAsync(BingUrl);

            await File.WriteAllBytesAsync(
                FilePath.AbsolutePath,
                await pictureResp.Content.ReadAsByteArrayAsync());
        }

        /// <summary>
        /// Add the picture to database var api.
        /// </summary>
        /// <param name="picture"></param>
        public async Task<HttpResponseMessage> AddPictureToApiAsync(Image picture)
            => await HttpClient.PostAsync(
                ApiUrl,
                JsonContent.Create(new
                {
                    title = picture.Copyright,
                    url = CdnUrl
                })
                );

        /// <summary>
        /// Fresh
        /// </summary>
        public void FreshAliCdn()
            => new AliCdnReFresher(
                Util.ReadConfig()
            ).Refresh(CdnUrl.AbsoluteUri);
    }

}
