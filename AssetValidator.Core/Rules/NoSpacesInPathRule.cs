using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

public sealed class NoSpacesInPathRule : IValidationRule
{
    public ValidationCategory Category => ValidationCategory.Structure;
    public ValidationSeverity Severity => ValidationSeverity.Warning;
    public string Name => "No spaces in asset path";
    public string Id => "PATH_STRUCT_001";

    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (IsPathEmpty(asset))
        {
            yield break;
        }

        if (PathContainsSpace(asset))
        {
            yield return ValidationResult.FromRule(this, asset, $"Asset path contains spaces: \"{asset.Path}\"");
        }
    }

    private static bool IsPathEmpty(Asset asset) => string.IsNullOrWhiteSpace(asset.Path);

    private static bool PathContainsSpace(Asset asset) => asset.Path.Contains(' ');
}