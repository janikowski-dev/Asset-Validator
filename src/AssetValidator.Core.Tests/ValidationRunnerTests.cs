using System.Text.Json;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using FluentAssertions;

namespace AssetValidator.Core.Tests;

public class ValidationRunnerTests
{
    [Test]
    public void Validate_With_Assets_Runs_Validation()
    {
        // Arrange
        IReadOnlyList<Asset> assets = new List<Asset>
        {
            new()
            {
                Path = "Folder With Space/Asset.fbx"
            }
        };

        // Act
        IReadOnlyList<ValidationResult> results = ValidationRunner.Validate(assets);

        // Assert
        results.Should().ContainSingle(r =>
            r.RuleId == "PATH_STRUCT_001" &&
            r.Severity == ValidationSeverity.Warning
        );
    }

    [Test]
    public void Validate_With_Valid_Json_File_Runs_Validation()
    {
        // Arrange
        const string json = """
                            [
                                {
                                    "Path": "Folder With Space/Asset.fbx"
                                }
                            ]
                            """;

        string path = WriteTemporaryJson(json);

        // Act
        IReadOnlyList<ValidationResult> results = ValidationRunner.Validate(path);

        // Assert
        results.Should().ContainSingle(r =>
            r.RuleId == "PATH_STRUCT_001" &&
            r.Severity == ValidationSeverity.Warning
        );
    }

    [Test]
    public void Validate_With_Invalid_Json_Throws()
    {
        // Arrange
        string path = WriteTemporaryJson("not valid json");

        // Act
        Action act = () => ValidationRunner.Validate(path);

        // Assert
        act.Should().Throw<JsonException>();
    }

    private static string WriteTemporaryJson(string json)
    {
        string path = Path.GetTempFileName();
        File.WriteAllText(path, json);
        return path;
    }
}