namespace OnlineStore.DAL.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public List<Address> Addresses { get; set; } = [];
    public List<CartItem> CartItems { get; set; } = [];
    public List<Order> Orders { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
}
