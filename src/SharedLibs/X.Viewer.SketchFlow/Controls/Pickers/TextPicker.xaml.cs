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
    public sealed partial class TextPicker : UserControl
    {
        public event EventHandler TextChanged;


        public TextPicker()
        {
            this.InitializeComponent();
        }

        private void tbMain_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, new TextPickerEventArgs() { Text = (string)tbMain.Text });
        }
    }

    public class TextPickerEventArgs : EventArgs
    {
        public string Text;
    }
}
