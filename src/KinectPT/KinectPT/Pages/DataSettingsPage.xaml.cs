using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
        }

        //Event handler for clicking Back button
        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        //Event handler for selecting duration unit
        void durationSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selected = durationBox.SelectedValue.ToString();
            Application.Current.Properties["durationUnit"] = selected.Substring(38, 7);

        }

        //Event handler for selecting begin at window open
        private void RadioButton1_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["beginAtExerciseStart"] = false;

        }

        //Event handler for selecting begin at exercise start
        private void RadioButton2_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["beginAtExerciseStart"] = true;
        }
    }

   
}
