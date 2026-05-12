using System.Text;

namespace AtelierRex.Core;

public readonly struct FourCC : IEquatable<FourCC>
{
    public string Value { get; }

    private FourCC(string value) => Value = value;

    public static FourCC Parse(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length < 4)
            throw new ArgumentException("FourCC requires at least 4 bytes.", nameof(bytes));
        return new FourCC(Encoding.Latin1.GetString(bytes[..4]));
    }

    public static FourCC Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length != 4)
            throw new ArgumentException("FourCC must be exactly 4 characters.", nameof(value));
        return new FourCC(value);
    }

    public bool Equals(FourCC other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is FourCC other && Equals(other);
    public override int GetHashCode() => Value?.GetHashCode(StringComparison.Ordinal) ?? 0;
    public override string ToString() => Value ?? string.Empty;

    public static bool operator ==(FourCC left, FourCC right) => left.Equals(right);
    public static bool operator !=(FourCC left, FourCC right) => !left.Equals(right);
}
