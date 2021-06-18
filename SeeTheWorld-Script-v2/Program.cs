using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2
{
    internal class Program
    {
        private static readonly Uri _cdnBase = new("https://img.cdn.gaein.cn/bing/");
        private static readonly Uri _apiBase = new("https://api.gaein.cn/SeeTheWorld/");
        private static readonly Uri _savePath = new(@"/home/www-data/wwwroot/cdn/img/bing");
        private static readonly JsonSerializerOptions _jsonOption = new () { WriteIndented = true };

        private static async Task Main()
        {
            var httpHelper = new Helper(_savePath, _apiBase, _cdnBase);

            Console.WriteLine("Start get picture from bing.");
            var pictureInfo = await httpHelper.GetPictureInfoAsync();
            Console.WriteLine(JsonSerializer.Serialize(pictureInfo, _jsonOption));

            await httpHelper.SavePictureAsync(pictureInfo);
            Console.WriteLine("Picture saved at:{0}", httpHelper.FilePath);

            var result = await httpHelper.AddPictureToApiAsync(pictureInfo);
            Console.WriteLine("Add picture to Api.");
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                result.StatusCode,
                Message = result.ReasonPhrase,
                IsSuccess = result.IsSuccessStatusCode,
            }, _jsonOption));

            httpHelper.FreshAliCdn();
            Console.WriteLine("AliCDN Refreshed.");

        }
    }

}
