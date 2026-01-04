using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class ValidationEngineTests
{
    [Test]
    public void Validator_Runs_All_Rules_On_All_Assets()
    {
        // Arrange
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
        IReadOnlyList<ValidationResult> results = ValidationRunner.Validate(assets);
        
        // Assert
        results.Should().HaveCount(3);
    }
}