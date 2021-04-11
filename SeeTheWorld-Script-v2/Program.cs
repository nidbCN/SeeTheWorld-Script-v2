using System;
using SeeTheWorld_Script_v2;

Console.WriteLine("Hello World!");

var httpHelper = new Helper();

var pictureInfo = await httpHelper.GetPictureInfoAsync();
await httpHelper.SavePictureAsync(pictureInfo);
