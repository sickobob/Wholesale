using System.Security.Cryptography;
using Wholesale.Application.Abstractions;

namespace Wholesale.Infrastructure.Services;

public sealed class PasswordGenerator : IPasswordGenerator
{
    private const string Alphabet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@#$%";

    public string Generate(int length = 12)
        => RandomNumberGenerator.GetString(Alphabet, length);
}
