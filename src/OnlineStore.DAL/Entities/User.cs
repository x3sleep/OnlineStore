namespace OnlineStore.DAL.Entities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PassHash { get; set; } = string.Empty;
}
