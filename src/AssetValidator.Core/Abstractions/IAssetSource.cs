using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Abstractions;

public interface IAssetSource
{
    IEnumerable<Asset> LoadAssets();
}