using System;
using System.Threading.Tasks;
using SeeTheWorld_Script_v2.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace SeeTheWorld_Script_v2
{
    internal class Program
    {
        private static async Task Main()
        {
            var container = StartUp.ConfigServices();

            var controller = container.GetRequiredService<ScriptController>();

            await controller.RunScript();
        }
    }

}
