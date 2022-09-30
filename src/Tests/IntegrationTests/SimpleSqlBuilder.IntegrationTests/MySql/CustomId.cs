namespace Dapper.SimpleSqlBuilder.IntegrationTests.MySql;

public struct CustomId : IEquatable<CustomId>
{
    public CustomId(Guid id)
    {
        Id = id;
    }

    public CustomId(byte[] bytes)
    {
        Id = new Guid(bytes);
    }

    public Guid Id { get; }

    public static implicit operator CustomId(Guid id)
        => new(id);

    public static implicit operator Guid(CustomId id)
        => id.Id;

    public static bool operator ==(CustomId left, CustomId right)
    {
        return left.Id.Equals(right.Id);
    }

    public static bool operator !=(CustomId left, CustomId right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
        => Id.GetHashCode();

    public override bool Equals(object? obj)
        => obj is CustomId mySqlGuidId && Equals(mySqlGuidId);

    public bool Equals(CustomId other)
        => Id.Equals(other.Id);

    public byte[] ToByteArray()
        => Id.ToByteArray();
}
