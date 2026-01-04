using System.Numerics;
using System.Text.Json;
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
        if (!TryGetRotationEuler(asset, out Vector3 scale))
        {
            yield break;
        }

        if (!IsRotationApplied(scale))
        {
            yield return ValidationResult.FromRule(this, asset, $"Rotation is not applied {scale}");
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
    
    private static bool TryGetRotationEuler(Asset asset, out Vector3 euler)
    {
        euler = Vector3.NaN;

        if (!asset.Metadata.TryGetValue(MetadataKeys.Transform.RotationEuler, out object? scaleObject))
        {
            return false;
        }

        if (scaleObject is not JsonElement eulerElement)
        {
            return false;
        }

        if (eulerElement.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        if (eulerElement.GetArrayLength() != 3)
        {
            return false;
        }

        euler = new Vector3(
            eulerElement[0].GetSingle(),
            eulerElement[1].GetSingle(),
            eulerElement[2].GetSingle()
        );
        return true;
    }
    
    private static bool IsRotationApplied(Vector3 scale) => scale == Vector3.Zero;
}