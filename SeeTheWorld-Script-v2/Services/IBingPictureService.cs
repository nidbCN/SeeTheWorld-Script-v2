using SeeTheWorld_Script_v2.Models;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2.Services
{
    public interface IBingPictureService
    {
        public Task<BingPicture> GetBingPictureAsync();
        public Task StorageBingPictureAsync();
    }
}
