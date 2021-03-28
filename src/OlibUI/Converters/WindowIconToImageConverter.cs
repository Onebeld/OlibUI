using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace OlibUI.Converters
{
    public class WindowIconToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                WindowIcon wIcon = value as WindowIcon;
                MemoryStream stream = new MemoryStream();
                wIcon.Save(stream);
                stream.Position = 0;
                try
                {
                    return new Bitmap(stream);
                }
                catch
                {
                    return null;
                }
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
