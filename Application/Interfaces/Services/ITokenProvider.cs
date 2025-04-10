using Domain.Entities.AccountRelated;
using System.Security.Claims;

namespace Application.Interfaces.Services;

public interface ITokenProvider
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    ClaimsPrincipal GetPrincipalFromToken(string token);
}