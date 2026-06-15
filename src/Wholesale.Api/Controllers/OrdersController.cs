using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wholesale.Api.Auth;
using Wholesale.Application.Features.Orders.CancelOrder;
using Wholesale.Application.Features.Orders.CreateOrder;
using Wholesale.Application.Features.Orders.Dtos;
using Wholesale.Application.Features.Orders.GetMyOrders;
using Wholesale.Application.Security;

namespace Wholesale.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(IMediator mediator) : ControllerBase
{
    /// <summary>Создание заказа из выбранных позиций.</summary>
    [HttpPost]
    [HasPermission(Permissions.Orders.Create)]
    [ProducesResponseType<OrderDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderCommand command, CancellationToken ct)
    {
        var order = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetMy), new { }, order);
    }

    /// <summary>Список своих заказов с детализацией по позициям.</summary>
    [HttpGet("my")]
    [HasPermission(Permissions.Orders.ViewOwn)]
    [ProducesResponseType<IReadOnlyList<OrderDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetMy(CancellationToken ct)
        => Ok(await mediator.Send(new GetMyOrdersQuery(), ct));

    /// <summary>Отмена неоплаченного заказа.</summary>
    [HttpPost("{id:guid}/cancel")]
    [HasPermission(Permissions.Orders.Cancel)]
    [ProducesResponseType<OrderDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<OrderDto>> Cancel(Guid id, CancellationToken ct)
        => Ok(await mediator.Send(new CancelOrderCommand(id), ct));
}
