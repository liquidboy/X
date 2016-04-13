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
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.MoveTopLeft } );
        }

        private void butTopRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.ToolbarTopRight });
        }

        private void butBottomLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.RotateBottomLeft });
        }

        private void butBottomRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.ResizeBottomRight });
        }

        private void butCenterRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.ResizeCenterRight });
        }
    }

    public class ResizeMoveEdgesEventArgs : EventArgs
    {
        public eActionTypes ActionType;
        public Windows.Foundation.Point StartPoint;
    }
}
