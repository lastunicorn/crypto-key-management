using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.Controls;

public class ImageButton : Button
{
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
        "Image",
        typeof(DrawingImage),
        typeof(ImageButton));

    public static DrawingImage GetImage(DependencyObject obj)
    {
        return (DrawingImage)obj.GetValue(ImageProperty);
    }

    public static void SetImage(DependencyObject obj, DrawingImage value)
    {
        obj.SetValue(ImageProperty, value);
    }

    static ImageButton()
    {
        // Override the default style key to target this specific type
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
    }
}