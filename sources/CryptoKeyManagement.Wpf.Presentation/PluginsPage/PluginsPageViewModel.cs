using AsyncMediator;
using DustInTheWind.CryptoKeyManagement.Infrastructure;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.Events;
using DustInTheWind.CryptoKeyManagement.Wpf.Application.UseCases.InitializePluginsPage;
using System.Collections.ObjectModel;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;

public class PluginsPageViewModel : ViewModelBase
{
    private readonly IMediator mediator;

    public ObservableCollection<PluginDto> Plugins { get; } = [];

    public CopyToClipboardCommand CopyToClipboardCommand { get; }
    
    public SetDefaultPluginCommand SetDefaultPluginCommand { get; }

    public PluginsPageViewModel(IMediator mediator, IEventBus eventBus)
    {
        ArgumentNullException.ThrowIfNull(eventBus);
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        CopyToClipboardCommand = new CopyToClipboardCommand();
        SetDefaultPluginCommand = new SetDefaultPluginCommand(mediator);

        eventBus.Subscribe<DefaultPluginChangedEvent>(HandleDefaultPluginChangedEvent);

        _ = InitializeAsync();
    }

    private Task HandleDefaultPluginChangedEvent(DefaultPluginChangedEvent @event, CancellationToken token)
    {
        return InitializeAsync();
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