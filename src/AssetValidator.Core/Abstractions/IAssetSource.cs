using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Abstractions;

internal interface IAssetSource
{
    IEnumerable<Asset> LoadAssets();
}