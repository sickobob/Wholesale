using Wholesale.Domain.Common;

namespace Wholesale.Domain.Entities;

public class OrderItem : AuditableEntity
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }

    /// <summary>Снапшот цены на момент заказа — изменение цены товара не влияет на старые заказы.</summary>
    public decimal UnitPrice { get; set; }

    public decimal Total => UnitPrice * Quantity;
}
