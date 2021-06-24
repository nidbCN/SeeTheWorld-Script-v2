using SeeTheWorld_Script_v2.Models;
using SeeTheWorld_Script_v2.Services;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SeeTheWorld_Script_v2.Controllers
{
    public class ScriptController
    {
        private readonly IAliCdnService _aliCdnService;
        private readonly IBingPictureService _bingPictureService;
        private readonly IPictureApiService _pictureApiService;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly ILogger<ScriptController> _logger;

        public ScriptController(
            IAliCdnService aliCdnService,
            IBingPictureService bingPictureService,
            IPictureApiService pictureApiService,
            ILogger<ScriptController> logger
            )
        {
            _aliCdnService = aliCdnService
                ?? throw new ArgumentNullException(nameof(aliCdnService));
            _bingPictureService = bingPictureService
                ?? throw new ArgumentNullException(nameof(bingPictureService));
            _pictureApiService = pictureApiService
                ?? throw new ArgumentNullException(nameof(pictureApiService));
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));

            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
        }

        public async Task RunScript()
        {
            _logger.LogInformation($"Running method {nameof(RunScript)}.");


            _logger.LogInformation("Start query picture infomation from Bing.");
            var pictureInfo = _bingPictureService.GetBingPicture();

            _logger.LogInformation(JsonSerializer.Serialize(pictureInfo, _jsonSerializerOptions));
            _logger.LogInformation("Start save picture from Bing to local.");

            await _bingPictureService.StorageBingPictureAsync(pictureInfo);

            _logger.LogInformation("Start add picture to WebAPI.");

            await _pictureApiService.AddPictureAsync(pictureInfo);

            _logger.LogInformation("Refresh AliCDN.");
            _aliCdnService.RefreshByName(pictureInfo.FileName);
        }
    }
}
