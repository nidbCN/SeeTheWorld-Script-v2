using SeeTheWorld_Script_v2.Models;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2.Services
{
    public interface IPictureApiService
    {
        public Task AddPictureAsync(BingPicture picture);
    }
}
