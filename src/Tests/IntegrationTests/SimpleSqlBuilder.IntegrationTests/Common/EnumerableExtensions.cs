namespace Dapper.SimpleSqlBuilder.IntegrationTests.Common;

internal static class EnumerableExtensions
{
    public static T[] AsArray<T>(this IEnumerable<T> enumerable)
    {
        return enumerable is null
            ? throw new ArgumentNullException(nameof(enumerable))
            : enumerable is T[] array ? array : enumerable.ToArray();
    }
}
