//------------------------------------------------------------------------------
// <copyright file="ButtonSample.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
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
using System.Windows.Shapes;

namespace Microsoft.Samples.Kinect.ControlsBasics
{

    /// <summary>
    /// Interaction logic for ReportGeneration
    /// </summary>
    public partial class ReportGeneration : UserControl
    {
        StreamReader _reader;
        public class ProcessInfo
        {
            public string MaxAngle { get; set; }
            public int PID { get; set; }
            public int CPU { get; set; }
            public int Thd { get; set; }
            public int Hnd { get; set; }
            public TimeSpan CpuTime { get; set; }
            public TimeSpan ElapsedTime { get; set; }
        }


        public ReportGeneration()
        {
            this.InitializeComponent();
            //_reader = new StreamReader("Assets/test1.txt");
            //List<ProcessInfo> processes = new List<ProcessInfo>();
            //_reader.ReadLine();


            //string currentLine = _reader.ReadLine();

            //while (currentLine != null)
            //{
            //    ProcessInfo newInfo = new ProcessInfo();
            //    //Actual parsing left up to the reader; String.Split is your friend.
            //    String[] substrings = currentLine.Split('\t');
            //    newInfo.MaxAngle = substrings[0];
            //    processes.Add(newInfo);
            //}
        }

        
       

    }
}
