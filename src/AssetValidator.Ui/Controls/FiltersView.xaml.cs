using System.Windows;

namespace AssetValidator.Ui.Controls;

public partial class FiltersView
{
    public event EventHandler? OnFiltersChanged;

    public bool ShowInfo
    {
        get => (bool)GetValue(ShowInfoProperty);
        set => SetValue(ShowInfoProperty, value);
    }

    public bool ShowWarnings
    {
        get => (bool)GetValue(ShowWarningsProperty);
        set => SetValue(ShowWarningsProperty, value);
    }

    public bool ShowErrors
    {
        get => (bool)GetValue(ShowErrorsProperty);
        set => SetValue(ShowErrorsProperty, value);
    }

    public string SearchText
    {
        get => (string)GetValue(SearchTextProperty);
        set => SetValue(SearchTextProperty, value);
    }
    
    public static readonly DependencyProperty ShowInfoProperty = DependencyProperty.Register(
        nameof(ShowInfo),
        typeof(bool),
        typeof(FiltersView),
        new PropertyMetadata(true, NotifyAboutFiltersChange)
    );

    public static readonly DependencyProperty ShowWarningsProperty = DependencyProperty.Register(
        nameof(ShowWarnings),
        typeof(bool),
        typeof(FiltersView),
        new PropertyMetadata(true, NotifyAboutFiltersChange)
    );

    public static readonly DependencyProperty ShowErrorsProperty = DependencyProperty.Register(
        nameof(ShowErrors),
        typeof(bool),
        typeof(FiltersView),
        new PropertyMetadata(true, NotifyAboutFiltersChange)
    );

    public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(
        nameof(SearchText),
        typeof(string),
        typeof(FiltersView),
        new PropertyMetadata(string.Empty, NotifyAboutFiltersChange)
    );

    public FiltersView()
    {
        InitializeComponent();
    }

    private static void NotifyAboutFiltersChange(DependencyObject dependency, DependencyPropertyChangedEventArgs _)
    {
        ((FiltersView)dependency).OnFiltersChanged?.Invoke(dependency, EventArgs.Empty);
    }
}