using Microsoft.Extensions.Logging;
using Wholesale.Application.Abstractions;

namespace Wholesale.Infrastructure.Services;

/// <summary>
/// Заглушка для тестового задания: вместо SMTP пишет письмо в лог.
/// В проде заменяется реализацией на MailKit/SendGrid — потребители не меняются.
/// </summary>
public sealed class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        logger.LogInformation("EMAIL -> {To} | {Subject}\n{Body}", to, subject, body);
        return Task.CompletedTask;
    }
}
