using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Sources;

namespace AssetValidator.Core.Engine;

public static class ValidationRunner
{
    public static IReadOnlyList<ValidationResult> Validate(IEnumerable<Asset> assets) => Validate(new InMemoryAssetSource(assets));
    
    public static IReadOnlyList<ValidationResult> Validate(string filePath) => Validate(new JsonFileAssetSource(filePath));

    private static IReadOnlyList<ValidationResult> Validate(IAssetSource source)
    {
        ValidationEngine engine = ValidationEngineFactory.Create();
        return engine.Validate(source.LoadAssets());
    }
}