namespace AssetValidator.Core.Domain;

public sealed class Asset
{
    public IReadOnlyDictionary<string, object>  Metadata { get; init; }
    public AssetType Type { get; init; }
    public long SizeInBytes { get; init; }
    public string Name { get; init; }
    public string Path { get; init; }
}