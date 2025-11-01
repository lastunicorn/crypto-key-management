using DustInTheWind.SignatureManagement.Wpf.Presentation.Services;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Sidebar;

public class SidebarViewModel : ViewModelBase
{
    private readonly ThemeSelector themeSelector;

    public string ThemeToggleText => themeSelector.IsDarkTheme
        ? "🌜"
        : "🌞";

    public ToggleThemeCommand ToggleThemeCommand { get; }

    public SidebarViewModel(ThemeSelector themeSelector, ToggleThemeCommand toggleThemeCommand)
    {
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));
        ToggleThemeCommand = toggleThemeCommand ?? throw new ArgumentNullException(nameof(toggleThemeCommand));

        themeSelector.ThemeChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(ThemeToggleText));
        };
    }
}