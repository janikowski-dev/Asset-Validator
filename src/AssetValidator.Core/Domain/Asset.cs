using System.Text.Json.Serialization;

namespace AssetValidator.Core.Domain;

public sealed class Asset
{
    public IReadOnlyDictionary<string, object> Metadata { get; internal init; } = new Dictionary<string, object>();
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetType Type { get; internal init; }
    
    public long SizeInBytes { get; internal init; }
    public string Name { get; internal init; } = string.Empty;
    public string Path { get; internal init; } = string.Empty;

    internal Asset()
    {
    }
    
    [JsonConstructor]
    internal Asset(
        string name,
        string path,
        AssetType type,
        long sizeInBytes,
        IReadOnlyDictionary<string, object> metadata)
    {
        Name = name;
        Path = path;
        Type = type;
        SizeInBytes = sizeInBytes;
        Metadata = metadata;
    }
}