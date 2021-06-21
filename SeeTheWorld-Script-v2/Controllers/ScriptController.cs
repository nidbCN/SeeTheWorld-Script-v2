using SeeTheWorld_Script_v2.Models;
using SeeTheWorld_Script_v2.Services;
using System;
using System.Threading.Tasks;
using System.Text.Json;

namespace SeeTheWorld_Script_v2.Controllers
{
    public class ScriptController
    {
        private readonly IAliCdnService _aliCdnService;
        private readonly IBingPictureService _bingPictureService;
        private readonly IPictureApiService _pictureApiService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ScriptController(
            IAliCdnService aliCdnService,
            IBingPictureService bingPictureService,
            IPictureApiService pictureApiService
            )
        {
            _aliCdnService = aliCdnService
                ?? throw new ArgumentNullException(nameof(aliCdnService));
            _bingPictureService = bingPictureService
                ?? throw new ArgumentNullException(nameof(bingPictureService));
            _pictureApiService = pictureApiService
                ?? throw new ArgumentNullException(nameof(pictureApiService));

            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
        }

        public async Task RunScript()
        {
            var pictureInfo = _bingPictureService.GetBingPicture();

            Console.WriteLine("Get picture Info");
            Console.WriteLine(JsonSerializer.Serialize(pictureInfo, _jsonSerializerOptions));

            await _bingPictureService.StorageBingPictureAsync(pictureInfo);

            Console.WriteLine("Save Picture");

            await _pictureApiService.AddPictureAsync(pictureInfo);

            Console.WriteLine("Push to api");

            _aliCdnService.RefreshByName(pictureInfo.FileName);

            Console.WriteLine("Ref CDN");
        }
    }
}
