using System.ComponentModel;

namespace Application.DTOs;

public record LoginRequest(
    // should add regex
    string UsernameOrEmail, 
    [property: PasswordPropertyText] string Password);
