using AsyncMediator;
using DustInTheWind.SignatureManagement.Infrastructure;
using DustInTheWind.SignatureManagement.Wpf.Application.UseCases.InitializeMain;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeyInfo;
using DustInTheWind.SignatureManagement.Wpf.Presentation.KeysSelector;
using DustInTheWind.SignatureManagement.Wpf.Presentation.SigningPanel;
using System.Reflection;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Services;

namespace DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

public class MainViewModel : ViewModelBase
{
    private readonly IMediator mediator;
    private readonly ThemeSelector themeSelector;

    public string WindowTitle { get; }

    public KeysSelectorViewModel KeysSelectorViewModel { get; }

    public KeyInfoViewModel KeyInfoViewModel { get; }

    public SigningPanelViewModel SigningPanelViewModel { get; }


    public string ThemeToggleText => themeSelector.IsDarkTheme
        ? "Dark"
        : "Light";

    public ToggleThemeCommand ToggleThemeCommand { get; }

    public MainViewModel(IMediator mediator, EventBus eventBus,
        KeysSelectorViewModel keysSelectorViewModel, SigningPanelViewModel signingPanelViewModel,
        KeyInfoViewModel keyInfoViewModel, ThemeSelector themeSelector, ToggleThemeCommand toggleThemeCommand)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        KeysSelectorViewModel = keysSelectorViewModel ?? throw new ArgumentNullException(nameof(keysSelectorViewModel));
        SigningPanelViewModel = signingPanelViewModel ?? throw new ArgumentNullException(nameof(signingPanelViewModel));
        KeyInfoViewModel = keyInfoViewModel ?? throw new ArgumentNullException(nameof(keyInfoViewModel));
        this.themeSelector = themeSelector ?? throw new ArgumentNullException(nameof(themeSelector));
        ToggleThemeCommand = toggleThemeCommand ?? throw new ArgumentNullException(nameof(toggleThemeCommand));

        WindowTitle = GetWindowTitle();

        themeSelector.ThemeChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(ThemeToggleText));
        };

        _ = InitializeAsync();
    }

    private static string GetWindowTitle()
    {
        var assembly = Assembly.GetEntryAssembly();
        var version = assembly?.GetName().Version?.ToString(3) ?? "Unknown";
        return $"Signature Keys Management {version}";
    }

    private Task InitializeAsync()
    {
        return AsInitializationAsync(async () =>
        {
            InitializeMainRequest request = new();
            InitializeMainResponse response = await mediator.Query<InitializeMainRequest, InitializeMainResponse>(request);

            KeysSelectorViewModel.Initialize(response.SignatureKeys, response.SelectedSignatureKeyId);
        });
    }
}
