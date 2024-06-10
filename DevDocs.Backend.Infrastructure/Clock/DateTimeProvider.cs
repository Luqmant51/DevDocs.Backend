using DevDocs.Backend.Application.Abstractions.Clock;

namespace DevDocs.Backend.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
