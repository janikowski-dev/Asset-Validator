using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

internal sealed class TopologyDensityWithinRangeRule : IValidationRule
{
    public ValidationCategory Category => ValidationCategory.Quality;
    public ValidationSeverity Severity => ValidationSeverity.Error;
    public string Name =>  "Topology Density Within Range";
    public string Id => "MESH_003";

    private const float MinVertexToTriangleRatio = 0.4f;
    private const float MaxVertexToTriangleRatio = 2.5f;
    
    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (!TryGetInfo(asset, out int vertexCount, out int triangleCount))
        {
            yield break;
        }

        if (!IsDensityWithinRange(vertexCount, triangleCount))
        {
            yield return ValidationResult.FromRule(this, asset, $"Density outside expected range (vertices: {vertexCount}, triangles: {triangleCount})");
        }
    }

    public bool AppliesTo(Asset asset) => asset.Type == AssetType.Mesh;
    
    private static bool TryGetInfo(Asset asset, out int vertexCount, out int triangleCount)
    {
        triangleCount = -1;
        vertexCount = -1;

        if (!asset.Metadata.TryGetValue(MetadataKeys.Mesh.TriangleCount, out object? triangleCountObject))
        {
            return false;
        }

        if (triangleCountObject is not int triangleCountInt)
        {
            return false;
        }

        triangleCount = triangleCountInt;

        if (!asset.Metadata.TryGetValue(MetadataKeys.Mesh.VertexCount, out object? vertexCountObject))
        {
            return false;
        }

        if (vertexCountObject is not int vertexCountInt)
        {
            return false;
        }

        vertexCount = vertexCountInt;
        return true;
    }
    
    private static bool IsDensityWithinRange(int vertexCount, int triangleCount)
    {
        if (vertexCount <= 0 || triangleCount <= 0)
        {
            return false;
        }

        float ratio = (float)vertexCount / triangleCount;

        if (ratio < MinVertexToTriangleRatio)
        {
            return false;
        }
        
        return ratio <= MaxVertexToTriangleRatio;
    }
}