using Microsoft.Extensions.Options;
using SeeTheWorld_Script_v2.Models;
using System;
using System.Linq;

namespace SeeTheWorld_Script_v2.Services
{
    public class AliCdnService : IAliCdnService
    {
        private readonly AlibabaCloud.SDK.Cdn20180510.Client _client;

        public AliCdnService(IOptions<AliCdnServiceOption> options)
        {
            if (options is null)
                 throw new ArgumentNullException(nameof(options));

            _client = new AlibabaCloud.SDK.Cdn20180510.Client(
                new AlibabaCloud.OpenApiClient.Models.Config
                {
                    // AccessKey ID
                    AccessKeyId = options.Value.AccessKeyId,
                    // AccessKey Secret
                    AccessKeySecret = options.Value.AccessKeySecurity,
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

            _client.PushObjectCache(pushObjectCacheRequest);
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
    }
}
