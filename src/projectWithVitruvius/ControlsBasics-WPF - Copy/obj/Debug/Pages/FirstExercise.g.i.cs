﻿#pragma checksum "..\..\..\Pages\FirstExercise.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2497CCB08818EBE8CE9533D4A12D7B14D87CE38E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LightBuzz.Vitruvius.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Microsoft.Samples.Kinect.ControlsBasics {
    
    
    /// <summary>
    /// AnglePage
    /// </summary>
    public partial class AnglePage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\Pages\FirstExercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock pageTitle;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Pages\FirstExercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run tblAngle;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Pages\FirstExercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run tblGesture;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Pages\FirstExercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Documents.Run ArmRaiseCount;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Pages\FirstExercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LightBuzz.Vitruvius.Controls.KinectViewer viewer;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\Pages\FirstExercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LightBuzz.Vitruvius.Controls.KinectAngle angle;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Pages\FirstExercise.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Instructions;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ControlsBasics-WPF;component/pages/firstexercise.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\FirstExercise.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\Pages\FirstExercise.xaml"
            ((Microsoft.Samples.Kinect.ControlsBasics.AnglePage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.pageTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.tblAngle = ((System.Windows.Documents.Run)(target));
            return;
            case 4:
            this.tblGesture = ((System.Windows.Documents.Run)(target));
            return;
            case 5:
            this.ArmRaiseCount = ((System.Windows.Documents.Run)(target));
            return;
            case 6:
            this.viewer = ((LightBuzz.Vitruvius.Controls.KinectViewer)(target));
            return;
            case 7:
            this.angle = ((LightBuzz.Vitruvius.Controls.KinectAngle)(target));
            return;
            case 8:
            this.Instructions = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

