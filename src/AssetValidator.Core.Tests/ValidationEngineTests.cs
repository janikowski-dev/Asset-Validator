using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using AssetValidator.Core.Rules;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class ValidationEngineTests
{
    [Test]
    public void Validator_Runs_All_Rules_On_All_Assets()
    {
        // Arrange
        IValidationRule[] rules =
        [
            new ResolutionRule(),
            new NoSpacesInPathRule()
        ];

        ValidationEngine engine = new(rules);

        Asset[] assets =
        [
            new()
            {
                Type = AssetType.Image,
                Name = "Bad Image",
                Path = "Bad Image.png",
                Metadata = new Dictionary<string, object>
                {
                    ["Image.Width"] = 4096,
                    ["Image.Height"] = 4096
                }
            }
        ];

        // Act
        IReadOnlyList<ValidationResult> results = engine.Validate(assets);
        
        // Assert
        results.Should().HaveCount(3);
    }
}