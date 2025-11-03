using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Sidebar;

/// <summary>
/// Represents a tab item in the sidebar control.
/// </summary>
public class SidebarTabItem : TabItem
{
    /// <summary>
    /// Dependency property for the tab icon.
    /// </summary>
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(string), typeof(SidebarTabItem), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Dependency property for the tooltip text.
    /// </summary>
    public static readonly DependencyProperty TooltipTextProperty =
        DependencyProperty.Register(nameof(TooltipText), typeof(string), typeof(SidebarTabItem), new PropertyMetadata(string.Empty));

    /// <summary>
    /// Gets or sets the icon character for the tab.
    /// </summary>
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// Gets or sets the tooltip text for the tab.
    /// </summary>
    public string TooltipText
    {
        get => (string)GetValue(TooltipTextProperty);
        set => SetValue(TooltipTextProperty, value);
    }

    /// <summary>
    /// Initializes static members of the SidebarTabItem class.
    /// </summary>
    static SidebarTabItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarTabItem), new FrameworkPropertyMetadata(typeof(SidebarTabItem)));
    }

    /// <summary>
    /// Initializes a new instance of the SidebarTabItem class.
    /// </summary>
    public SidebarTabItem()
    {
        // Set tooltip binding
        SetBinding(ToolTipProperty, new System.Windows.Data.Binding(nameof(TooltipText)) { Source = this });
    }
}