using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

internal sealed class NoSpacesInPathRule : IValidationRule
{
    public ValidationCategory Category => ValidationCategory.Structure;
    public ValidationSeverity Severity => ValidationSeverity.Warning;
    public string Name => "No Spaces In Path";
    public string Id => "GENERAL_001";

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

    public bool AppliesTo(Asset asset) => asset.Source != SourceType.Blender;

    private static bool IsPathEmpty(Asset asset) => string.IsNullOrWhiteSpace(asset.Path);

    private static bool PathContainsSpace(Asset asset) => asset.Path.Contains(' ');
}