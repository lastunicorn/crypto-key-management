using AsyncMediator;
using DustInTheWind.SignatureManagement.Ports.StateAccess;

namespace DustInTheWind.SignatureManagement.Wpf.Application.SelectSignatureKey;

internal class SelectSignatureKeyUseCase : ICommandHandler<SelectSignatureKeyCommand>
{
    private readonly IApplicationState applicationStateService;

    public SelectSignatureKeyUseCase(IApplicationState applicationStateService)
    {
        this.applicationStateService = applicationStateService ?? throw new ArgumentNullException(nameof(applicationStateService));
    }

    public Task<ICommandWorkflowResult> Handle(SelectSignatureKeyCommand command)
    {
        applicationStateService.SelectedSignatureKeyId = command.SignatureKeyId;

        CommandWorkflowResult result = new();
        return Task.FromResult<ICommandWorkflowResult>(result);
    }
}