﻿namespace Domain.Entities.ProductRelated;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    //public Category? Parent { get; set; }
}