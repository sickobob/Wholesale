using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wholesale.Application.Abstractions;
using Wholesale.Application.Common.Exceptions;
using Wholesale.Application.Features.Customers.Dtos;
using Wholesale.Application.Security;
using Wholesale.Domain.Entities;
using Wholesale.Domain.Enums;

namespace Wholesale.Application.Features.Customers.RegisterCustomer;

public sealed class RegisterCustomerHandler(
    IUnitOfWork uow,
    IPasswordGenerator passwordGenerator,
    IPasswordHasher passwordHasher,
    IEmailSender emailSender,
    IMapper mapper)
    : IRequestHandler<RegisterCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(RegisterCustomerCommand request, CancellationToken ct)
    {
        var login = request.Login.ToLowerInvariant();

        if (await uow.Users.Query().AnyAsync(u => u.Login == login, ct))
            throw new ConflictException($"Пользователь с логином '{login}' уже существует.");

        var password = passwordGenerator.Generate();

        var user = new User
        {
            Login = login,
            Name = request.Name,
            LegalAddress = request.LegalAddress,
            PasswordHash = passwordHasher.Hash(password),
            Role = UserRole.Customer
        };

        // базовый набор прав + возможные корректировки
        var codes = request.Permissions is { Count: > 0 }
            ? request.Permissions
            : Permissions.DefaultCustomer;

        var permissions = await uow.Permissions.Query()
            .Where(p => codes.Contains(p.Code))
            .ToListAsync(ct);
        user.ReplacePermissions(permissions);

        uow.Users.Add(user);
        await uow.SaveChangesAsync(ct);

        await emailSender.SendAsync(
            user.Login,
            "Доступ к платформе оптовых закупок",
            $"Ваш логин: {user.Login}\nВаш пароль: {password}",
            ct);

        return mapper.Map<CustomerDto>(user);
    }
}
