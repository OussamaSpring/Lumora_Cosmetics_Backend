namespace Application.DTOs.Profile;

public record UpdateCredentialsRequest(
    string Email,
    string password);
