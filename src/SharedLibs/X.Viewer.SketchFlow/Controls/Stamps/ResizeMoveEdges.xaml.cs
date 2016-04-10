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
    public partial class ResizeMoveEdges : UserControl
    {
        public event EventHandler PerformAction;


        public ResizeMoveEdges()
        {
            this.InitializeComponent();
        }

        private void butTopLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = "MoveTopLeft" } );
        }

        private void butTopRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = "ToolbarTopRight" });
        }

        private void butBottomLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = "RotateBottomLeft" });
        }

        private void butBottomRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = "ResizeBottomRight" });
        }
    }

    public class ResizeMoveEdgesEventArgs : EventArgs
    {
        public string ActionType;
        public Windows.Foundation.Point StartPoint;
    }
}
