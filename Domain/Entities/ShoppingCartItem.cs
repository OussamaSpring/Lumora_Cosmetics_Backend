namespace Domain.Entities;

public class ShoppingCartItem
{
    public int ShoppingCartId { get; set; }
    public int ProductItemId { get; set; }
    public int Quantity { get; set; }
}