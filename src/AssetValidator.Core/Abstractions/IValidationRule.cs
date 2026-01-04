using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Abstractions;

internal interface IValidationRule
{
    ValidationCategory Category { get; }
    ValidationSeverity Severity { get; }
    string Name { get; }
    string Id { get; }
    
    IEnumerable<ValidationResult> Validate(Asset asset);
    bool AppliesTo(Asset asset);
}