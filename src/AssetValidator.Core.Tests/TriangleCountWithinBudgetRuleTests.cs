using AssetValidator.Core.Domain;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class TriangleCountWithinBudgetRuleTests
{
    [Test]
    public void AppliesTo_Mesh_Returns_True()
    {
        // Arrange
        TriangleCountWithinBudgetRule rule = new();

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
        TriangleCountWithinBudgetRule rule = new();

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
    public void Validate_Triangle_Count_Within_Budget_Produces_No_Results()
    {
        // Arrange
        TriangleCountWithinBudgetRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.TriangleCount, 25_000 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Triangle_Count_At_Budget_Limit_Produces_No_Results()
    {
        // Arrange
        TriangleCountWithinBudgetRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.TriangleCount, 40_000 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Triangle_Count_Above_Budget_Produces_Error()
    {
        // Arrange
        TriangleCountWithinBudgetRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.TriangleCount, 50_000 }
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
    public void Validate_Mesh_Without_Triangle_Count_Metadata_Produces_No_Results()
    {
        // Arrange
        TriangleCountWithinBudgetRule rule = new();

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
    public void Validate_Mesh_With_Invalid_Triangle_Count_Metadata_Type_Produces_No_Results()
    {
        // Arrange
        TriangleCountWithinBudgetRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.TriangleCount, "40000" }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }
}
