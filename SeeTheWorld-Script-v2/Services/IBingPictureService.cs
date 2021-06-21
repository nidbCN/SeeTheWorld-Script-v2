using SeeTheWorld_Script_v2.Models;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2.Services
{
    public interface IBingPictureService
    {
        public BingPicture GetBingPictureAsync();
        public void StorageBingPictureAsync(BingPicture pictureInfo);
    }
}
