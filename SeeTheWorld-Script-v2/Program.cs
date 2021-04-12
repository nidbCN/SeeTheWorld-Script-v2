using System;
using SeeTheWorld_Script_v2;

Uri cdnBase = new("https://img.cdn.gaein.cn/Bing/");
Uri apiBase = new("https://api.gaein.cn/SeeTheWorld/");
const string savePath = @"/data/img/bing/";

var httpHelper = new Helper(savePath, apiBase);

var pictureInfo = await httpHelper.GetPictureInfoAsync();

var fileName = DateTime.Today.ToString("yyyyMMdd") + ".jpg";

await httpHelper.SavePictureAsync(pictureInfo, fileName);

httpHelper.FreshAliCdn(cdnBase, fileName);