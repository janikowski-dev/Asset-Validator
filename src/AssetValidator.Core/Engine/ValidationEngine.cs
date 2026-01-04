using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Engine;

internal sealed class ValidationEngine(IEnumerable<IValidationRule> rules)
{
    internal IReadOnlyList<ValidationResult> Validate(IEnumerable<Asset> assets)
    {
        ArgumentNullException.ThrowIfNull(assets);
        return ValidateInternal(assets);
    }

    private IReadOnlyList<ValidationResult> ValidateInternal(IEnumerable<Asset> assets)
    {
        List<ValidationResult> results = [];

        foreach (Asset asset in assets)
        {
            foreach (IValidationRule rule in rules)
            {
                if (!rule.AppliesTo(asset))
                {
                    continue;
                }
                
                foreach (ValidationResult result in rule.Validate(asset))
                {
                    results.Add(result);
                }
            }
        }

        return results;
    }
}