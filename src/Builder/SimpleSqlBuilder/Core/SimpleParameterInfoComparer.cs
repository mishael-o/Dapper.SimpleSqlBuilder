#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Dapper.SimpleSqlBuilder;

internal sealed class SimpleParameterInfoComparer : IEqualityComparer<SimpleParameterInfo>
{
    private static readonly Lazy<SimpleParameterInfoComparer> Lazy
        = new(() => new SimpleParameterInfoComparer());

    private SimpleParameterInfoComparer()
    {
    }

    public static SimpleParameterInfoComparer Instance
        => Lazy.Value;

    public bool Equals(SimpleParameterInfo? x, SimpleParameterInfo? y)
    {
        if (x is null || y is null || !x.HasValue || !y.HasValue)
        {
            return false;
        }

        return CheckEquality(x.Value, y.Value)
            && CheckEquality(x.Type, y.Type)
            && CheckEquality(x.DbType, y.DbType)
            && CheckEquality(x.Direction, y.Direction)
            && CheckEquality(x.Size, y.Size)
            && CheckEquality(x.Precision, y.Precision)
            && CheckEquality(x.Scale, y.Scale);

        static bool CheckEquality<T>(T first, T second)
            => EqualityComparer<T>.Default.Equals(first, second);
    }

#if NET6_0_OR_GREATER
    public int GetHashCode([DisallowNull] SimpleParameterInfo obj)
#else

    public int GetHashCode(SimpleParameterInfo obj)
#endif
        => HashCode.Combine(obj.Value, obj.Type, obj.DbType, obj.Direction, obj.Size, obj.Precision, obj.Scale);
}
