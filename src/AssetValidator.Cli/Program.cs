using System.Text.Json;
using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using AssetValidator.Core.Rules;
using AssetValidator.Core.Sources;

IReadOnlyList<ValidationResult> results = ValidateDemoAssets(out bool hasErrors);

if (ShouldUseJson(args))
{
    Console.Write(ToJson(results));
}
else
{
    foreach (ValidationResult result in results)
    {
        Print(result);
    }
}

Environment.Exit(hasErrors ? 1 : 0);
return;

static void Print(ValidationResult result)
{
    Console.ForegroundColor = GetColor(result);
    Console.WriteLine(Format(result));
    Console.ResetColor();
}

static ConsoleColor GetColor(ValidationResult result) => result.Severity switch
{
    ValidationSeverity.Error => ConsoleColor.Red,
    ValidationSeverity.Warning => ConsoleColor.Yellow,
    ValidationSeverity.Log => ConsoleColor.Gray,
    _ => throw new ArgumentOutOfRangeException()
};

static string Format(ValidationResult result) => $"[{result.Severity}] {result.RuleId} {result.Asset.Path} - {result.Message}";

static bool ShouldUseJson(string[] args) => args.Contains("--json");

static string ToJson(IReadOnlyList<ValidationResult> results)
{
    JsonSerializerOptions options = new()
    {
        WriteIndented = true
    };

    return JsonSerializer.Serialize(results, options);
}

static IReadOnlyList<ValidationResult> ValidateDemoAssets(out bool hasErrors)
{
    IAssetSource source = new InMemoryAssetSource([
        new Asset
        {
            Type = AssetType.Image,
            Name = "Bad Image",
            Path = "Bad Image.png",
            SizeInBytes = 2 * 1024 * 1024,
            Metadata = new Dictionary<string, object>
            {
                ["Image.Width"] = 4096,
                ["Image.Height"] = 4096
            }
        }
    ]);

    IValidationRule[] rules =
    [
        new ResolutionRule(),
        new NoSpacesInPathRule()
    ];

    IReadOnlyList<ValidationResult> results = new ValidationEngine(rules).Validate(source.LoadAssets());
    hasErrors = results.Any(r => r.Severity == ValidationSeverity.Error);
    return results;
}