namespace Wholesale.Application.Abstractions;

public interface IPasswordGenerator
{
    string Generate(int length = 12);
}
