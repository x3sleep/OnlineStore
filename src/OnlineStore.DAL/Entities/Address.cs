namespace OnlineStore.DAL.Entities;

public class Address
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public bool IsDefault { get; set; }

    public Customer Customer { get; set; } = null!;
}
