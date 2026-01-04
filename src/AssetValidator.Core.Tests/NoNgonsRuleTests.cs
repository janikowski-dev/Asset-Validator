using AssetValidator.Core.Domain;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class NoNgonsRuleTests
{
    [Test]
    public void AppliesTo_Mesh_From_Blender_Returns_True()
    {
        // Arrange
        NoNgonsRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender
        };

        // Act
        bool applies = rule.AppliesTo(asset);

        // Assert
        applies.Should().BeTrue();
    }

    [Test]
    public void AppliesTo_Unreal_Mesh_Returns_False()
    {
        // Arrange
        NoNgonsRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Unreal
        };

        // Act
        bool applies = rule.AppliesTo(asset);

        // Assert
        applies.Should().BeFalse();
    }

    [Test]
    public void AppliesTo_Non_Mesh_Returns_False()
    {
        // Arrange
        NoNgonsRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Source = SourceType.Blender
        };

        // Act
        bool applies = rule.AppliesTo(asset);

        // Assert
        applies.Should().BeFalse();
    }

    [Test]
    public void Validate_Mesh_With_Ngons_Produces_Warning()
    {
        // Arrange
        NoNgonsRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.NgonCount, 3 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Severity.Should().Be(ValidationSeverity.Warning);
    }

    [Test]
    public void Validate_Mesh_Without_Ngons_Produces_No_Results()
    {
        // Arrange
        NoNgonsRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.NgonCount, 0 }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Mesh_Without_Ngon_Metadata_Produces_No_Results()
    {
        // Arrange
        NoNgonsRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>()
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Mesh_With_Invalid_Ngon_Metadata_Type_Produces_No_Results()
    {
        // Arrange
        NoNgonsRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Mesh.NgonCount, "five" }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }
}
