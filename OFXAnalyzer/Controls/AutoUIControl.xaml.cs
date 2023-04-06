using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiqUtils.Wpf.UIBuilders;

namespace OFXAnalyzer.Controls
{
    /// <summary>
    /// Interaction logic for AutoUIControl.xaml
    /// </summary>
    public partial class AutoUIControl : UserControl
    {

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(
                nameof(Item),
                typeof(object),
                typeof(AutoUIControl),
                new PropertyMetadata(DateTime.Now, DatePropertyChanged));

        private void DatePropertyChanged(object item)
        {
            this.ContentControl.Children.Clear();
            if (item != null)
            {
                this.Item.CreateAutoSettingsControl(this.ContentControl);
            }
        }

        private static void DatePropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AutoUIControl)d).DatePropertyChanged(e.NewValue);
        }

        public AutoUIControl()
        {
            this.InitializeComponent();
        }

        public object Item
        {
            get
            {
                return (object)this.GetValue(ItemProperty);
            }
            set
            {
                this.SetValue(ItemProperty, value);
            }
        }
    }
}
