namespace Wholesale.Domain.Enums;

/// <summary>
/// Статусная модель заказа:
/// AwaitingPayment -> Paid -> Shipped -> Completed
/// AwaitingPayment -> Cancelled (отмена возможна только до оплаты)
/// </summary>
public enum OrderStatus
{
    AwaitingPayment = 1,
    Paid = 2,
    Shipped = 3,
    Completed = 4,
    Cancelled = 5
}
