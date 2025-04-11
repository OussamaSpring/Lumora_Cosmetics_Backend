namespace Domain.Enums.enProduct;

public enum Gender
{
    Male = 1,
    Female = 2,
    Unisex = 3,
    Children = 4
}
public enum ProductStatus
{
    Draft = 1,
    Published = 2,
    Out_of_Stock = 3,
    Discontinued = 4 // Products with this status are no longer sold or produced. They won't be restocked or made available again.
}

public enum StockStatus
{
    In_Stock = 1,
    Low_Stock = 2,
    Out_of_Stock = 3,
    Pre_Order = 4
}