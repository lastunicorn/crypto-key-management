using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializePluginsPage;
using System.Collections.ObjectModel;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;

public class PluginsPageViewModel : ViewModelBase
{
    private readonly IMediator mediator;

    public ObservableCollection<PluginDto> Plugins { get; } = [];

    public CopyToClipboardCommand CopyToClipboardCommand { get; }

    public PluginsPageViewModel(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        CopyToClipboardCommand = new CopyToClipboardCommand();

        _ = InitializeAsync();
    }

    private Task InitializeAsync()
    {
        return AsInitializationAsync(async () =>
        {
            InitializePluginsPageRequest request = new();
            InitializePluginsPageResponse response = await mediator.Query<InitializePluginsPageRequest, InitializePluginsPageResponse>(request);

            Plugins.Clear();
            foreach (PluginDto plugin in response.Plugins)
                Plugins.Add(plugin);
        });
    }
}