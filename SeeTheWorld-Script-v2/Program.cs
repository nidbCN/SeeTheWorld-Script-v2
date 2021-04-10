using System;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2
{
    class Program
    {


        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Helper httpHelper = new();

            await httpHelper.GetPictureInfo();
        }


    }
}
