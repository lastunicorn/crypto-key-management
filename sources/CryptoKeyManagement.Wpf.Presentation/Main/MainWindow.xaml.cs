using System.Windows;
using System.Windows.Controls;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Main;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;
using DustInTheWind.CryptoKeyManagement.Wpf.Presentation.SigningPage;

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

        ConfigureSidebarTabs();
    }

    private void ConfigureSidebarTabs()
    {
        //SidebarControl sidebar = SidebarControl;
        //SidebarTabControl tabControl = sidebar.TabControl;

        //// Subscribe to selection changed event
        //tabControl.SelectionChanged += TabControl_SelectionChanged;

        //// Add Signing Page tab (using existing functionality)
        //SidebarTabItem signingTab = new SidebarTabItem
        //{
        //    Icon = "S",
        //    TooltipText = "Signing page",
        //    Content = new SigningPage()
        //};
        //// Set the DataContext for the signing page
        //if (signingTab.Content is SigningPage signingPage && DataContext is MainViewModel mainViewModel)
        //{
        //    signingPage.DataContext = mainViewModel.SigningPageViewModel;
        //}
        //tabControl.Items.Add(signingTab);

        //// Add Plugins tab
        //SidebarTabItem pluginsTab = new SidebarTabItem
        //{
        //    Icon = "P",
        //    TooltipText = "Plug-ins page",
        //    Content = new PluginsPage()
        //};
        //tabControl.Items.Add(pluginsTab);

        //// Set the first tab as selected and show its content
        //tabControl.SelectedIndex = 0;
        //if (tabControl.Items.Count > 0 && tabControl.Items[0] is SidebarTabItem firstTab)
        //{
        //    MainContentPresenter.Content = firstTab.Content;
        //}
    }

    //private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    if (sender is SidebarTabControl tabControl && tabControl.SelectedItem is SidebarTabItem selectedTab)
    //    {
    //        // Update the main content area with the selected tab's content
    //        MainContentPresenter.Content = selectedTab.Content;
    //    }
    //}
}