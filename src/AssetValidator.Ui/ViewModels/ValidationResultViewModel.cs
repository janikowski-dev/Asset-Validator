using AssetValidator.Core.Domain;

namespace AssetValidator.Ui;

public sealed class ValidationResultViewModel(ValidationResult result)
{
    public ValidationSeverity Severity { get; } = result.Severity;
    public string RuleId { get; } = result.RuleId;
    public string AssetPath { get; } = result.Asset.Path;
    public string Message { get; } = result.Message;
}