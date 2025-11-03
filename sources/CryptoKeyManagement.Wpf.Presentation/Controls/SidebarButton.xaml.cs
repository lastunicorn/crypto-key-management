using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Controls;

/// <summary>
/// A custom sidebar button that automatically applies the SidebarButtonStyle.
/// This control ensures consistent styling and automatic theme changes.
/// </summary>
public class SidebarButton : Button
{
    /// <summary>
    /// Initializes static members of the SidebarButton class.
    /// </summary>
    static SidebarButton()
    {
        // Override the default style key to target this specific type
        DefaultStyleKeyProperty.OverrideMetadata(typeof(SidebarButton), new FrameworkPropertyMetadata(typeof(SidebarButton)));
    }

    /// <summary>
    /// Initializes a new instance of the SidebarButton class.
    /// </summary>
    public SidebarButton()
    {
        // Set the style to use the SidebarButtonStyle resource
        //SetResourceReference(StyleProperty, "SidebarButtonStyle");
    }
}