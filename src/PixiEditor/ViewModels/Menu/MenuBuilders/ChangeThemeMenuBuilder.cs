using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia;
using PixiEditor.Extensions.UI;
using PixiEditor.UI.Common.Localization;
using CommunityToolkit.Mvvm.Input; // For RelayCommand / SimpleCommand

namespace PixiEditor.ViewModels.Menu.MenuBuilders;

internal class ChangeThemeMenuBuilder : MenuItemBuilder
{
    public override void ModifyMenuTree(ICollection<MenuItem> tree)
    {
        // Try to find the "VIEW" menu
        if (TryFindMenuItem(tree, "VIEW", out MenuItem? viewItem))
        {
            var toggleThemeItem = new MenuItem
            {
                //Header = "Toggle Themebbbbb",
                Command = new RelayCommand(() =>
                {
                    // Access the current application
                    var app = (App)Application.Current;

                    // Determine if the current theme is dark
                    bool isDark = app.RequestedThemeVariant == ThemeVariant.Dark;

                    // Toggle between dark and light theme
                    app.SetTheme(!isDark);
                })
            };

            Translator.SetKey(toggleThemeItem, "CHANGE_THEME"); // Optional localization
            viewItem.Items.Add(toggleThemeItem);
        }
    }

    public override void ModifyMenuTree(ICollection<NativeMenuItem> tree)
    {
        // Optional: implement for native menu support if needed
    }
}
