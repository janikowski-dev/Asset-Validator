using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Abstractions;

public interface IValidationRule
{
    ValidationCategory Category { get; }
    ValidationSeverity Severity { get; }
    string Name { get; }
    string Id { get; }
    
    IEnumerable<ValidationResult> Validate(Asset asset);
}