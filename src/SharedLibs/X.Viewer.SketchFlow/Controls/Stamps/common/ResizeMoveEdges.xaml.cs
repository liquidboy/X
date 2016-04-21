using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

        bool _isLocked = false;
        public bool IsLocked {
            get {
                return _isLocked;
            }
            set {
                _isLocked = value;

                if (_isLocked) {
                    //bottomTouchArea.Fill = new SolidColorBrush(Colors.Transparent);
                    bottomTouchArea.Visibility = Visibility.Collapsed;
                    butBottomLeft.Visibility = Visibility.Collapsed;
                    butBottomRight.Visibility = Visibility.Collapsed;
                    butTopLeft.Visibility = Visibility.Collapsed;
                    butCenterLeft.Visibility = Visibility.Collapsed;
                    butCenterRight.Visibility = Visibility.Collapsed;
                }
                else {
                    //bottomTouchArea.Fill = new SolidColorBrush(new Color() { A=0, B=255,G=255,R=255  });
                    bottomTouchArea.Visibility = Visibility.Visible;
                    butBottomLeft.Visibility = Visibility.Visible;
                    butBottomRight.Visibility = Visibility.Visible;
                    butTopLeft.Visibility = Visibility.Visible;
                    butCenterLeft.Visibility = Visibility.Visible;
                    butCenterRight.Visibility = Visibility.Visible;
                }
            }
        }

        public ResizeMoveEdges()
        {
            this.InitializeComponent();
        }

        private void butTopLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.MoveTopLeft } );
        }

        private void butTopRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.ToolbarTopRight });
        }

        private void butBottomLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.RotateBottomLeft });
        }

        private void butBottomRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.ResizeBottomRight });
        }

        private void butCenterRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.ResizeCenterRight });
        }

        private void butCenterLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ResizeMoveEdgesEventArgs() { ActionType = eActionTypes.CenterLeft });
        }

        private void General_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ((FrameworkElement)sender).Opacity = 1;
        }

        private void General_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ((FrameworkElement)sender).Opacity = 0.5;
        }

        private void layoutRoot_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            butTopLeft.Opacity = 0.5;
            butBottomLeft.Opacity = 0.5;
            butBottomRight.Opacity = 0.5;
            butTopRight.Opacity = 0.5;
            butCenterLeft.Opacity = 0.5;
            butCenterRight.Opacity = 0.5;
        }

        private void layoutRoot_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            butTopLeft.Opacity = 0.2;
            butBottomLeft.Opacity = 0.2;
            butBottomRight.Opacity = 0.2;
            butTopRight.Opacity = 0.2;
            butCenterLeft.Opacity = 0.2;
            butCenterRight.Opacity = 0.2;
        }
    }

    public class ResizeMoveEdgesEventArgs : EventArgs
    {
        public eActionTypes ActionType;
        public Windows.Foundation.Point StartPoint;
    }
}
