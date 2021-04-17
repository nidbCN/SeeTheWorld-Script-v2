using System;
using System.IO;
using System.Text.Json;
using SeeTheWorld_Script_v2;

Uri cdnBase = new("https://img.cdn.gaein.cn/Bing/");
Uri apiBase = new("https://api.gaein.cn/SeeTheWorld/");
var options = new JsonSerializerOptions()
{
    WriteIndented = true
};


const string savePath = @"/root/data/wwwroot/cdn/img/bing";

var httpHelper = new Helper(savePath, apiBase);

Console.WriteLine("Start get picture from bing.");
var pictureInfo = await httpHelper.GetPictureInfoAsync();
Console.WriteLine(JsonSerializer.Serialize(pictureInfo, options));

var fileName = DateTime.Today.ToString("yyyyMMdd") + ".jpg";
await httpHelper.SavePictureAsync(pictureInfo, fileName);
Console.WriteLine("Picture saved at:{0}", Path.Combine(savePath, fileName));

await httpHelper.AddPictureToApiAsync(pictureInfo, fileName);
Console.WriteLine("Add picture to Api.");

httpHelper.FreshAliCdn(cdnBase, fileName);
Console.WriteLine("AliCDN Refreshed.");

return 0;