namespace OnlineStore.DAL.Entities;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }

    public List<Product> Products { get; set; } = [];
}
