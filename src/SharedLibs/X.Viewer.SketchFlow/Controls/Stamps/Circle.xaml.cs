using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.Viewer.SketchFlow.Controls.Stamps
{


    public sealed partial class Circle : UserControl
    {
        public event EventHandler PerformAction;

        public Circle()
        {
            this.InitializeComponent();

            this.Unloaded += Circle_Unloaded;
        }

        private void Circle_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void edges_PerformAction(object sender, EventArgs e)
        {
            var ea = e as ResizeMoveEdgesEventArgs;

            if (ea.ActionType == eActionTypes.ToolbarTopRight) {
                if(spToolbar.Visibility == Visibility.Visible) spToolbar.Visibility = Visibility.Collapsed;
                else spToolbar.Visibility = Visibility.Visible;
            }

            PerformAction?.Invoke(this, e);
        }

        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new CircleEventArgs() { ActionType = eActionTypes.CloseStamp } );
        }

        private void butStamp_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new CircleEventArgs() { ActionType = eActionTypes.CreateFromStamp });
        }
    }

    public class CircleEventArgs : EventArgs
    {
        public eActionTypes ActionType;
    }
}
