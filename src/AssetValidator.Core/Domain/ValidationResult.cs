using AssetValidator.Core.Abstractions;

namespace AssetValidator.Core.Domain;

public sealed class ValidationResult
{
    public ValidationSeverity Severity { get; }
    public ValidationCategory Category { get; }
    public DateTimeOffset Timestamp { get; }
    public string RuleName { get; }
    public string Message { get; }
    public string RuleId { get; }
    public Asset Asset { get; }

    private ValidationResult(
        Asset asset,
        string ruleId,
        string ruleName,
        ValidationSeverity severity,
        ValidationCategory category,
        string message,
        DateTimeOffset timestamp)
    {
        Timestamp = timestamp;
        RuleName = ruleName;
        Severity = severity;
        Category = category;
        Message = message;
        RuleId = ruleId;
        Asset = asset;
    }

    public static ValidationResult FromRule(IValidationRule rule, Asset asset, string message)
    {
        Validate(rule, asset, message);
        return Create(rule, asset, message);
    }

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    private static void Validate(IValidationRule rule, Asset asset, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be empty.", nameof(message));
        }
        
        ArgumentNullException.ThrowIfNull(asset);
        ArgumentNullException.ThrowIfNull(rule);
    }
    
    private static ValidationResult Create(IValidationRule rule, Asset asset, string message) => new(
        asset,
        rule.Id,
        rule.Name,
        rule.Severity,
        rule.Category,
        message,
        DateTimeOffset.UtcNow
    );
}