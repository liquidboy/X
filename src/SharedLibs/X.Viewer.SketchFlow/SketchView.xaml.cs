using System;
using Windows.UI.Xaml.Controls;

namespace X.Viewer.SketchFlow
{
    public sealed partial class SketchView : UserControl, IDisposable
    {
        IContentRenderer _renderer;

        public SketchView(IContentRenderer renderer)
        {
            _renderer = renderer;
            this.InitializeComponent();
        }

        public void Dispose()
        {
            _renderer = null;
        }

        private void toolbar_PerformAction(object sender, EventArgs e)
        {
            var actionToPerform = (string)sender;
            _renderer.SendMessageThru(null, new ContentViewEventArgs() { Type = actionToPerform });
        }
    }
}
