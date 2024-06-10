using System.Data;

namespace DevDocs.Backend.Domain.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
