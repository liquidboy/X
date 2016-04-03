using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            }
            else {
                spToolbar.Visibility = Visibility.Visible;
            }
        }
    }
}
