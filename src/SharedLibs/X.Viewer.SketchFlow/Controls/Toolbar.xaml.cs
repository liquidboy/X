using System;
using Windows.UI;
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
            if (PerformAction != null) PerformAction("AddCircle", EventArgs.Empty);
        }
    }
}
