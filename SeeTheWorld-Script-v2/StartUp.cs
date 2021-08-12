using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SeeTheWorld_Script_v2.Controllers;
using SeeTheWorld_Script_v2.Models.Options;
using SeeTheWorld_Script_v2.Services;

namespace SeeTheWorld_Script_v2
{
    public class StartUp
    {
        public static IConfiguration Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

        public static ServiceProvider ConfigServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddEventSourceLogger();
                builder.AddConsole();
            });

            services.Configure<AliCdnOption>(Configuration.GetSection(nameof(AliCdnOption)));
            services.Configure<BingPictureOption>(Configuration.GetSection(nameof(BingPictureOption)));
            services.Configure<PictureApiOption>(Configuration.GetSection(nameof(PictureApiOption)));

            services.AddSingleton<IAliCdnService, AliCdnService>();

            services.AddSingleton<IBingPictureService, BingPictureService>();
            services.AddSingleton<IPictureApiService, PictureApiService>();

            services.AddHttpClient();

            services.AddTransient<ScriptController>();

            return services.BuildServiceProvider();
        }
    }


}
