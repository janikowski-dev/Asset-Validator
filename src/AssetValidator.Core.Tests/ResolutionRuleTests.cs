using AssetValidator.Core.Domain;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class ResolutionRuleTests
{
    [Test]
    public void Invalid_Image_Resolution_Produces_Errors()
    {
        // Arrange
        ResolutionRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, object>
            {
                ["Image.Width"] = 4096,
                ["Image.Height"] = 4096
            }
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(2);
    }

    [Test]
    public void Valid_Image_Resolution_Produces_No_Results()
    {
        // Arrange
        ResolutionRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, object>
            {
                ["Image.Width"] = 1024,
                ["Image.Height"] = 1024
            }
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Image_With_Invalid_Height_Only_Produces_One_Error()
    {
        // Arrange
        ResolutionRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, object>
            {
                ["Image.Width"] = 512,
                ["Image.Height"] = 4096
            }
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Message.Should().Contain("Height");
        results[0].Severity.Should().Be(ValidationSeverity.Error);
    }

    [Test]
    public void Image_With_Invalid_Width_Only_Produces_One_Error()
    {
        // Arrange
        ResolutionRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, object>
            {
                ["Image.Width"] = 4096,
                ["Image.Height"] = 512
            }
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Message.Should().Contain("Width");
        results[0].Severity.Should().Be(ValidationSeverity.Error);
    }

    [Test]
    public void Non_Image_Asset_Produces_No_Results()
    {
        // Arrange
        ResolutionRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Mesh,
            Metadata = new Dictionary<string, object>
            {
                ["Image.Width"] = 4096,
                ["Image.Height"] = 4096
            }
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Missing_Metadata_Produces_No_Results()
    {
        // Arrange
        ResolutionRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, object>()
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Invalid_Metadata_Types_Produce_No_Results()
    {
        // Arrange
        ResolutionRule rule = new();

        Asset asset = new()
        {
            Type = AssetType.Image,
            Metadata = new Dictionary<string, object>
            {
                ["Image.Width"] = "1024",
                ["Image.Height"] = "1024"
            }
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }
}