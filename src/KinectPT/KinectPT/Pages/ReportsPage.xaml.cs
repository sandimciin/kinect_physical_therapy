using System;
using System.IO;
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

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Globalization;


namespace KinectPT
{
    /// <summary>
    /// Interaction logic for ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        public ReportsPage()
        {
            this.InitializeComponent();
            //this.Model = CreateNormalDistributionModel();
            //string userDataFile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\ReportData.csv");
            string userDataFile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\ExerciseTime.csv");
            this.Model = OpenTimes(userDataFile);
            //this.Model = OpenDoubles(userDataFile);

            this.DataContext = this;
        }


        public PlotModel Model { get; set; }

        //private void OpenCsv_Click(object sender, RoutedEventArgs e)
        //{
        //    var dlg = new OpenFileDialog();
        //    dlg.Filter = ".csv files|*.csv";
        //    dlg.DefaultExt = ".csv";
        //    if (dlg.ShowDialog().Value)
        //    {
        //        vm.Open(dlg.FileName);
        //    }
        //}

        public PlotModel OpenDoubles(string file)
        {
            var doc = new CsvDocument();
            doc.Load(file);
            var tmp = new PlotModel { Title = "Generated User Report" /*Path.GetFileNameWithoutExtension(file)*/ };
            //tmp.LegendPosition = LegendPosition.RightTop;
            //tmp.LegendPlacement = LegendPlacement.Outside;
            tmp.IsLegendVisible = false;
            tmp.PlotMargins = new OxyThickness(50, 0, 0, 40);
            for (int i = 1; i < doc.Headers.Length; i++)
            {
                var ls = new LineSeries { Title = doc.Headers[i] };
                foreach (var item in doc.Items)
                {
                    double x = ParseDouble(item[0]);
                    double y = ParseDouble(item[i]);
                    ls.Points.Add(new DataPoint(x, y));
                }
                tmp.Series.Add(ls);
            }
            tmp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = doc.Headers[0],
                TickStyle = TickStyle.Inside
            });
            tmp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = doc.Headers[1],
                TickStyle = TickStyle.Inside
            });
            return tmp;
        }

        public PlotModel OpenTimes(string file)
        {
            var doc = new CsvDocument();
            doc.Load(file);
            var tmp = new PlotModel { Title = "Exercise Duration Over Time" /*Path.GetFileNameWithoutExtension(file)*/ };
            //tmp.LegendPosition = LegendPosition.RightTop;
            //tmp.LegendPlacement = LegendPlacement.Outside;
            tmp.IsLegendVisible = false;
            tmp.PlotMargins = new OxyThickness(50, 0, 0, 40);
            for (int i = 1; i < doc.Headers.Length; i++)
            {
                var ls = new ScatterSeries { Title = doc.Headers[i] };
                foreach (var item in doc.Items)
                {
                    var t1 = DateTime.Parse(item[0]);
                    var t2 = DateTime.Parse(item[i]);

                    double x = DateTimeAxis.ToDouble(t2);
                    double y = DateTimeAxis.ToDouble(t1.Minute);

                    ls.Points.Add(new ScatterPoint(x, y));
                }
                tmp.Series.Add(ls);
            }
            for (int i = 1; i < doc.Headers.Length; i++)
            {
                var ls = new LineSeries { Title = doc.Headers[i] };
                foreach (var item in doc.Items)
                {
                    var t1 = DateTime.Parse(item[0]);
                    var t2 = DateTime.Parse(item[i]);

                    double x = DateTimeAxis.ToDouble(t2);
                    double y = DateTimeAxis.ToDouble(t1.Minute);

                    ls.Points.Add(new DataPoint(x, y));
                }
                tmp.Series.Add(ls);
            }

            tmp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = doc.Headers[0],
                TickStyle = TickStyle.Inside,
                
            });
            tmp.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = doc.Headers[1],
                TickStyle = TickStyle.Inside,
                StringFormat = "MM/dd/yyyy"
            });
            return tmp;
        }


        private double ParseDouble(string s)
        {
            if (s == null)
                return double.NaN;
            s = s.Replace(',', '.');
            double result;
            if (double.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
                return result;
            return double.NaN;
        }

        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_Arm(object sender, RoutedEventArgs e)
        {
            //load arm raises exercise data
        }

        private void Click_Walk(object sender, RoutedEventArgs e)
        {
            //load walking exercise data
        }

        private void Click_Sit(object sender, RoutedEventArgs e)
        {
            //load sitting and standing exercise data
        }
    }
}
