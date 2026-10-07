namespace OnlineStore.BLL.DTOs;

public record OrderSummary(int Id, string Status, decimal TotalAmount, int ItemsCount);
