using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

internal sealed class ResolutionWithinRangeRule : IValidationRule
{
    public ValidationSeverity Severity => ValidationSeverity.Error;
    public ValidationCategory Category => ValidationCategory.Size;
    public string Name => "Image Resolution Within Range";
    public string Id => "IMAGE_001";

    private const int MaxHeight = 2048;
    private const int MinHeight = 128;
    private const int MaxWidth = 2048;
    private const int MinWidth = 128;

    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (!MetadataKeys.TryReadValue(asset, MetadataKeys.Image.Height, out int height))
        {
            yield break;
        }
        
        if (!MetadataKeys.TryReadValue(asset, MetadataKeys.Image.Width, out int width))
        {
            yield break;
        }

        if (!IsHeightValid(height))
        {
            yield return ValidationResult.FromRule(this, asset, $"Height is invalid ({height})");
        }

        if (!IsWidthValid(width))
        {
            yield return ValidationResult.FromRule(this, asset, $"Width is invalid ({width})");
        }
    }

    public bool AppliesTo(Asset asset) => asset.Type == AssetType.Image;

    private static bool IsHeightValid(int height)
    {
        if (height > MaxHeight)
        {
            return false;
        }
        
        return height >= MinHeight;
    }

    private static bool IsWidthValid(int width)
    {
        if (width > MaxWidth)
        {
            return false;
        }
        
        return width >= MinWidth;
    }
}
