using MediatR;
using Microsoft.AspNetCore.Mvc;
using Wholesale.Api.Auth;
using Wholesale.Application.Features.Products.CreateProduct;
using Wholesale.Application.Features.Products.DeleteProduct;
using Wholesale.Application.Features.Products.Dtos;
using Wholesale.Application.Features.Products.GetProducts;
using Wholesale.Application.Features.Products.UpdateProduct;
using Wholesale.Application.Security;

namespace Wholesale.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IMediator mediator) : ControllerBase
{
    /// <summary>Список товаров (для заказчиков — доступные к заказу).</summary>
    [HttpGet]
    [HasPermission(Permissions.Products.Read)]
    [ProducesResponseType<IReadOnlyList<ProductDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(
        [FromQuery] bool onlyAvailable, CancellationToken ct)
        => Ok(await mediator.Send(new GetProductsQuery(onlyAvailable), ct));

    /// <summary>Добавление товара.</summary>
    [HttpPost]
    [HasPermission(Permissions.Products.Manage)]
    [ProducesResponseType<ProductDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Create(CreateProductCommand command, CancellationToken ct)
    {
        var product = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { }, product);
    }

    /// <summary>Редактирование товара.</summary>
    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.Products.Manage)]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> Update(
        Guid id, UpdateProductRequest request, CancellationToken ct)
        => Ok(await mediator.Send(
            new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.IsAvailable), ct));

    /// <summary>Удаление товара (soft-delete).</summary>
    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.Products.Manage)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteProductCommand(id), ct);
        return NoContent();
    }
}

public sealed record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    bool IsAvailable);
