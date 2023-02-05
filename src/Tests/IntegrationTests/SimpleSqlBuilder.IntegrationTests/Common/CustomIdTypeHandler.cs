using System.Data;
using Dapper.SimpleSqlBuilder.IntegrationTests.Models;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

public class CustomIdTypeHandler : SqlMapper.TypeHandler<CustomId>
{
    public override CustomId Parse(object value)
    {
        return value is byte[] bytes
            ? new(bytes)
            : throw new InvalidCastException($"Cannot convert {value?.GetType().FullName} to {typeof(CustomId).FullName}");
    }

    public override void SetValue(IDbDataParameter parameter, CustomId value)
    {
        if (parameter is null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        parameter.Value = value.ToByteArray();
    }
}
