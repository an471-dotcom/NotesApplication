using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI;

namespace Notes_Application.Utility_Classes
{
    internal class UtilityClass
    {
        public static Color GetColorFromHex(string colorString)
        {

            byte a = byte.Parse(colorString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            byte r = byte.Parse(colorString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(colorString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(colorString.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
            return Windows.UI.Color.FromArgb(a, r, g, b);
        }

        public static string GetDescriptionFromEnum(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        public static string TruncateString(string value,int length)
        {
            
            if(value!= null)
            {
                return value.Length <= length ? value : value.Substring(0, length) + "...";
            }
            return string.Empty;

        }
    }
    
   

   
}
