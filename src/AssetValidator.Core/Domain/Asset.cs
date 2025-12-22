using System.Text.Json.Serialization;

namespace AssetValidator.Core.Domain;

public sealed class Asset
{
    public IReadOnlyDictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>();
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AssetType Type { get; init; }
    
    public long SizeInBytes { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Path { get; init; } = string.Empty;
}