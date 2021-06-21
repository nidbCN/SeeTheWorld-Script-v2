using System;

namespace SeeTheWorld_Script_v2.Extensions
{
    public static class UriExtension
    {
        public static Uri Combine(this Uri uri, string uriAppend)
        {
            var uriStr = uri.AbsoluteUri;

            return new Uri(
                $"{uriStr}{(!uriStr.EndsWith('/') ? '/' : "")}{uriAppend}"
            );
        }
    }
}
