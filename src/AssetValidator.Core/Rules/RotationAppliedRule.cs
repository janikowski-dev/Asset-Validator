using System.Numerics;
using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

internal sealed class RotationAppliedRule : IValidationRule
{
    public ValidationCategory Category => ValidationCategory.Quality;
    public ValidationSeverity Severity => ValidationSeverity.Error;
    public string Name => "Blender Rotation Applied";
    public string Id => "TRANSFORM_001";
    
    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (!MetadataKeys.TryReadValue(asset, MetadataKeys.Transform.RotationEuler, out Vector3 euler))
        {
            yield break;
        }

        if (!IsRotationApplied(euler))
        {
            yield return ValidationResult.FromRule(this, asset, $"Rotation is not applied {euler}");
        }
    }

    public bool AppliesTo(Asset asset)
    {
        if (asset.Source != SourceType.Blender)
        {
            return false;
        }
        
        return asset.Type == AssetType.Mesh;
    }
    
    private static bool IsRotationApplied(Vector3 euler) => euler == Vector3.Zero;
}