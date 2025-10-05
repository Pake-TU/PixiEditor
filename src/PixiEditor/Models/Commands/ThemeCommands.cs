using Avalonia;
using Avalonia.Styling;
using static PixiEditor.Models.Commands.Attributes.Commands.Command;

namespace PixiEditor.Models.Commands
{

    internal class ThemeCommands
    {
        // This method will be executed when the command with ID "PixiEditor.Application.ToggleTheme" runs
        public void ToggleTheme()
        {
            var app = (App)Application.Current;
            bool isDark = app.RequestedThemeVariant == ThemeVariant.Dark;
            app.SetTheme(!isDark);
        }
    }
}
