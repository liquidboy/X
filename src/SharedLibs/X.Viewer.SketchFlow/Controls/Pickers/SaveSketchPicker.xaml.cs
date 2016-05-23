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

namespace X.Viewer.SketchFlow.Controls.Pickers
{
    public sealed partial class SaveSketchPicker : UserControl
    {
        public event EventHandler PerformAction;
        
        public SaveSketchPicker()
        {
            this.InitializeComponent();
        }

        private void butDoSave_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke("SaveSketch", new ToolbarEventArgs() { ActionType = "SaveSketch", Data = txtSaveName.Value });
        }
    }
}
