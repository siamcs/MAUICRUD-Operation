using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeMAUI.Model
{
    public class Base64ToImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64String)
            {
                try
                {
                    byte[] imageBytes = System.Convert.FromBase64String(base64String);
                    return ImageSource.FromStream(() => new System.IO.MemoryStream(imageBytes));
                }
                catch (Exception)
                {

                }
            }


            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
