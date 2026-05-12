namespace AtelierRex.Core;

public enum Endianness { LittleEndian, BigEndian }

public readonly struct FileFormat : IEquatable<FileFormat>
{
    public string Name { get; }
    public string Family { get; }
    public Version Version { get; }
    public Endianness Endianness { get; }

    public FileFormat(string name, string family, Version version, Endianness endianness)
    {
        Name = name;
        Family = family;
        Version = version;
        Endianness = endianness;
    }

    public bool Equals(FileFormat other) =>
        Name == other.Name &&
        Family == other.Family &&
        Version == other.Version &&
        Endianness == other.Endianness;

    public override bool Equals(object? obj) => obj is FileFormat other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Name, Family, Version, Endianness);
    public override string ToString() => $"{Name} ({Family} v{Version}, {Endianness})";

    public static bool operator ==(FileFormat left, FileFormat right) => left.Equals(right);
    public static bool operator !=(FileFormat left, FileFormat right) => !left.Equals(right);
}
