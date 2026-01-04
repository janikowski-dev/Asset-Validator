using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Sources;

internal sealed class InMemoryAssetSource(IEnumerable<Asset> assets) : IAssetSource
{
    private readonly IReadOnlyList<Asset> _assets = assets.ToList();

    public IEnumerable<Asset> LoadAssets()
    {
        if (_assets.Count == 0)
        {
            throw new InvalidOperationException("Asset list is empty.");
        }

        return _assets;
    }
}