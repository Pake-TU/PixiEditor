using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using PixiEditor.UI.Common.Rendering;

namespace PixiEditor.UI.Common.Fonts;

public static class PixiPerfectIconExtensions
{
    private static readonly FontFamily pixiPerfectFontFamily =
        new("avares://PixiEditor.UI.Common/Fonts/PixiPerfect.ttf#pixiperfect");


    public static Stream GetFontStream()
    {
        return AssetLoader.Open(new Uri("avares://PixiEditor.UI.Common/Fonts/PixiPerfect.ttf"));
    }

    public static IImage ToIcon(string unicode, double size = 18)
    {
        if (string.IsNullOrEmpty(unicode)) return null;

        if (Application.Current.ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark)
        {
            return new IconImage(unicode, pixiPerfectFontFamily, size, Colors.White);
        }

        return new IconImage(unicode, pixiPerfectFontFamily, size, Colors.Black);
    }

    public static IImage ToIcon(string unicode, double size, double rotation)
    {
        if (string.IsNullOrEmpty(unicode)) return null;

        if (Application.Current.ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark)
        {
            return new IconImage(unicode, pixiPerfectFontFamily, size, Colors.White, rotation);
        }

        return new IconImage(unicode, pixiPerfectFontFamily, size, Colors.Black, rotation);
    }

    public static string? TryGetByName(string? icon)
    {
        if (string.IsNullOrEmpty(icon))
        {
            return null;
        }

        if (Application.Current.Styles.TryGetResource(icon, null, out object resource))
        {
            return resource as string;
        }

        return icon;
    }
}
