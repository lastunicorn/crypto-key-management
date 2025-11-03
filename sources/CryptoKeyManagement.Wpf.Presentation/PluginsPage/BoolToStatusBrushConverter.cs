using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DustInTheWind.CryptoKeyManagement.Wpf.Presentation.PluginsPage;

public class BoolToStatusBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isActive)
        {
            return isActive
                ? new SolidColorBrush(Colors.LightGreen)
                : new SolidColorBrush(Colors.LightGray);
        }

        return new SolidColorBrush(Colors.LightGray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}