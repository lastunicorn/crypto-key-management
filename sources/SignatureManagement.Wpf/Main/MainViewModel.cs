using System.Collections.ObjectModel;
using System.Windows;
using DustInTheWind.SignatureManagement.Ports.SignatureAccess;

namespace SignatureManagement.Wpf.Main;

internal class MainViewModel
{
    private readonly ISignatureKeyRepository _signatureKeyRepository;

    public ObservableCollection<SignatureKeyViewModel> SignatureKeys { get; private set; }

    public MainViewModel()
    {

        _signatureKeyRepository = new SignatureKeyRepository();
        LoadSignatureKeys();
    }

    private void LoadSignatureKeys()
    {
        try
        {
            IEnumerable<SignatureKeyViewModel> signatureKeys = _signatureKeyRepository.GetAll()
                .Select(SignatureKeyViewModel.FromSignatureKey);

            SignatureKeys = new ObservableCollection<SignatureKeyViewModel>(signatureKeys);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading signature keys: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
