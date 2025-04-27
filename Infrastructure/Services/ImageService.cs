using Application.Interfaces.Services;
using Domain.Shared;

namespace Infrastructure.Services;

public class ImageService : IImageService
{
    public async Result<string?> Upload(IFormFile formFile)
    {

        if (formFile.Length > 0)
        {
            using var stream = System.IO.File.Create(filePath);
            await formFile.CopyToAsync(stream);
        }

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders
            .Add("Authentication", "e6ca182d213128a 53a314a06e8c9a9041bb1ea88fbd2952dd5c2a04");


        var content = new MultipartFormDataContent();
        var imageBytes = File.ReadAllBytes("C:\\Users\\adem\\Desktop\\7666-3-original.jpg");


        content.Add(new ByteArrayContent(imageBytes), "profile_photo", "7666-3-original.jpg");

        var respone = await client.PostAsync("https://api.imgur.com/3/upload", content);
        var url = await respone.Content.ReadAsStringAsync();

        return Result<string?>.Success(url);
    }
}
