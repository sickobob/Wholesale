using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Features.Products.Dtos;

namespace Wholesale.Application.Features.Products.GetProducts;

public sealed class GetProductsHandler(IUnitOfWork uow, IMapper mapper)
    : IRequestHandler<GetProductsQuery, IReadOnlyList<ProductDto>>
{
    public async Task<IReadOnlyList<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var query = uow.Products.Query();
        if (request.OnlyAvailable)
            query = query.Where(p => p.IsAvailable);

        return await query
            .OrderBy(p => p.Name)
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }
}
