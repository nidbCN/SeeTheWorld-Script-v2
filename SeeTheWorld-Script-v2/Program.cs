using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using SeeTheWorld_Script_v2;

Uri cdnBase = new("https://img.cdn.gaein.cn/bing/");
Uri apiBase = new("https://api.gaein.cn/SeeTheWorld/");

var options = new JsonSerializerOptions()
{
    WriteIndented = true
};


const string savePath = @"/home/www-data/wwwroot/cdn/img/bing";

var httpHelper = new Helper(savePath, apiBase, cdnBase);

Console.WriteLine("Start get picture from bing.");
var pictureInfo = await httpHelper.GetPictureInfoAsync();
Console.WriteLine(JsonSerializer.Serialize(pictureInfo, options));

var fileName = DateTime.Today.ToString("yyyyMMdd") + ".jpg";
await httpHelper.SavePictureAsync(pictureInfo, fileName);
Console.WriteLine("Picture saved at:{0}", Path.Combine(savePath, fileName));

var result = await httpHelper.AddPictureToApiAsync(pictureInfo, fileName);
Console.WriteLine("Add picture to Api.");
Console.WriteLine(JsonSerializer.Serialize<HttpResponseMessage>(result, options));

httpHelper.FreshAliCdn(fileName);
Console.WriteLine("AliCDN Refreshed.");

return 0;
