using System.Numerics;
using System.Text.Json;
using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

internal sealed class ScaleAppliedRule : IValidationRule
{
    public ValidationCategory Category => ValidationCategory.Quality;
    public ValidationSeverity Severity => ValidationSeverity.Error;
    public string Name => "Blender Scale Applied";
    public string Id => "TRANSFORM_002";
    
    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (!MetadataKeys.TryReadValue(asset, MetadataKeys.Transform.Scale, out Vector3 scale))
        {
            yield break;
        }

        if (!IsScaleApplied(scale))
        {
            yield return ValidationResult.FromRule(this, asset, $"Scale is not applied {scale}");
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
    
    private static bool IsScaleApplied(Vector3 scale) => scale == Vector3.One;
}