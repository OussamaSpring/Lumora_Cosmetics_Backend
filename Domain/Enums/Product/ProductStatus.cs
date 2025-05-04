namespace Domain.Enums.Product;

public enum ProductStatus
{
    Draft = 1,
    Published = 2,
    Discontinued = 3 // Products with this status are no longer sold or produced. They won't be restocked or made available again.
}
