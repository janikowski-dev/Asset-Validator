using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

internal sealed class TriangleCountWithinBudgetRule : IValidationRule
{
    public ValidationCategory Category => ValidationCategory.Performance;
    public ValidationSeverity Severity => ValidationSeverity.Error;
    public string Name => "Triangle Count Within Budget";
    public string Id => "MESH_002";

    private const int TriangleBudget = 40_000;
    
    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (!TryGetTriangleCount(asset, out int count))
        {
            yield break;
        }

        if (!IsWithinBudget(count))
        {
            yield return ValidationResult.FromRule(this, asset, $"Mesh has {count} triangles (budget: {TriangleBudget})");
        }
    }

    public bool AppliesTo(Asset asset) => asset.Type == AssetType.Mesh;
    
    private static bool TryGetTriangleCount(Asset asset, out int count)
    {
        count = -1;

        if (!asset.Metadata.TryGetValue(MetadataKeys.Mesh.TriangleCount, out object? countObject))
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

    private bool IsWithinBudget(int count) => count <= TriangleBudget;
}