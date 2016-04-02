using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace X.Viewer.SketchFlow
{
    public sealed partial class SketchView : UserControl, IDisposable
    {
        IContentRenderer _renderer;
        Sketch vm;

        public SketchView(IContentRenderer renderer)
        {
            

            this.InitializeComponent();

            vm = new Sketch();
            this.DataContext = vm;
            
            _renderer = renderer;

            InitDemo();
        }

        private void InitDemo() {
            var pg = new SketchPage() { Width = 360, Height = 640, Top = 100, Left = 100 };
            pg.Layers.Add(new PageLayer());
            var nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Width = 360, Height = 640, Top = 100, Left = 560 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Width = 360, Height = 640, Top = 100, Left = 1020 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);
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
