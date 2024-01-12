using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DigitalClean;
using System.Windows.Media.Animation;

namespace DigitalSoap
{
    public class ContextTitle : DependencyObject
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached("Title", typeof(string),
                typeof(ContextTitle), new PropertyMetadata("Context Title"));

        public static void SetTitle(DependencyObject _Target, string _Title)
        {
            _Target.SetValue(TitleProperty, _Title);
        }

        public static string GetTitle(DependencyObject _Target)
        {
            return (string)_Target.GetValue(TitleProperty);
        }
    }
}
