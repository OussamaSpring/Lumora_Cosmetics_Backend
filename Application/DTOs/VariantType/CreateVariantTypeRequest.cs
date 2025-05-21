namespace Application.DTOs.VariantType;

public record CreateVariantTypeRequest(
    string Name,
    string? Description);
