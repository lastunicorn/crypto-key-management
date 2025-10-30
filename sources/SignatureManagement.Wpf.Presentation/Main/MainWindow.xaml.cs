using System.Windows;
using DustInTheWind.SignatureManagement.Wpf.Presentation.Main;

namespace DustInTheWind.SignatureManagement.Wpf.Main;

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