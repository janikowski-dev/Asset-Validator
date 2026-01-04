using System.Collections;
using System.Windows;

namespace AssetValidator.Ui.Controls;

public partial class ResultsView
{
    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(ResultsView),
        new PropertyMetadata(null)
    );
    
    public ResultsView()
    {
        InitializeComponent();
    }
}