using System;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace X.Viewer.SketchFlow
{
    public sealed partial class SketchView : UserControl, IDisposable
    {
        IContentRenderer _renderer;
        Sketch vm;

        bool IsMouseDown = false;

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
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Width = 360, Height = 640, Top = 100, Left = 1480 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);
            
            vm.Pages[1].Layers[0].XamlFragments.Add(@"<Rectangle HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Fill=""Black""></Rectangle>");
            vm.Pages[1].ExternalPC("Layers");
            
            vm.Pages[2].Layers[0].XamlFragments.Add(@"<Rectangle Fill=""Black"" />");
            vm.Pages[2].Layers[1].XamlFragments.Add(@"<Rectangle Fill=""#FF252525"" Height=""160"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Top""/>");
            vm.Pages[2].Layers[2].XamlFragments.Add(@"<TextBlock x:Name=""textBlock"" HorizontalAlignment=""Center"" TextWrapping=""Wrap"" Text=""Jose Fajardo"" VerticalAlignment=""Top"" Foreground=""White"" Margin=""0,120,0,0"" /><Ellipse Height=""85"" Margin=""0,15,0,0"" VerticalAlignment=""Top"" Width=""85"" HorizontalAlignment=""Center""><Ellipse.Fill><ImageBrush ImageSource=""http://art.ngfiles.com/images/378000/378294_kukatoo_minecraft-aqua-blue-avatar.png"" Stretch=""UniformToFill"" /></Ellipse.Fill></Ellipse>");
            vm.Pages[2].ExternalPC("Layers");

            vm.Pages[3].Layers[0].XamlFragments.Add(@"<Rectangle Fill=""Black"" />");
            vm.Pages[3].Layers[0].XamlFragments.Add(@"<Rectangle Fill=""#FF252525"" Height=""160"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Top""/>");
            vm.Pages[3].Layers[1].XamlFragments.Add(@"<StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Left"" VerticalAlignment=""Top"" Margin=""10,5,0,0""><xuip:Path PathType=""Wifi2"" PathWidth=""30"" PathHeight=""15"" Width=""30"" Height=""20"" Foreground=""White"" Margin=""0,0,2,0"" /><xuip:Path PathType=""Wifi1"" Width=""20"" Height=""20"" Foreground=""White"" /></StackPanel>");
            vm.Pages[3].Layers[1].XamlFragments.Add(@"<StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Right"" VerticalAlignment=""Top"" Margin=""0,5,10,0""><xuip:Path PathType=""BatteryLow"" Width=""35"" Height=""22"" Foreground=""White""  /></StackPanel>");
            vm.Pages[3].ExternalPC("Layers");


            
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

        private void layoutRoot_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //var delta = 25;
            var change = e.GetCurrentPoint(null).Properties.MouseWheelDelta;
            var ct = cvMain.RenderTransform as CompositeTransform;
            
            if (change > 0)
            {
                ct.ScaleX += -0.05;
                ct.ScaleY += -0.05;
                if (ct.ScaleX <= 0.3) ct.ScaleX = 0.3;
                if (ct.ScaleY <= 0.3) ct.ScaleY = 0.3;
            }
            else if (change < 0)
            {
                ct.ScaleX += 0.05;
                ct.ScaleY += 0.05;
                if (ct.ScaleX >= 2.0) ct.ScaleX = 2.0;
                if (ct.ScaleY >= 2.0) ct.ScaleY = 2.0;
            }
        }





        PointerPoint ptStart;
        double ptDifXStart = 0;
        double ptDifYStart = 0;
        private void layoutRoot_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsMouseDown = true;
            ptStart = e.GetCurrentPoint(null);
            
        }

        private void layoutRoot_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsMouseDown = false;

            ptDifXStart = ptDifX;
            ptDifYStart = ptDifY;

        }

        double ptDifX = 0;
        double ptDifY = 0;
        private void layoutRoot_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!IsMouseDown) return;

            var ptEnd = e.GetCurrentPoint(null);

            ptDifX = ptDifXStart + ptStart.Position.X - ptEnd.Position.X;
            ptDifY = ptDifYStart + ptStart.Position.Y - ptEnd.Position.Y;

            var ct = cvMain.RenderTransform as CompositeTransform;
            ct.TranslateX = -1 * ptDifX;
            ct.TranslateY = -1 * ptDifY;
        }
    }


}
