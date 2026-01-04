using System.Windows;

namespace AssetValidator.Ui.Controls;

public partial class HeaderView
{
    public event EventHandler? OnValidationRequired;
    
    public HeaderView()
    {
        InitializeComponent();
    }

    private void RequireValidation(object _, RoutedEventArgs __)
    {
        OnValidationRequired?.Invoke(this, EventArgs.Empty);
    }
}