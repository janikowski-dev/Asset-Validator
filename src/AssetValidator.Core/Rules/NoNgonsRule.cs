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
        if (!MetadataKeys.TryReadValue(asset, MetadataKeys.Mesh.NgonCount, out int count))
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

    private bool HasNgons(int count) => count > 0;
}