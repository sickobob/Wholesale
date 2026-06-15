namespace Wholesale.Application.Security;

/// <summary>
/// Коды прав — единый источник истины: используются в атрибутах endpoint'ов,
/// кладутся в JWT-claims и сидируются в таблицу Permissions.
/// </summary>
public static class Permissions
{
    public const string ClaimType = "permission";

    public static class Users
    {
        public const string Manage = "users.manage";
    }

    public static class Products
    {
        public const string Read = "products.read";
        public const string Manage = "products.manage";
    }

    public static class Orders
    {
        public const string Create = "orders.create";
        public const string ViewOwn = "orders.view_own";
        public const string Cancel = "orders.cancel";
    }

    public static readonly IReadOnlyList<string> All =
    [
        Users.Manage,
        Products.Read, Products.Manage,
        Orders.Create, Orders.ViewOwn, Orders.Cancel
    ];

    /// <summary>Стандартный набор прав заказчика при регистрации (далее редактируется админом).</summary>
    public static readonly IReadOnlyList<string> DefaultCustomer =
    [
        Products.Read, Orders.Create, Orders.ViewOwn, Orders.Cancel
    ];

    public static readonly IReadOnlyList<string> DefaultAdministrator = All;
}
