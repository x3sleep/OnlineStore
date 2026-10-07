using OnlineStore.BLL.DTOs;
using OnlineStore.DAL.Entities;

namespace OnlineStore.BLL.Services;

public interface IOrderService
{
    Task<OrderSummary> PlaceOrderAsync(int customerId, int addressId, IReadOnlyList<OrderLine> lines, PaymentMethod paymentMethod, CancellationToken cancellationToken = default);
}
