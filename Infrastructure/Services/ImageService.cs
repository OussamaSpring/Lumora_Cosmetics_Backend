using Application.Interfaces.Services;
using Domain.Shared;

namespace Infrastructure.Services;

public class ImageService : IImageService
{
    public Result<string?> Upload(string imageId)
    {

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders
            .Add("Authentication", "e6ca182d213128a 53a314a06e8c9a9041bb1ea88fbd2952dd5c2a04");



        return Result<string?>.Success(string.Empty);
    }
}
