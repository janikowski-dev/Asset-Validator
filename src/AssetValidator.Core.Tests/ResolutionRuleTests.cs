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
        ResolutionRule rule = new ResolutionRule();
        
        Asset asset = new Asset
        {
            Type = AssetType.Image,
            Name = "Bad Image",
            Path = "Bad Image.png",
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
        results.Should().OnlyContain(r => r.Severity == ValidationSeverity.Error);
    }
}