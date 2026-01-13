using System.Text.Json;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class ResolutionWithinRangeRuleTests
{
    [Test]
    public void AppliesTo_Image_Returns_True()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image
        };

        // Act
        bool applies = rule.AppliesTo(asset);

        // Assert
        applies.Should().BeTrue();
    }

    [Test]
    public void AppliesTo_Non_Image_Returns_False()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh
        };

        // Act
        bool applies = rule.AppliesTo(asset);

        // Assert
        applies.Should().BeFalse();
    }

    [Test]
    public void Validate_Image_With_Valid_Resolution_Produces_No_Results()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, JsonElement>
            {
                { MetadataKeys.Image.Width, JsonSerializer.SerializeToElement(1024) },
                { MetadataKeys.Image.Height, JsonSerializer.SerializeToElement(1024) }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Image_With_Height_Too_Small_Produces_Error()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, JsonElement>
            {
                { MetadataKeys.Image.Width, JsonSerializer.SerializeToElement(512) },
                { MetadataKeys.Image.Height, JsonSerializer.SerializeToElement(64) }
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
    public void Validate_Image_With_Width_Too_Large_Produces_Error()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, JsonElement>
            {
                { MetadataKeys.Image.Width, JsonSerializer.SerializeToElement(4096) },
                { MetadataKeys.Image.Height, JsonSerializer.SerializeToElement(1024) }
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
    public void Validate_Image_With_Invalid_Width_And_Height_Produces_Two_Errors()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, JsonElement>
            {
                { MetadataKeys.Image.Width, JsonSerializer.SerializeToElement(64) },
                { MetadataKeys.Image.Height, JsonSerializer.SerializeToElement(4096) }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(2);
        results.Should().OnlyContain(r => r.Severity == ValidationSeverity.Error);
    }

    [Test]
    public void Validate_Image_Missing_Metadata_Produces_No_Results()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, JsonElement>()
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Validate_Image_With_Invalid_Metadata_Type_Produces_No_Results()
    {
        // Arrange
        ResolutionWithinRangeRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, JsonElement>
            {
                { MetadataKeys.Image.Width, JsonSerializer.SerializeToElement("1024") },
                { MetadataKeys.Image.Height, JsonSerializer.SerializeToElement(1024) }
            }
        };

        rule.AppliesTo(asset).Should().BeTrue();

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }
}
