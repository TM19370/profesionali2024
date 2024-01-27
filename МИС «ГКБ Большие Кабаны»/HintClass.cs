using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace МИС__ГКБ_Большие_Кабаны_
{
    public class HintClass
    {
        public static String GetCaption(DependencyObject obj)
        {
            return (String)obj.GetValue(CaptionProperty);
        }

        public static void SetCaption(DependencyObject obj, String value)
        {
            obj.SetValue(CaptionProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.RegisterAttached("Caption", typeof(String), typeof(EnterWindow), new UIPropertyMetadata(null));
    }
}
