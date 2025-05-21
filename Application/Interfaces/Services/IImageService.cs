using Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services;

public interface IImageService
{
    Task<Result<string?>> Upload(IFormFile file);
}
