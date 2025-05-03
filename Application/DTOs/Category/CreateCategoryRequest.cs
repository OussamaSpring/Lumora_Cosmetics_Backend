namespace Application.DTOs.Category;

public record CreateCategoryRequest(
    string Name,
    string? Description);