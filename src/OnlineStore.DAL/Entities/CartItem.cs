namespace OnlineStore.DAL.Entities;

public class CartItem
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public Customer Customer { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
