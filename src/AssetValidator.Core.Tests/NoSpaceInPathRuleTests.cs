using AssetValidator.Core.Domain;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class NoSpaceInPathRuleTests
{
    [Test]
    public void Path_With_Spaces_Produces_Warning()
    {
        // Arrange
        NoSpacesInPathRule rule = new();

        Asset asset = new()
        {
            Path = "Folder With Spaces/Asset.fbx"
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().HaveCount(1);
        results[0].Severity.Should().Be(ValidationSeverity.Warning);
    }

    [Test]
    public void Path_Without_Spaces_Produces_No_Results()
    {
        // Arrange
        NoSpacesInPathRule rule = new();

        Asset asset = new()
        {
            Path = "Folder/Asset.fbx"
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Empty_Path_Produces_No_Results()
    {
        // Arrange
        NoSpacesInPathRule rule = new();

        Asset asset = new()
        {
            Path = string.Empty
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }

    [Test]
    public void Whitespace_Path_Produces_No_Results()
    {
        // Arrange
        NoSpacesInPathRule rule = new();

        Asset asset = new()
        {
            Path = "   "
        };

        // Act
        List<ValidationResult> results = rule.Validate(asset).ToList();

        // Assert
        results.Should().BeEmpty();
    }
}