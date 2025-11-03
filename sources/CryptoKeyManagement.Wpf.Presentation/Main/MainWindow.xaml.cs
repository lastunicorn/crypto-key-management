using System.Windows;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Main;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}