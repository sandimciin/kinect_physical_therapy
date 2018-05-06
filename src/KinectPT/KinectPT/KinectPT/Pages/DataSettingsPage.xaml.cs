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
using System.Globalization;

namespace KinectPT
{
    /// <summary>
    /// Interaction logic for DataSettingsPage.xaml
    /// </summary>
    public partial class DataSettingsPage : Page
    {
        public DataSettingsPage()
        {
            InitializeComponent();

            exerciseStart.Text = Application.Current.Properties["beginAtExerciseStart"].ToString();
        }

        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        void durationSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selected = durationBox.SelectedValue.ToString();
            Application.Current.Properties["durationUnit"] = selected.Substring(38, 7);
            
        }

        
    }

    public class EnumMatchToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue,
                     StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            bool useValue = (bool)value;
            string targetValue = parameter.ToString();
            if (useValue)
                return Enum.Parse(targetType, targetValue);

            return null;
        }
    }
}
