using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Engine;

public sealed class ValidationEngine(IEnumerable<IValidationRule> rules)
{
    private readonly IReadOnlyList<IValidationRule> _rules = ValidateAndCacheRules(rules);

    public IReadOnlyList<ValidationResult> Validate(IEnumerable<Asset> assets)
    {
        ArgumentNullException.ThrowIfNull(assets);
        return ValidateInternal(assets);
    }
    
    private static List<IValidationRule> ValidateAndCacheRules(IEnumerable<IValidationRule> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);
        return ToValidList(rules);
    }

    private static List<IValidationRule> ToValidList(IEnumerable<IValidationRule> rules)
    {
        List<IValidationRule> list = rules.ToList();

        if (list.Count == 0)
        {
            throw new ArgumentException("At least one validation rule is required.");
        }

        return list;
    }

    private IReadOnlyList<ValidationResult> ValidateInternal(IEnumerable<Asset> assets)
    {
        List<ValidationResult> results = new List<ValidationResult>();

        foreach (Asset asset in assets)
        {
            foreach (IValidationRule rule in _rules)
            {
                foreach (ValidationResult result in rule.Validate(asset))
                {
                    results.Add(result);
                }
            }
        }

        return results;
    }
}