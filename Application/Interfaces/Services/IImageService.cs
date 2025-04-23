using Domain.Shared;

namespace Application.Interfaces.Services;

public interface IImageService
{
    Result<string?> Upload(string imageId);
}
