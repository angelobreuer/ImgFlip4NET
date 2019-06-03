# ImgFlip4NET

A .NET wrapper for the [imgflip.com](https://imgflip.com) Meme Generator RESTful HTTP api.



## `Getting Started`

###### Prerequisites
 - imgflip.com-Account ([Get](https://imgflip.com/signup))
 - .NET Core >= 2.0 ([Get](https://dotnet.microsoft.com/download/dotnet-core/2.0))
 - Install the [ImgFlip4NET-NuGet Package](https://www.nuget.org/packages/ImgFlip4NET/)

###### Getting a meme template

```csharp
var service = new ImgFlipService(new ImgFlipOptions());
var template = await service.GetRandomMemeTemplateAsync();
```

###### Creating a new meme

```csharp
var service = new ImgFlipService(new ImgFlipOptions {
    Username = "[YOUR USERNAME]",
    Password = "[YOUR PASSWORD]"
});

var template = await service.GetRandomMemeTemplateAsync();
var meme = await service.CreateMemeAsync(template.Id, "some text here", "and some here...");

Console.WriteLine(meme.ImageUrl);
```

___

## `Dependencies`

- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) (used for HTTP payload deserialization)
