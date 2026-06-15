using Wholesale.Domain.Common;
using Wholesale.Domain.Enums;
using Wholesale.Domain.Exceptions;

namespace Wholesale.Domain.Entities;

/// <summary>
/// Агрегат "Заказ". Переходы статусов инкапсулированы в методах —
/// бизнес-правила живут в домене, а не в хендлерах.
/// </summary>
public class Order : AuditableEntity
{
    public Guid CustomerId { get; set; }
    public User? Customer { get; set; }

    public OrderStatus Status { get; private set; } = OrderStatus.AwaitingPayment;
    public decimal TotalAmount { get; private set; }

    public List<OrderItem> Items { get; set; } = [];

    public void AddItem(Product product, int quantity)
    {
        if (!product.IsAvailable)
            throw new DomainException($"Товар '{product.Name}' недоступен для заказа.");
        if (quantity <= 0)
            throw new DomainException("Количество должно быть больше нуля.");

        Items.Add(new OrderItem
        {
            OrderId = Id,
            ProductId = product.Id,
            Quantity = quantity,
            UnitPrice = product.Price
        });
        TotalAmount += product.Price * quantity;
    }

    public void Cancel()
    {
        if (Status != OrderStatus.AwaitingPayment)
            throw new DomainException("Отменить можно только неоплаченный заказ.");
        Status = OrderStatus.Cancelled;
    }

    public void MarkPaid()
    {
        if (Status != OrderStatus.AwaitingPayment)
            throw new DomainException("Оплатить можно только заказ, ожидающий оплаты.");
        Status = OrderStatus.Paid;
    }
}
