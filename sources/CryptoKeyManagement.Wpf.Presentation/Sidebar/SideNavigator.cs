using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;

public class SideNavigator : TabControl
{
    public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(
        nameof(Buttons),
        typeof(ObservableCollection<Button>),
        typeof(SideNavigator)
    );

    public ObservableCollection<Button> Buttons
    {
        get => (ObservableCollection<Button>)GetValue(ButtonsProperty);
        set => SetValue(ButtonsProperty, value);
    }

    static SideNavigator()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SideNavigator), new FrameworkPropertyMetadata(typeof(SideNavigator)));
    }

    public SideNavigator()
    {
        Buttons = [];
    }
}
