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


namespace X.Viewer.SketchFlow.Controls
{
    public sealed partial class PageLayout : UserControl
    {
        public PageLayout()
        {
            this.InitializeComponent();
        }

        private void pgLayer_LayerChanged(object sender, EventArgs e)
        {
            var page = this.DataContext as SketchPage;
            page.ExternalPC("Layers");
        }
    }
}
