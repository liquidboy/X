using System;
using Windows.UI.Xaml.Controls;


namespace X.Viewer.SketchFlow.Controls.Pickers
{
    public sealed partial class StampColorPicker : UserControl
    {
        public event EventHandler ColorChanged;

        public StampColorPicker()
        {
            this.InitializeComponent();
        }

        private void cs_ColorChanged(object sender, EventArgs e)
        {
            ColorChanged?.Invoke(sender, e);
        }
    }
}
