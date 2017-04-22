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
using X.Services.Data;

namespace X.Viewer.SketchFlow.Controls.Pickers
{
    public sealed partial class LoadSketchPicker : UserControl
    {
        public event EventHandler PerformAction;

        public LoadSketchPicker()
        {
            this.InitializeComponent();
        }

        public void LoadSketchs(IList<SketchDataModel> sketches)
        {
            lbSketches.ItemsSource = sketches;
        }
        public void ClearSketchs()
        {
            txtLoadName.DataContext = null;
            lbSketches.ItemsSource = null;
        }

        private void butDoLoad_Click(object sender, RoutedEventArgs e)
        {

            if (txtLoadName.Value == "deleteall")
            {
                PerformAction?.Invoke("DeleteAllSketchs", new ToolbarEventArgs() { ActionType = "DeleteAllSketchs" });
                return;
            }

            if (txtLoadName.Value == "loadsample")
            {
                PerformAction?.Invoke("LoadSampleSketch", new ToolbarEventArgs() { ActionType = "LoadSampleSketch" });
                return;
            }

            if (txtLoadName.DataContext is SketchDataModel)
            {
                var dc = txtLoadName.DataContext as SketchDataModel;
                PerformAction?.Invoke("LoadSketch", new ToolbarEventArgs() { ActionType = "LoadSketch", Data = dc.Id.ToString() });
                txtLoadName.DataContext = null;
            }
            
        }

        private void butDoDelete_Click(object sender, RoutedEventArgs e)
        {
            if (txtLoadName.DataContext is SketchDataModel)
            {
                var dc = txtLoadName.DataContext as SketchDataModel;
                PerformAction?.Invoke("DeleteSketch", new ToolbarEventArgs() { ActionType = "DeleteSketch", Data = dc.Id.ToString() });
                txtLoadName.DataContext = null;
            }
        }

        private void lbSketches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var found = e.AddedItems[0];
                txtLoadName.DataContext = found;
            }
        }
    }
}
