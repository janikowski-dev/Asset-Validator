using System.Text.Json;
using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Sources;

public sealed class JsonFileAssetSource(string filePath) : IAssetSource
{
    public IEnumerable<Asset> LoadAssets()
    {
        ArgumentNullException.ThrowIfNull(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Asset file not found.", filePath);
        }
        
        return JsonSerializer.Deserialize<List<Asset>>(File.ReadAllText(filePath)) ?? throw new InvalidOperationException("Failed to deserialize assets.");
    }
}