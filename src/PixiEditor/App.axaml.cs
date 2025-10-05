using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using PixiEditor.Extensions.CommonApi.UserPreferences;
using PixiEditor.Initialization;

namespace PixiEditor;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    // Helps telling Avalonia which theme-mode to use
    public void SetTheme()
    {
        string themeVariant = IPreferences.Current.GetPreference(PreferencesConstants.RequestedTheme, PreferencesConstants.RequestedThemeDefault);
        RequestedThemeVariant = convertThemeName(themeVariant);
    }

    private ThemeVariant convertThemeName(string requestedTheme)
    {
        ThemeVariant variant = requestedTheme switch
        {
            "Dark" => ThemeVariant.Dark,
            "Light" => ThemeVariant.Light,
            "Default" => ThemeVariant.Default,
            _ => ThemeVariant.Default
        };
        return variant;
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ClassicDesktopEntry entry = new(desktop);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            throw new NotImplementedException();
            //singleViewPlatform.MainView = new MainView { DataContext = new MainViewModel() };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
