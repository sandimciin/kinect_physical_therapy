using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            userDataFile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\ArmRaisesReportData.csv");
            if (File.Exists(userDataFile))
            {
                this.Model = OpenTimes(userDataFile);
            }


            this.DataContext = this;
        }


        public PlotModel Model { get; set; }
        public string userDataFile { get; set; }


        public PlotModel OpenDoubles(string file)
        {
            var doc = new CsvDocument();
            doc.Load(file);
            var tmp = new PlotModel { Title = "Generated User Report" };
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
            var tmp = new PlotModel { Title = "Exercise Duration Over Time" };
            tmp.IsLegendVisible = false;
            tmp.PlotMargins = new OxyThickness(50, 0, 0, 40);
            for (int i = 1; i < doc.Headers.Length; i++)
            {
                var ls = new ScatterSeries { Title = doc.Headers[i] };
                foreach (var item in doc.Items)
                {
                    var t1 = TimeSpan.Parse(item[0]);
                    var t2 = DateTime.Parse(item[i]);

                    double x = DateTimeAxis.ToDouble(t2);
                    double y = TimeSpanAxis.ToDouble(t1);

                    ls.Points.Add(new ScatterPoint(x, y));
                }
                tmp.Series.Add(ls);
            }
            for (int i = 1; i < doc.Headers.Length; i++)
            {
                var ls = new LineSeries { Title = doc.Headers[i] };
                foreach (var item in doc.Items)
                {
                    var t1 = TimeSpan.Parse(item[0]);
                    var t2 = DateTime.Parse(item[i]);

                    double x = DateTimeAxis.ToDouble(t2);
                    double y = TimeSpanAxis.ToDouble(t1);

                    ls.Points.Add(new DataPoint(x, y));
                }
                tmp.Series.Add(ls);
            }

            tmp.Axes.Add(new TimeSpanAxis
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
            this.DataContext = null;
            //load walking exercise data
            userDataFile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\ArmRaisesReportData.csv");
            if (File.Exists(userDataFile))
            {
                this.Model = OpenTimes(userDataFile);
                this.Model.Title = "Arm Raise Exercise Duration Over Time";
                this.DataContext = this;
                this.Model.InvalidatePlot(true);
            }
            else
            {
                this.Model = null;
            }

        }

        private void Click_Walk(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            //load walking exercise data
            userDataFile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\WalkingReportData.csv");
            if (File.Exists(userDataFile))
            {
                this.Model = OpenTimes(userDataFile);
                this.Model.Title = "Walking Exercise Duration Over Time";
                this.DataContext = this;
                this.Model.InvalidatePlot(true);
            }
            else
            {
                this.Model = null;
            }
        }

        private void Click_Sit(object sender, RoutedEventArgs e)
        {
            //load sitting and standing exercise data
            this.DataContext = null;
            //load walking exercise data
            userDataFile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\SittingReportData.csv");
            if (File.Exists(userDataFile))
            {
                this.Model = OpenTimes(userDataFile);
                this.Model.Title = "Sitting Exercise Duration Over Time";
                this.DataContext = this;
                this.Model.InvalidatePlot(true);
            }
            else
            {
                this.Model = null;
            }
        }
    }
}
