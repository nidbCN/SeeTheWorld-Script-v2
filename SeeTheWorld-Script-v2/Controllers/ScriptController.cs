using SeeTheWorld_Script_v2.Models;
using SeeTheWorld_Script_v2.Services;
using System;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2.Controllers
{
    public class ScriptController
    {
        private readonly IAliCdnService _aliCdnService;
        private readonly IBingPictureService _bingPictureService;
        private readonly IPictureApiService _pictureApiService;

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
        }

        public async Task RunScript()
        {
            var pictureInfo = _bingPictureService.GetBingPicture();
            await _bingPictureService.StorageBingPictureAsync(pictureInfo);
            await _pictureApiService.AddPictureAsync(pictureInfo);
            _aliCdnService.RefreshByName(pictureInfo.FileName);
        }
    }
}
