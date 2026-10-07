namespace OnlineStore.DAL.Entities;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int AddressId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.New;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Customer Customer { get; set; } = null!;
    public Address Address { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = [];
    public Payment? Payment { get; set; }
    public Shipment? Shipment { get; set; }
}
