using System;

namespace SeeTheWorld_Script_v2.Models.Options
{
    public class AliCdnOption
    {
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public Uri CdnBase { get; set; }
    }
}
