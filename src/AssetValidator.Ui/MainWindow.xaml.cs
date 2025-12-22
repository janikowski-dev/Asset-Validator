using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using AssetValidator.Core.Rules;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using AssetValidator.Core.Sources;
using Microsoft.Win32;

namespace AssetValidator.Ui;

public partial class MainWindow
{
    private const string InfoBoxCaption = "Asset Validator";
    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void ValidateJsonAssets(object _, RoutedEventArgs __)
    {
        try
        {
            if (!TryReadAssetsFromJson(out IEnumerable<Asset>? assets))
            {
                return;
            }
            
            RefreshUi(Validate(assets!));
            ShowSuccess("Validation finished.");
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
    
    private static void ShowError(params string[] messages)
    {
        MessageBox.Show(ToMessage(messages), InfoBoxCaption, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private static void ShowSuccess(params string[] messages)
    {
        MessageBox.Show(ToMessage(messages), InfoBoxCaption, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void RefreshUi(IReadOnlyList<ValidationResult> results)
    {
        ResultsGrid.ItemsSource = new ObservableCollection<ValidationResultViewModel>(
            results.Select(r => new ValidationResultViewModel(r))
        );
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

        IAssetSource source = new JsonFileAssetSource(dialog.FileName);
        assets = source.LoadAssets();
        return true;
    }

    private static IReadOnlyList<ValidationResult> Validate(IEnumerable<Asset> assets)
    {
        IValidationRule[] rules =
        [
            new ResolutionRule(),
            new NoSpacesInPathRule()
        ];

        return new ValidationEngine(rules).Validate(assets);
    }
}