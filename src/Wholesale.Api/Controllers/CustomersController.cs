using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wholesale.Api.Auth;
using Wholesale.Application.Features.Customers.DeleteCustomer;
using Wholesale.Application.Features.Customers.Dtos;
using Wholesale.Application.Features.Customers.GetCustomers;
using Wholesale.Application.Features.Customers.RegisterCustomer;
using Wholesale.Application.Features.Customers.UpdateCustomer;
using Wholesale.Application.Security;

namespace Wholesale.Api.Controllers;

[ApiController]
[Route("api/customers")]
[HasPermission(Permissions.Users.Manage)] // весь менеджмент заказчиков — только для админа
public sealed class CustomersController(IMediator mediator) : ControllerBase
{
    /// <summary>Список заказчиков.</summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CustomerDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(CancellationToken ct)
        => Ok(await mediator.Send(new GetCustomersQuery(), ct));

    /// <summary>Регистрация заказчика. Пароль генерируется и отправляется на почту.</summary>
    [HttpPost]
    [ProducesResponseType<CustomerDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CustomerDto>> Register(RegisterCustomerCommand command, CancellationToken ct)
    {
        var customer = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { }, customer);
    }

    /// <summary>Изменение профиля заказчика: имя, адрес, набор прав.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType<CustomerDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> Update(
        Guid id, UpdateCustomerRequest request, CancellationToken ct)
        => Ok(await mediator.Send(
            new UpdateCustomerCommand(id, request.Name, request.LegalAddress, request.Permissions), ct));

    /// <summary>Удаление профиля заказчика (soft-delete).</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteCustomerCommand(id), ct);
        return NoContent();
    }
}

/// <summary>Тело PUT-запроса без id (id берётся из маршрута).</summary>
public sealed record UpdateCustomerRequest(
    string Name,
    string LegalAddress,
    IReadOnlyList<string> Permissions);
