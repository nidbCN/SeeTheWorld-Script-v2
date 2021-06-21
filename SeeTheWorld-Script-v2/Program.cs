using Microsoft.Extensions.DependencyInjection;
using SeeTheWorld_Script_v2.Controllers;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeeTheWorld_Script_v2
{
    internal class Program
    {
        private static readonly Uri _cdnBase = new("https://img.cdn.gaein.cn/bing/");
        private static readonly Uri _apiBase = new("https://api.gaein.cn/SeeTheWorld/");
        private static readonly Uri _savePath = new(@"/home/www-data/wwwroot/cdn/img/bing");
        private static readonly JsonSerializerOptions _jsonOption = new() { WriteIndented = true };

        private static async Task Main()
        {
            var container = StartUp.ConfigServices();

            var controller = container.GetRequiredService<ScriptController>();

            try
            {
                await controller.RunScript();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
