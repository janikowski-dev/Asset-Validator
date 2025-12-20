using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;

namespace AssetValidator.Core.Rules;

public sealed class ResolutionRule : IValidationRule
{
    public ValidationSeverity Severity => ValidationSeverity.Error;
    public ValidationCategory Category => ValidationCategory.Size;
    public string Name => "Image Resolution";
    public string Id => "IMG_RES_001";
    
    private const string HeightMetadataName = "Image.Height";
    private const string WidthMetadataName = "Image.Width";

    private const int MaxHeight = 2048;
    private const int MinHeight = 128;
    private const int MaxWidth = 2048;
    private const int MinWidth = 128;

    public IEnumerable<ValidationResult> Validate(Asset asset)
    {
        if (!IsApplicable(asset))
        {
            yield break;
        }

        if (!TryGetSize(asset, out var width, out var height))
        {
            yield break;
        }

        if (!IsHeightValid(height))
        {
            yield return ValidationResult.FromRule(this, asset, $"Height is invalid ({height}).");
        }

        if (!IsWidthValid(width))
        {
            yield return ValidationResult.FromRule(this, asset, $"Width is invalid ({width}).");
        }
    }

    private static bool IsApplicable(Asset asset) => asset.Type == AssetType.Image;

    private static bool TryGetSize(Asset asset, out int width, out int height)
    {
        height = -1;
        width = -1;

        if (!asset.Metadata.TryGetValue(HeightMetadataName, out object? heightObject))
        {
            return false;
        }

        if (heightObject is not int heightInt)
        {
            return false;
        }

        height = heightInt;

        if (!asset.Metadata.TryGetValue(WidthMetadataName, out object? widthObject))
        {
            return false;
        }

        if (widthObject is not int widthInt)
        {
            return false;
        }

        width = widthInt;
        return true;
    }

    private bool IsHeightValid(int height)
    {
        if (height > MaxHeight)
        {
            return false;
        }
        
        return height >= MinHeight;
    }

    private bool IsWidthValid(int width)
    {
        if (width > MaxWidth)
        {
            return false;
        }
        
        return width >= MinWidth;
    }
}
