using System;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace X.Viewer.SketchFlow.Controls
{
    public sealed partial class Toolbar : UserControl
    {
        public event EventHandler PerformAction;

        public Toolbar()
        {
            this.InitializeComponent();
        }

        private void butOne_Click(object sender, RoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction("SnapViewer", EventArgs.Empty);
        }

        private void butToolbar_Click(object sender, RoutedEventArgs e)
        {
            if (spToolbar.Visibility == Visibility.Visible) {
                spToolbar.Visibility = Visibility.Collapsed;
                spPageSizes.Visibility = Visibility.Collapsed;
                spShapes.Visibility = Visibility.Collapsed;
                butToolbar.Background = new SolidColorBrush(Colors.LightGray);
                butAddPage.Background = new SolidColorBrush(Colors.LightGray);
                butShape.Background = new SolidColorBrush(Colors.LightGray);
            }
            else {
                spToolbar.Visibility = Visibility.Visible;
                butToolbar.Background = new SolidColorBrush(Colors.Gray);


            }
        }

        private void butAddPage_Click(object sender, RoutedEventArgs e)
        {
            //other buts disabled
            spShapes.Visibility = Visibility.Collapsed;
            butShape.Background = new SolidColorBrush(Colors.LightGray);


            if (spPageSizes.Visibility == Visibility.Visible)
            {
                spPageSizes.Visibility = Visibility.Collapsed;
                butAddPage.Background = new SolidColorBrush(Colors.LightGray);
            }
            else {
                spPageSizes.Visibility = Visibility.Visible;
                butAddPage.Background = new SolidColorBrush(Colors.Gray);
            }
            
        }

        private void butShape_Click(object sender, RoutedEventArgs e)
        {
            //other buts disabled
            spPageSizes.Visibility = Visibility.Collapsed;
            butAddPage.Background = new SolidColorBrush(Colors.LightGray);

            if (spShapes.Visibility == Visibility.Visible)
            {
                spShapes.Visibility = Visibility.Collapsed;
                butShape.Background = new SolidColorBrush(Colors.LightGray);
            }
            else {
                spShapes.Visibility = Visibility.Visible;
                butShape.Background = new SolidColorBrush(Colors.Gray);
            }
        }



        private void but640360_Click(object sender, RoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction("AddPage640360", EventArgs.Empty);
        }

        private void but200200_Click(object sender, RoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction("AddPageTiles", EventArgs.Empty);
        }

        private void but1200600_Click(object sender, RoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction("AddPage1200600", EventArgs.Empty);
        }

        private void but1600900_Click(object sender, RoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction("AddPage1600900", EventArgs.Empty);
        }

        private void but1400768_Click(object sender, RoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction("AddPage1400768", EventArgs.Empty);
        }

        private void but18001200_Click(object sender, RoutedEventArgs e)
        {
            if (PerformAction != null) PerformAction("AddPage18001200", EventArgs.Empty);
        }
        
        private void butCircle_Click(object sender, RoutedEventArgs e)
        {
            var pt = getPoint((UIElement)sender);
            if (PerformAction != null) PerformAction(null, new ToolbarEventArgs() { ActionType = "AddStamp", StartPoint = pt, StampType  = typeof(Stamps.Circle) });
        }

        private void butRectangle_Click(object sender, RoutedEventArgs e)
        {
            var pt = getPoint((UIElement)sender);
            if (PerformAction != null) PerformAction(null, new ToolbarEventArgs() { ActionType = "AddStamp", StartPoint = pt, StampType = typeof(Stamps.Rectangle) });
        }

        private Windows.Foundation.Point getPoint(UIElement el) {
            var sv = (FrameworkElement)((FrameworkElement)this.Parent).Parent; // get canvas in the parent , tight relation between this control and the parent now
            //var cv = (UIElement)sv.FindName("cvMain"); // get canvas in parent
            var cv = (UIElement)sv.FindName("cvMainAdorner"); // get canvas in parent
            var tp = cv.TransformToVisual(el); // get transform of this button in relation to the canvas(cv)
            var pt = tp.TransformPoint(new Windows.Foundation.Point(0, 0)); // get position from the transform(tp)

            //just tweak the position so its not under toolbar
            pt.X = pt.X + 20;
            pt.Y = pt.Y - 90;

            return pt;
        }

        private void butStar_Click(object sender, RoutedEventArgs e)
        {
            var pt = getPoint((UIElement)sender);
            if (PerformAction != null) PerformAction(null, new ToolbarEventArgs() { ActionType = "AddStamp", StartPoint = pt, StampType = typeof(Windows.UI.Xaml.Shapes.Path), Data= "M383.519,0L501.986,252.479 767,292.964 575.223,489.496 620.525,767 383.519,635.979 146.475,767 191.74,489.496 0,292.964 265.014,252.479z" });
        }
    }


    public class ToolbarEventArgs : EventArgs
    {
        public string ActionType;
        public Type StampType;
        public Windows.Foundation.Point StartPoint;
        public string Data;
    }
}
