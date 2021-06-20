using System;

namespace SeeTheWorld_Script_v2.Services
{
    public interface IAliCdnService
    {
        public void Refresh(Uri url);
        public void Refresh(string url);
        public void Refresh(params Uri[] urls);
        public void Refresh(params string[] urls);
    }
}
