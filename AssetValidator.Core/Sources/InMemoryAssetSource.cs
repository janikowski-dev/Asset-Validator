using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Sources;

public sealed class InMemoryAssetSource(IEnumerable<Asset> assets) : IAssetSource
{
    private readonly IReadOnlyList<Asset> _assets = ValidateAndCacheAssets(assets);

    public IEnumerable<Asset> LoadAssets() => _assets;
    
    private static List<Asset> ValidateAndCacheAssets(IEnumerable<Asset> assets)
    {
        ArgumentNullException.ThrowIfNull(assets);
        return ToValidList(assets);
    }

    private static List<Asset> ToValidList(IEnumerable<Asset> assets)
    {
        List<Asset> list = assets.ToList();

        if (list.Count == 0)
        {
            throw new ArgumentException("At least one asset is required.");
        }

        return list;
    }
}