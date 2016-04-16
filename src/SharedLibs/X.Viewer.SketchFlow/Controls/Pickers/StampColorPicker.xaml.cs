using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;


namespace X.Viewer.SketchFlow.Controls.Pickers
{
    public sealed partial class StampColorPicker : UserControl
    {
        public event EventHandler ColorChanged;

        public List<String> ColorTypes { get; set; } = new List<string>();

        public StampColorPicker()
        {
            this.InitializeComponent();
        }

        private void cs_ColorChanged(object sender, EventArgs e)
        {
            ColorChanged?.Invoke(sender, new ColorPickerEventArgs() { ColorType = (string)rcb.Value2  } );
        }
    }

    public class ColorPickerEventArgs : EventArgs {
        public string ColorType;
    }
}
