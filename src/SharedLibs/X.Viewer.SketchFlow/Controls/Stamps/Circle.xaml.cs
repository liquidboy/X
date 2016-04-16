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


    public sealed partial class Circle : UserControl, IStamp
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

            if (ea.ActionType == eActionTypes.ToolbarTopRight)
            {
                if (spToolbar.Visibility == Visibility.Visible) spToolbar.Visibility = Visibility.Collapsed;
                else spToolbar.Visibility = Visibility.Visible;
            }
            else if (ea.ActionType == eActionTypes.CenterLeft) {
                if (colorPicker.Visibility == Visibility.Visible) colorPicker.Visibility = Visibility.Collapsed;
                else colorPicker.Visibility = Visibility.Visible;
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

        public string GenerateXAML(double scaleX, double scaleY, double left, double top)
        {
            var rotationAngle =  ((CompositeTransform)el.RenderTransform).Rotation;
            var leftToUse = left;
            var topToUse = top;
            var rotationStr = $"<Ellipse.RenderTransform><CompositeTransform Rotation=\"{ rotationAngle }\" /></Ellipse.RenderTransform>";
            if (rotationAngle == 0) rotationStr = "";

            var newStroke = el.StrokeThickness * (1 / scaleX);

            return $"<Ellipse HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" Height=\"{ (this.Height * (1 / scaleY)) }\" Width=\"{ (this.Width * (1 / scaleX)) }\"  StrokeThickness=\"{ newStroke }\" Stroke=\"DarkOrange\" Canvas.Left=\"{ leftToUse }\" Canvas.Top=\"{ topToUse }\" RenderTransformOrigin=\"0.5,0.5\">{ rotationStr }</Ellipse>";
        }

        public void PopulateFromUIElement(UIElement element)
        {
            throw new NotImplementedException();
        }

        public void UpdateRotation(double angle)
        {
            ((CompositeTransform)el.RenderTransform).Rotation = angle;
        }
    }

    public class CircleEventArgs : EventArgs, IStampEventArgs
    {
        public eActionTypes ActionType { get; set; }
        
    }
}
