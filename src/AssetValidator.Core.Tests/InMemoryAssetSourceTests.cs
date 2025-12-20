using AssetValidator.Core.Domain;
using AssetValidator.Core.Sources;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class InMemoryAssetSourceTests
{
    [Test]
    public void Source_Returns_All_Assets()
    {
        // Arrange
        Asset[] assets =
        [
            new() { Name = "A" },
            new() { Name = "B" }
        ];
        
        InMemoryAssetSource source = new InMemoryAssetSource(assets);

        // Act
        List<Asset> result = source.LoadAssets().ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(assets);
    }
}