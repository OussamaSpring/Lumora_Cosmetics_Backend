using Application.Interfaces.Services;
using Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class ImageService : IImageService
{
    public async Task<Result<string?>> Upload(IFormFile file)
    {
        const int maxSize = 10 * 1024 * 1024;
        if (file.Length == 0)
            return Result<string?>.Failure(new Error("Upload", "file is empty"));
        
        if (!file.ContentType.StartsWith("image/"))
            return Result<string?>.Failure(new Error("Upload", "file is not image"));

        if (file.Length > maxSize)
            return Result<string?>.Failure(new Error("Upload", "max length of photo is 10MB"));


        var imageBytes = await FormFileToByteArrayAsync(file);
        //using HttpClient client = new HttpClient();
        //client.DefaultRequestHeaders
        //    .Add("Authentication", "e6ca182d213128a d1f88705964dbbf4616cd6b07c1952d5eeb31a47");

        //var content = new MultipartFormDataContent();
        //content.Add(new ByteArrayContent(imageBytes));

        //var response = await client.PostAsync("https://api.imgur.com/3/upload", content);

        //if (response is not null && response.StatusCode == HttpStatusCode.OK)
        //{
        //    var url = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine(url);
        //    return Result<string?>.Success(url);
        //}

        var imageDirectory = "images";
        Directory.CreateDirectory(imageDirectory);

        var extension = file.ContentType.Substring(file.ContentType.IndexOf('/') + 1);
        var photoName = file.ContentType.Substring(0, file.ContentType.IndexOf('/'));

        string path = $"{imageDirectory}/{Guid.NewGuid().ToString("N")}.{extension}";
        File.WriteAllBytes(path, imageBytes);

        return Result<string?>.Success(path);
    }

    private async Task<byte[]> FormFileToByteArrayAsync(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
