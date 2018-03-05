//------------------------------------------------------------------------------
// <copyright file="ButtonSample.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
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

namespace Microsoft.Samples.Kinect.ControlsBasics
{

    /// <summary>
    /// Interaction logic for ExerciseOptions
    /// </summary>
    public partial class ExerciseOptions : UserControl
    {
        AnglePage ArmRaisesPage = new AnglePage();
        SecondExercise secondExercisePage = new SecondExercise();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseOptions" /> class.
        /// </summary>
        public ExerciseOptions()
        {
            this.InitializeComponent();
        }

        private void Click_ArmRaises(object sender, RoutedEventArgs e)
        {
            //AnglePage armRaisePage = new AnglePage();
            // this.NavigationService.Navigate(armRaisePage);
            Content = ArmRaisesPage;
        }

        private void Click_SecondExercise(object sender, RoutedEventArgs e)
        {
            //AnglePage armRaisePage = new AnglePage();
            // this.NavigationService.Navigate(armRaisePage);
            Content = secondExercisePage;
        }
    }
}
