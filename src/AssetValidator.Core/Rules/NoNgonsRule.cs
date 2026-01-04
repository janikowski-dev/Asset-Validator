using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

internal sealed class NoNgonsRule : IValidationRule
{
    public ValidationCategory Category => ValidationCategory.Quality;
    public ValidationSeverity Severity => ValidationSeverity.Warning;
    public string Name => "No Ngons";
    public string Id => "MESH_001";
    
    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (!TryGetNgonCount(asset, out int count))
        {
            yield break;
        }

        if (HasNgons(count))
        {
            yield return ValidationResult.FromRule(this, asset, $"Mesh contains {count} ngons");
        }
    }

    public bool AppliesTo(Asset asset)
    {
        if (asset.Source == SourceType.Unreal)
        {
            return false;
        }

        return asset.Type == AssetType.Mesh;
    }
    
    private static bool TryGetNgonCount(Asset asset, out int count)
    {
        count = -1;

        if (!asset.Metadata.TryGetValue(MetadataKeys.Mesh.NgonCount, out object? countObject))
        {
            return false;
        }

        if (countObject is not int countInt)
        {
            return false;
        }

        count = countInt;
        return true;
    }

    private bool HasNgons(int count) => count > 0;
}