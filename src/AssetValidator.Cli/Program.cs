using System.Text.Json;
using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using AssetValidator.Core.Rules;
using AssetValidator.Core.Sources;

const string outputAsJsonParameterName = "--json-results";
const string inputPathParameterName = "--input";

try
{
    if (!TryReadAssetsFromJson(args, out IEnumerable<Asset> assets))
    {
        Console.WriteLine($"Usage: AssetValidator.Cli {inputPathParameterName} <assets.json> [{outputAsJsonParameterName}]");
        Environment.Exit(1);
    }

    IReadOnlyList<ValidationResult> results = Validate(assets, out bool hasErrors);
    Console.WriteLine("Validation finished.");

    if (ShouldOutputAsJson(args))
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
}
catch (JsonException caughtException)
{
    Console.WriteLine(ToMessage("Failed to parse the file.", caughtException.Message));
    Environment.Exit(2);
}
catch (IOException caughtException)
{
    Console.WriteLine(ToMessage("Failed to read the file.", caughtException.Message));
    Environment.Exit(2);
}
catch (Exception caughtException)
{
    Console.WriteLine(ToMessage("Unexpected error occurred.", caughtException.Message));
    Environment.Exit(2);
}

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

static bool ShouldOutputAsJson(string[] args) => args.Contains(outputAsJsonParameterName);

static string ToJson(IReadOnlyList<ValidationResult> results)
{
    JsonSerializerOptions options = new()
    {
        WriteIndented = true
    };

    return JsonSerializer.Serialize(results, options);
}

static bool TryReadAssetsFromJson(string[] args, out IEnumerable<Asset> assets)
{
    int index = Array.IndexOf(args, inputPathParameterName);
    assets = [];
    
    if (index < 0 || index == args.Length - 1)
    {
        return false;
    }

    IAssetSource source = new JsonFileAssetSource(args[index + 1]);
    assets = source.LoadAssets();
    return true;
}

static IReadOnlyList<ValidationResult> Validate(IEnumerable<Asset> assets, out bool hasErrors)
{
    IValidationRule[] rules =
    [
        new ResolutionRule(),
        new NoSpacesInPathRule()
    ];

    IReadOnlyList<ValidationResult> results = new ValidationEngine(rules).Validate(assets);
    hasErrors = results.Any(r => r.Severity == ValidationSeverity.Error);
    return results;
}

static string ToMessage(params string[] messages) => string.Join("\n\n", messages);