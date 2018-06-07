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
        /// <summary>
        /// Initialize the Reports Page and open the ArmRaises CSV Report by default.
        /// </summary>
        public ReportsPage()
        {
            this.InitializeComponent();
            userDataFile = Path.Combine(Environment.CurrentDirectory, @"..\..\..\UserData\ArmRaisesReportData.csv");
            if (File.Exists(userDataFile))
            {
                this.Model = OpenTimes(userDataFile);
                this.Model.Title = "Arm Raise Exercise Duration Over Time";
            }
            this.DataContext = this;
        }

        /// <summary>
        /// The model for which were are going to plot our results.
        /// </summary>
        public PlotModel Model { get; set; }
        public string userDataFile { get; set; }

        /// <summary>
        /// Opens a given CSV file containing date-time entries for a specific exercise and creates OxyPlot 
        /// line and scatter series for which to graph. The axes for the graph are standard value (int/float) axes.
        /// </summary>
        /// <param name="file">Given a string file name, in this case a path 
        /// for where the CSV file specific to which exercise you want to plot</param>
        /// <returns>Returns a plot model created above</returns>
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

        /// <summary>
        /// Opens a given CSV file containing date-time entries for a specific exercise and creates OxyPlot 
        /// line and scatter series for which to graph. The axes for the graph are date-time axes for 
        /// plotting data that is in date-time format. The axes headers correspond to columns in the CSV file
        /// and the items of those headers are added to the line/scatter series.
        /// </summary>
        /// <param name="file">Given a string file name, in this case a path 
        /// for where the CSV file specific to which exercise you want to plot</param>
        /// <returns>Returns a plot model created above</returns>
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
             /* Here we create the second plotting series. This is done because 
              * because we want our plotted points to be connected which is done
              * by drawing a secondary line series instead of a scatter series.
              * This gives the desired effect of a point-connected graph.
             */
            
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
            // This will be the Left Y Axis 
            tmp.Axes.Add(new TimeSpanAxis
            {
                Position = AxisPosition.Left,
                Title = doc.Headers[0],
                TickStyle = TickStyle.Inside,
            });

            // This will be the Bottom X Axis
            tmp.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = doc.Headers[1],
                TickStyle = TickStyle.Inside,
                StringFormat = "MM/dd/yyyy"
            });
            return tmp;
        }

        /// <summary>
        /// A helper function for parsing data in a CSV file of type 'double'.
        /// </summary>
        /// <param name="s">double in string format</param>
        /// <returns>A double if it is a valid double. Returns double.NaN(not a number) otherwise</returns>
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
        /// <summary>
        /// An event handler for our main page navigation. When the user clicks the back button,
        /// they will be navigated back to the main page.
        /// </summary>
        /// <param name="sender">Event Handler</param>
        /// <param name="e">Event that contains state information and event data</param>
        private void Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        /// <summary>
        /// Event handler for when a user clicks the ArmRaises button in the reports page.
        /// </summary>
        /// <param name="sender">Event Handler</param>
        /// <param name="e">Event that contains state information and event data</param>
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

        /// <summary>
        /// Event handler for when a user clicks the Walking Exercise button in the reports page.
        /// </summary>
        /// <param name="sender">Event Handler</param>
        /// <param name="e">Event that contains state information and event data</param>
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

        /// <summary>
        /// Event handler for when a user clicks the Sitting Exercise button in the reports page.
        /// </summary>
        /// <param name="sender">Event Handler</param>
        /// <param name="e">Event that contains state information and event data</param>
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
