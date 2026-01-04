using System.Text.Json;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class ScaleAppliedRuleTests
{
    [Test]
    public void AppliesTo_Blender_Mesh_Returns_True()
    {
        // Arrange
        ScaleAppliedRule rule = new();

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
    public void AppliesTo_Non_Blender_Source_Returns_False()
    {
        // Arrange
        ScaleAppliedRule rule = new();

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
        ScaleAppliedRule rule = new();

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
    public void Validate_Mesh_With_Non_Unit_Scale_Produces_Error()
    {
        // Arrange
        ScaleAppliedRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>
            {
                {
                    MetadataKeys.Transform.Scale,
                    JsonSerializer.SerializeToElement(new[] { 1f, 2f, 1f })
                }
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
    public void Validate_Mesh_With_Unit_Scale_Produces_No_Results()
    {
        // Arrange
        ScaleAppliedRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>
            {
                {
                    MetadataKeys.Transform.Scale,
                    JsonSerializer.SerializeToElement(new[] { 1f, 1f, 1f })
                }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Mesh_Without_Scale_Metadata_Produces_No_Results()
    {
        // Arrange
        ScaleAppliedRule rule = new();

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
    public void Validate_Mesh_With_Invalid_Scale_Metadata_Type_Produces_No_Results()
    {
        // Arrange
        ScaleAppliedRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>
            {
                { MetadataKeys.Transform.Scale, "1,1,1" }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Mesh_With_Invalid_Scale_Array_Length_Produces_No_Results()
    {
        // Arrange
        ScaleAppliedRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Source = SourceType.Blender,
            Metadata = new Dictionary<string, object>
            {
                {
                    MetadataKeys.Transform.Scale,
                    JsonSerializer.SerializeToElement(new[] { 1f, 1f })
                }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }
}
