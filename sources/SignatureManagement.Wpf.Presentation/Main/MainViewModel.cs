using System.Collections.ObjectModel;
using AsyncMediator;
using SignatureManagement.Wpf.Application.InitializeMain;

namespace SignatureManagement.Wpf.Main;

public class MainViewModel
{
    private readonly IMediator mediator;

    public ObservableCollection<SignatureKeyViewModel> SignatureKeys { get; private set; }

    public MainViewModel(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        InitializeMainRequest request = new();
        InitializeMainResponse response = await mediator.Query<InitializeMainRequest, InitializeMainResponse>(request);

        SignatureKeys = new ObservableCollection<SignatureKeyViewModel>(response.SignatureKeys.ToViewModels());
    }
}
