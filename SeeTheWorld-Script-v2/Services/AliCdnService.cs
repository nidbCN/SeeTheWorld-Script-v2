using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SeeTheWorld_Script_v2.Models.Options;
using System;
using System.Linq;
using Tea;

namespace SeeTheWorld_Script_v2.Services
{
    public class AliCdnService : IAliCdnService
    {
        private readonly AlibabaCloud.SDK.Cdn20180510.Client _client;
        private readonly IOptions<AliCdnOption> _options;
        private readonly ILogger<AliCdnService> _logger;

        public AliCdnService(IOptions<AliCdnOption> options, ILogger<AliCdnService> logger)
        {
            _options = options
                ?? throw new ArgumentNullException(nameof(options));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));

            // new aliCloud SDK CDN Client
            _client = new AlibabaCloud.SDK.Cdn20180510.Client(
                new AlibabaCloud.OpenApiClient.Models.Config
                {
                    // AccessKey ID
                    AccessKeyId = _options.Value.AccessKeyId,
                    // AccessKey Secret
                    AccessKeySecret = _options.Value.AccessKeySecret,
                    Endpoint = "cdn.aliyuncs.com",
                }
                );
        }


        public void Refresh(string url)
        {
            var pushObjectCacheRequest = new AlibabaCloud.SDK.Cdn20180510.Models.PushObjectCacheRequest()
            {
                ObjectPath = url
            };

            try
            {
                _client.PushObjectCache(pushObjectCacheRequest);
            }
            catch(TeaException ex)
            {
                _logger.LogError($"Cannot refresh AliCDN, please check your AccessKey and url configuration:\n{ex.Message}");
            }
        }

        public void Refresh(Uri url)
            => Refresh(url.AbsoluteUri);

        public void Refresh(params string[] urls)
        {
            var urlsData = string.Join('\n', urls);
            Refresh(urlsData);
        }

        public void Refresh(params Uri[] urls) =>
            Refresh(urls.Select(it => it.AbsoluteUri).ToArray());

        public void RefreshByName(string name) =>
            Refresh(new Uri(_options.Value.CdnBase, name));
    }
}
