using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using AssetValidator.Core.Sources;
using AssetValidator.Ui.Controls;
using Microsoft.Win32;

namespace AssetValidator.Ui;

public partial class MainWindow
{
    private IReadOnlyList<ValidationResult> _validationResults = [];
    private string _filter = string.Empty;
    private bool _showWarnings = true;
    private bool _showErrors = true;
    private bool _showInfo = true;
    
    public ObservableCollection<ValidationResultViewModel> ValidationResults { get; } = [];
    
    private const string InfoBoxCaption = "Asset Validator";

    public MainWindow()
    {
        InitializeComponent();
        InitContext();
    }

    private void ValidateJsonAssets(object? _, EventArgs __)
    {
        try
        {
            if (!TryReadAssetsFromJson(out IEnumerable<Asset>? assets))
            {
                return;
            }

            Cache(ValidationRunner.Validate(assets!));
            RefreshUi();
            ShowInfo("Validation finished.");
        }
        catch (JsonException caughtException)
        {
            ShowError("Failed to parse the file.", caughtException.Message);
        }
        catch (IOException caughtException)
        {
            ShowError("Failed to read the file.", caughtException.Message);
        }
        catch (Exception caughtException)
        {
            ShowError("Unexpected error occurred.", caughtException.Message);
        }
    }

    private void RefreshUi(object? sender, EventArgs e)
    {
        if (TryRefreshFilters(sender))
        {
            RefreshUi();
        }
    }

    private static void ShowError(params string[] messages)
    {
        MessageBox.Show(ToMessage(messages), InfoBoxCaption, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private static void ShowInfo(params string[] messages)
    {
        MessageBox.Show(ToMessage(messages), InfoBoxCaption, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private bool TryRefreshFilters(object? sender)
    {
        if (sender is not FiltersView filters)
        {
            return false;
        }

        _showWarnings = filters.ShowWarnings;
        _showErrors = filters.ShowErrors;
        _showInfo = filters.ShowInfo;
        _filter = filters.SearchText;
        return true;
    }

    private void RefreshUi()
    {
        ValidationResults.Clear();

        foreach (ValidationResult result in _validationResults)
        {
            bool passesSeverityFilter = result.Severity switch
            {
                ValidationSeverity.Log => _showInfo,
                ValidationSeverity.Warning => _showWarnings,
                ValidationSeverity.Error => _showErrors,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (!passesSeverityFilter)
            {
                continue;
            }

            bool passesSearchFilter = result.RuleName.Contains(_filter, StringComparison.OrdinalIgnoreCase) ||
                                      result.RuleId.Contains(_filter, StringComparison.OrdinalIgnoreCase) ||
                                      result.Message.Contains(_filter, StringComparison.OrdinalIgnoreCase);

            if (!passesSearchFilter)
            {
                continue;
            }

            ValidationResults.Add(new ValidationResultViewModel(result));
        }
    }
    
    private static string ToMessage(string[] messages) => string.Join("\n\n", messages);

    private static bool TryReadAssetsFromJson(out IEnumerable<Asset>? assets)
    {
        OpenFileDialog dialog = new()
        {
            Filter = "JSON files (*.json)|*.json",
            Title = "Validate assets",
            Multiselect = false
        };
        assets = null;

        if (dialog.ShowDialog() != true)
        {
            return false;
        }

        JsonFileAssetSource source = new(dialog.FileName);
        assets = source.LoadAssets();
        return true;
    }

    private void Cache(IReadOnlyList<ValidationResult> results)
    {
        _validationResults = results;
    }

    private void InitContext()
    {
        DataContext = this;
    }
}