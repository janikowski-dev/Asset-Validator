using AssetValidator.Core.Domain;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class TopologyDensityWithinRangeRuleTests
{
    [Test]
    public void AppliesTo_Mesh_Returns_True()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh
        };

        // Act
        bool applies = rule.AppliesTo(asset);

        // Assert
        applies.Should().BeTrue();
    }

    [Test]
    public void AppliesTo_Non_Mesh_Returns_False()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image
        };

        // Act
        bool applies = rule.AppliesTo(asset);

        // Assert
        applies.Should().BeFalse();
    }

    [Test]
    public void Validate_Density_Within_Range_Produces_No_Results()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        // ratio = 1000 / 800 = 1.25 (within 0.4 – 2.5)
        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.VertexCount, 1000 },
                { MetadataKeys.Mesh.TriangleCount, 800 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Density_Below_Minimum_Produces_Error()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        // ratio = 100 / 500 = 0.2 (< 0.4)
        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.VertexCount, 100 },
                { MetadataKeys.Mesh.TriangleCount, 500 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Severity.Should().Be(ValidationSeverity.Error);
    }

    [Test]
    public void Validate_Density_Above_Maximum_Produces_Error()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        // ratio = 3000 / 1000 = 3.0 (> 2.5)
        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.VertexCount, 3000 },
                { MetadataKeys.Mesh.TriangleCount, 1000 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Severity.Should().Be(ValidationSeverity.Error);
    }

    [Test]
    public void Validate_Zero_Triangle_Count_Produces_Error()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.VertexCount, 100 },
                { MetadataKeys.Mesh.TriangleCount, 0 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
    }

    [Test]
    public void Validate_Zero_Vertex_Count_Produces_Error()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.VertexCount, 0 },
                { MetadataKeys.Mesh.TriangleCount, 100 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
    }

    [Test]
    public void Validate_Missing_Metadata_Produces_No_Results()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>()
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Invalid_Metadata_Type_Produces_No_Results()
    {
        // Arrange
        TopologyDensityWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.VertexCount, "1000" },
                { MetadataKeys.Mesh.TriangleCount, 500 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }
}
