using OnlineStore.BLL.DTOs;
using OnlineStore.BLL.Exceptions;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Repositories.Interfaces;

namespace OnlineStore.BLL.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    public const decimal CashOnDeliveryLimit = 100_000m;

    public async Task<OrderSummary> PlaceOrderAsync(int customerId, int addressId, IReadOnlyList<OrderLine> lines, PaymentMethod paymentMethod, CancellationToken cancellationToken = default)
    {
        if (lines.Count == 0)
        {
            throw new BusinessRuleException("Заказ должен содержать хотя бы одну позицию");
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var customer = await unitOfWork.Customers.GetWithAddressesAsync(customerId, cancellationToken)
                ?? throw new NotFoundException($"Покупатель с Id = {customerId} не найден");
            if (customer.Addresses.All(a => a.Id != addressId))
            {
                throw new BusinessRuleException("Адрес доставки не принадлежит покупателю");
            }

            var order = new Order { CustomerId = customerId, AddressId = addressId };
            foreach (var line in lines)
            {
                var product = await unitOfWork.Products.GetByIdAsync(line.ProductId, cancellationToken)
                    ?? throw new NotFoundException($"Товар с Id = {line.ProductId} не найден");
                if (product.StockQuantity < line.Quantity)
                {
                    throw new BusinessRuleException($"Недостаточно товара «{product.Name}» на складе");
                }

                product.StockQuantity -= line.Quantity;
                order.Items.Add(new OrderItem { ProductId = product.Id, Quantity = line.Quantity, UnitPrice = product.Price });
            }

            order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            await unitOfWork.Orders.AddAsync(order, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (paymentMethod == PaymentMethod.CashOnDelivery && order.TotalAmount > CashOnDeliveryLimit)
            {
                throw new BusinessRuleException($"Оплата при получении доступна для заказов до {CashOnDeliveryLimit:N0} ₽");
            }

            await unitOfWork.Payments.AddAsync(new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Method = paymentMethod,
                Status = PaymentStatus.Succeeded,
                PaidAt = DateTime.UtcNow
            }, cancellationToken);
            order.Status = OrderStatus.Paid;
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            return new OrderSummary(order.Id, order.Status.ToString(), order.TotalAmount, order.Items.Count);
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
