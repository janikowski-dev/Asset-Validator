using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Engine;

public static class ValidationRunner
{
    public static IReadOnlyList<ValidationResult> Validate(IEnumerable<Asset> assets) => ValidationEngineFactory.Create().Validate(assets);
}