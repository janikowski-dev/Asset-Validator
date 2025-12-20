using AssetValidator.Core.Abstractions;
using AssetValidator.Core.Domain;
using AssetValidator.Core.Engine;
using AssetValidator.Core.Rules;
using AssetValidator.Core.Sources;
using System.Collections.ObjectModel;
using System.Windows;

namespace AssetValidator.Ui;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void RefreshResults(object _, RoutedEventArgs __)
    {
        RefreshUi(ValidateDemoAssets());
    }

    private static IReadOnlyList<ValidationResult> ValidateDemoAssets()
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

        ValidationEngine engine = new ValidationEngine(rules);
        return engine.Validate(source.LoadAssets());
    }

    private void RefreshUi(IReadOnlyList<ValidationResult> results)
    {
        ResultsGrid.ItemsSource = new ObservableCollection<ValidationResultViewModel>(
            results.Select(r => new ValidationResultViewModel(r))
        );
    }
}