using System.Data;

namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public class MySqlCustomIdTypeHandler : SqlMapper.TypeHandler<CustomId>
{
    public override CustomId Parse(object value)
    {
        return value is byte[] bytes
            ? new(bytes)
            : throw new InvalidCastException($"Cannot convert {value?.GetType().FullName} to {typeof(CustomId).FullName}");
    }

    public override void SetValue(IDbDataParameter parameter, CustomId value)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        parameter.Value = value.ToByteArray();
    }
}
