using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace InteractServer.Utils
{
    [ValueConversion(typeof(string), typeof(bool))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            FileAttributes attr = File.GetAttributes(s);

            // default icon
            Uri uri = new Uri("pack://application:,,,/Resources/Images/page.gif");

            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // switch to folder icon
                uri = new Uri("pack://application:,,,/Resources/Images/folder.gif");
            } else
            {
                string ext = Path.GetExtension(s);
                if(ext.Equals(".png")) {
                    uri = new Uri("pack://application:,,,/Resources/Images/image.gif");
                } else if(ext.Equals(".ogg"))
                {
                    uri = new Uri("pack://application:,,,/Resources/Images/page_sound.gif");
                } else if (ext.Equals(".model"))
                {
                    uri = new Uri("pack://application:,,,/Resources/Images/icon_package.gif");
                } else if (ext.Equals(".intp"))
                {
                    uri = new Uri("pack://application:,,,/Resources/Images/page_settings.gif");
                }
            }

            BitmapImage source = new BitmapImage(uri);
            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Cannot convert back");
        }
    }
}
