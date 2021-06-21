﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SeeTheWorld_Script_v2.Controllers;
using SeeTheWorld_Script_v2.Services;

namespace SeeTheWorld_Script_v2
{
    public class StartUp
    {
        public static IConfiguration Configuration => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

        public static IServiceProvider ConfigServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<ScriptController>();

            services.AddSingleton<IAliCdnService, AliCdnService>();
            services.AddHttpClient<IBingPictureService, BingPictureService>();
            services.AddHttpClient<IPictureApiService, PictureApiService>();

            return services.BuildServiceProvider();
        }
    }


}
