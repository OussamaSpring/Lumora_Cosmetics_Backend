﻿namespace Application.DTOs.Category;

public record UpdateCategoryRequest(
    string Name,
    string? Description);