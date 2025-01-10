using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Client.App.Converters
{
    public class StringPrefixConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int prefixLength = 15;

            var converter = new Int32Converter();

            if (parameter != null)
                prefixLength = (int)converter.ConvertFrom((string)parameter);

            return ((string)value)[..Math.Min(prefixLength, ((string)value).Length)];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
