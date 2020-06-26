using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ComissionGeneratorUWP.ViewModel
{
    public static class Converters
    {
        /// <summary>
        /// Returns the reverse of provided value
        /// </summary>
        public static bool Not(bool value) => !value;

        /// <summary>
        /// Returns Visibility.Collapsed if value is true, and Visibility.Visible otherise
        /// </summary>
        public static Visibility CollapsedIf(bool value) =>
            value ? Visibility.Collapsed : Visibility.Visible;

        /// <summary>
        /// Returns Visibility.Visible if value is true, and Visibility.Collapsed otherise
        /// </summary>
        public static Visibility CollapsedIfNot(bool value) =>
            value ? Visibility.Visible : Visibility.Collapsed;

        public static decimal StringToDecimalConverter(string str) =>
             Decimal.Parse(str);
    }
}
