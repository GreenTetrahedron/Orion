using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Text;

namespace Orion.Client.App.Converters
{
    public class UsernameToAbbreviationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string username = value as string;

            string res = "";

            foreach(var word in username.Split(' '))
            {
                res += word[0];

                if (res.Length == 2)
                    return res;
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
