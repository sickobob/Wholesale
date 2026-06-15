using AutoMapper;
using Wholesale.Application.Features.Customers.Dtos;
using Wholesale.Application.Features.Orders.Dtos;
using Wholesale.Application.Features.Products.CreateProduct;
using Wholesale.Application.Features.Products.Dtos;
using Wholesale.Domain.Entities;

namespace Wholesale.Application.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, CustomerDto>()
            .ForMember(d => d.Permissions,
                opt => opt.MapFrom(s => s.Permissions.Select(p => p.Permission!.Code)));

        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductCommand, Product>();

        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product!.Name));
    }
}
