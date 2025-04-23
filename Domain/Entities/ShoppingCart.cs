namespace Domain.Entities;

public class ShoppingCart
{
    public int Id { get; set; }
    public Guid customer_id { get; set; }
    List<ShoppingCartItem> items { get; set; } = new List<ShoppingCartItem>();
}
