using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using X.Viewer.SketchFlow.Controls;
using X.Viewer.SketchFlow.Controls.Stamps;

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

            var ct = cvMain.RenderTransform as CompositeTransform;
            _scaleX = ct.ScaleX;
            _scaleY = ct.ScaleY;


            SampleData();
        }

        private void SampleData() {
            var pg = new SketchPage() { Title = "Splash", Width = 360, Height = 640, Top = 100, Left = 100 };
            pg.Layers.Add(new PageLayer());
            var nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Title = "Shell", Width = 360, Height = 640, Top = 100, Left = 600 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Title = "Landing Page", Width = 360, Height = 640, Top = 100, Left = 1100 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Title = "Profile", Width = 360, Height = 640, Top = 100, Left = 1600 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);
            
            vm.Pages[1].Layers[0].XamlFragments.Add(@"<Rectangle HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Fill=""Black""></Rectangle>");
            vm.Pages[1].Layers[1].XamlFragments.Add(@"<Rectangle Fill=""#FF252525"" Height=""40"" Opacity=""0.8"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Bottom""/><StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Right"" VerticalAlignment=""Bottom"" Margin=""0,0,5,5"" ><StackPanel Orientation=""Vertical""><TextBlock Text=""4:49 PM"" Margin=""7,0,0,0"" FontSize=""12"" Foreground=""White"" /><TextBlock Text=""3/04/2016"" FontSize=""12"" Foreground=""White"" /></StackPanel><xuip:Path PathType=""More"" Rotation=""90"" Width=""20"" Height=""30"" Foreground=""White"" /></StackPanel>");
            vm.Pages[1].ExternalPC("Layers");
            
            vm.Pages[2].Layers[0].XamlFragments.Add(@"<Rectangle Fill=""Black"" />");
            vm.Pages[2].Layers[1].XamlFragments.Add(@"<Rectangle Fill=""#FF252525"" Height=""160"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Top""/>");
            vm.Pages[2].Layers[2].XamlFragments.Add(@"<TextBlock x:Name=""textBlock"" HorizontalAlignment=""Center"" TextWrapping=""Wrap"" Text=""Jose Fajardo"" VerticalAlignment=""Top"" Foreground=""White"" Margin=""0,120,0,0"" /><Ellipse Height=""85"" Margin=""0,15,0,0"" VerticalAlignment=""Top"" Width=""85"" HorizontalAlignment=""Center""><Ellipse.Fill><ImageBrush ImageSource=""http://art.ngfiles.com/images/378000/378294_kukatoo_minecraft-aqua-blue-avatar.png"" Stretch=""UniformToFill"" /></Ellipse.Fill></Ellipse>");
            vm.Pages[2].ExternalPC("Layers");

            vm.Pages[3].Layers[0].XamlFragments.Add(@"<Rectangle Fill=""Black"" />");
            vm.Pages[3].Layers[0].XamlFragments.Add(@"<Rectangle Fill=""#FF252525"" Height=""30"" Opacity=""0.4"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Top""/>");
            vm.Pages[3].Layers[1].XamlFragments.Add(@"<StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Left"" VerticalAlignment=""Top"" Margin=""10,5,0,0""><xuip:Path PathType=""Wifi2"" PathWidth=""30"" PathHeight=""15"" Width=""30"" Height=""20"" Foreground=""White"" Margin=""0,0,2,0"" /><xuip:Path PathType=""Wifi1"" Width=""20"" Height=""20"" Foreground=""White"" /></StackPanel>");
            vm.Pages[3].Layers[1].XamlFragments.Add(@"<StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Right"" VerticalAlignment=""Top"" Margin=""0,5,10,0""><xuip:Path PathType=""Sound"" Width=""20"" Height=""20"" Foreground=""White""  /><xuip:Path PathType=""BatteryLow"" Width=""35"" Height=""22"" Foreground=""White""  /></StackPanel>");
            vm.Pages[3].ExternalPC("Layers");

            
        }



        public void Dispose()
        {
            _renderer = null;
        }

        private void toolbar_PerformAction(object sender, EventArgs e)
        {
            var actionToPerform = (string)sender;

            if (actionToPerform == "SnapViewer") _renderer.SendMessageThru(null, new ContentViewEventArgs() { Type = actionToPerform });
            else if (actionToPerform == "AddPage640360") AddPage(360, 640);
            else if (actionToPerform == "AddPage18001200") AddPage(1800, 1200);
            else if (actionToPerform == "AddPage1400768") AddPage(1400, 768);
            else if (actionToPerform == "AddPage1600900") AddPage(1600, 900);
            else if (actionToPerform == "AddPage1200600") AddPage(1200, 600);
            else if (actionToPerform == "AddPageTiles")
            {
                var rect = AddPage(70, 70);
                var rect2 = AddPage(310, 310, (int)rect.X, (int)(rect.Y + rect.Z + 55));
                var rect3 = AddPage(310, 150, (int)rect2.X, (int)(rect2.Y + rect2.Z + 55));
                AddPage(150, 150, -1, (int)rect3.Y);
            }

            if (e is Controls.ToolbarEventArgs)
            {
                var ea = e as Controls.ToolbarEventArgs;
                
                if (ea.ActionType == "AddCircle")
                {
                  

                    var nc = new Controls.Stamps.Circle();
                    nc.Width = 85; nc.Height = 85;
                    nc.SetValue(Canvas.LeftProperty, Math.Abs(ea.StartPoint.X ));
                    nc.SetValue(Canvas.TopProperty, Math.Abs(ea.StartPoint.Y ));
                    nc.PerformAction += Stamp_PerformAction;
                    cvMainAdorner.Children.Add(nc);
                }
            }


        }

        bool IsMovingStamp = false;
        bool IsResizingStamp = false;
        UIElement _currentStamp;
        double _stampStartWidth = 0;
        double _stampStartHeight = 0;
        double _stampStartX = 0;
        double _stampStartY = 0;
        EventArgs _stampEA;
        private void Stamp_PerformAction(object sender, EventArgs e)
        {
            _currentStamp = (UIElement)sender;
            _stampStartX = (double)_currentStamp.GetValue(Canvas.LeftProperty);
            _stampStartY = (double)_currentStamp.GetValue(Canvas.TopProperty);

            if (e is Controls.Stamps.ResizeMoveEdgesEventArgs)
            {
                var te = e as Controls.Stamps.ResizeMoveEdgesEventArgs;
                
                _stampStartWidth = ((FrameworkElement)_currentStamp).Width;
                _stampStartHeight = ((FrameworkElement)_currentStamp).Height;
                
                _stampEA = e;
                
                IsResizingStamp = true;
            }
            else if (e is IStampEventArgs) {
                var te = e as IStampEventArgs;
                var stamp = ((IStamp)_currentStamp);

                if (te.ActionType == eActionTypes.CloseStamp )
                {
                    stamp.PerformAction -= Stamp_PerformAction;
                    cvMainAdorner.Children.Remove((UIElement)stamp);

                    return;
                }
                if (te.ActionType == eActionTypes.CreateFromStamp )
                {
                    //project stamp into the underlying page & new layer (in the future we should allow creating in an existing layer)

                    var found = VisualTreeHelper.FindElementsInHostCoordinates(new Windows.Foundation.Point(_stampStartX, _stampStartY), cvMain);
                    foreach (FrameworkElement el in found) {
                        
                        if (el.Parent is PageLayout)
                        {
                            var pl = el.Parent as PageLayout;
                            var plvm = pl.DataContext as SketchPage;

                            var npl = new PageLayer();
                            var gt = pl.TransformToVisual((UIElement)stamp);
                            var pt = gt.TransformPoint(new Windows.Foundation.Point(0, 0));
                            var stampo = _currentStamp as IStamp;
                            var str = stampo.GenerateXAML(_scaleX, _scaleY, pt.X, pt.Y);
                            npl.XamlFragments.Add(str);
                        
                            plvm.Layers.Add(npl);
                            plvm.ExternalPC("Layers");

                        }

                    }

                }
  
            }
        }

        private Vector4 AddPage(int width, int height, int left = -1, int top = -1)
        {
            var rect = new Vector4();
            rect.X = vm.Pages[vm.Pages.Count - 1].Left + vm.Pages[vm.Pages.Count - 1].Width + 150;
            rect.Y = 100;
            rect.W = width;
            rect.Z = height;

            if (left != -1) rect.X = left;
            if (top != -1) rect.Y = top;

            //var nextLeft = vm.Pages[vm.Pages.Count - 1].Left + vm.Pages[vm.Pages.Count - 1].Width + 150;

            var pg = new SketchPage() { Width = width, Height = height, Top = (int)rect.Y, Left = (int)rect.X };
            pg.Layers.Add(new PageLayer());
            var nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Canvas.LeftProperty, pg.Left);
            nc.SetValue(Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            return rect;
        }

        bool IsMovingPage = false;
        bool IsResizingPage = false;
        Controls.PageLayout _currentPageLayout;

        private void PageLayout_PerformAction(object sender, EventArgs e)
        {
            if(e is Controls.PageLayoutEventArgs)
            {
                var ea = e as Controls.PageLayoutEventArgs;
                _currentPageLayout = sender as Controls.PageLayout;
                if (ea.ActionType == "PageSelected")
                {
                    //var foundElements = VisualTreeHelper.FindElementsInHostCoordinates(ptEnd.Position, this);
                    //foreach (var el in foundElements)
                    //{
                    if (sender is PageLayout)
                    {
                        vm.SelectedPage = ((FrameworkElement)sender).DataContext as SketchPage;

                        //vm.SelectedPage.ExternalPC("Title");
                    }
                    //}
                }
                else if(ea.ActionType == "MovePageLayoutStarted") IsMovingPage = true;
                else if (ea.ActionType == "MovePageLayoutFinished") IsMovingPage = false;
                else if (ea.ActionType == "ResizePageLayoutStarted") IsResizingPage = true;
                else if (ea.ActionType == "ResizePageLayoutFinished") IsResizingPage = false;


                ptStartPt.X =  (double)_currentPageLayout.GetValue(Canvas.LeftProperty);
                ptStartPt.Y = (double)_currentPageLayout.GetValue(Canvas.TopProperty);

            }
      
        }

        double _scaleX = 0;
        double _scaleY = 0;
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
                if (ct.ScaleX >= 3.0) ct.ScaleX = 3.0;
                if (ct.ScaleY >= 3.0) ct.ScaleY = 3.0;
            }

            _scaleX = ct.ScaleX;
            _scaleY = ct.ScaleY;
        }





        PointerPoint ptStart;  //artboard moving
        Windows.Foundation.Point ptStartPt; //pagelayout moving
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
            IsMovingPage = false;
            IsResizingPage = false;
            IsResizingStamp = false;
            IsMovingStamp = false;

            _currentPageLayout = null;
            _currentStamp = null;

            ptDifXStart = ptDifX;
            ptDifYStart = ptDifY;

        }

        double ptDifX = 0;
        double ptDifY = 0;
        private void layoutRoot_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            var ptEnd = e.GetCurrentPoint(null);
            console1.Text = $"ex : {ptEnd.Position.X} { this.ActualWidth}  ey :  { ptEnd.Position.Y} {this.ActualHeight}    ";



            //var foundElements = VisualTreeHelper.FindElementsInHostCoordinates(ptEnd.Position, this);
            //foreach (var el in foundElements)
            //{
            //    if (el is PageLayout)
            //    {
            //        vm.SelectedPage = ((FrameworkElement)el).DataContext as SketchPage;

            //        //vm.SelectedPage.ExternalPC("Title");
            //    }
            //}



            if (!IsMouseDown) return;


            if (IsMovingPage)
            {
                double newX = 0; double newY = 0;

                var lvm = _currentPageLayout.DataContext as SketchPage;
                console3.Text = $"sx : {ptStart.Position.X}   sy :  { ptStart.Position.Y }     ";
                console2.Text = $"sx : {ptStartPt.X}   sy :  { ptStartPt.Y}     ";


                if (ptEnd.Position.X > ptStart.Position.X)
                {
                    newX = Math.Abs(ptEnd.Position.X - ptStart.Position.X); //diff from start
                    newX = (newX * (1 / _scaleX));  //take into account the scale factor
                    newX = ptStartPt.X + newX; // add to current canvas position

                    //console1.Text = $"right  : {ptStartPt.X + newX}   ey :  { 0 }     ";
                }
                else {
                    newX = Math.Abs(ptStart.Position.X - ptEnd.Position.X); //diff from start
                    newX = (newX * (1 / _scaleX)); //take into account the scale factor
                    newX = ptStartPt.X - newX; // add to current canvas position

                    //console1.Text = $"left : {ptStartPt.X - newX}   ey :  { 0 }     ";
                }

                _currentPageLayout.SetValue(Canvas.LeftProperty, newX);
                lvm.Left = (int)newX;
                lvm.ExternalPC("Left");

                if (ptEnd.Position.Y > ptStart.Position.Y)
                {
                    newY = Math.Abs(ptEnd.Position.Y - ptStart.Position.Y); //diff from start
                    newY = (newY * (1 / _scaleY));  //take into account the scale factor
                    newY = ptStartPt.Y + newY; // add to current canvas position

                    //console1.Text = $"right  : {ptStartPt.X + newY}   ey :  { 0 }     ";
                }
                else {
                    newY = Math.Abs(ptStart.Position.Y - ptEnd.Position.Y); //diff from start
                    newY = (newY * (1 / _scaleY)); //take into account the scale factor
                    newY = ptStartPt.Y - newY; // add to current canvas position

                    //console1.Text = $"left : {ptStartPt.Y - newY}   ey :  { 0 }     ";
                }

                _currentPageLayout.SetValue(Canvas.TopProperty, newY);
                lvm.Top = (int)newX;
                lvm.ExternalPC("Top");

                //_currentPageLayout.SetValue(Canvas.TopProperty, (ptStartPt.Y - (ptStart.Position.Y - ptEnd.Position.Y)));
                return;
            }
            else if (IsResizingPage)
            {

                return;
            }
            else if (IsResizingStamp) {
                console2.Text = $"sx : {ptStart.Position.X}   sy :  { ptStart.Position.Y }     ";
                console3.Text = $"deltax : {ptEnd.Position.X - ptStart.Position.X}   deltay :  { ptEnd.Position.Y - ptStart.Position.Y}     ";

                var stampe = _stampEA as Controls.Stamps.ResizeMoveEdgesEventArgs;

                if (stampe.ActionType == eActionTypes.MoveTopLeft)
                {
                    console2.Text = $"sx : {ptStart.Position.X}  { _stampStartX }  sy :  { ptStart.Position.Y } { _stampStartY }    ";

                    _currentStamp.SetValue(Canvas.LeftProperty, _stampStartX + (ptEnd.Position.X - ptStart.Position.X));
                    _currentStamp.SetValue(Canvas.TopProperty, _stampStartY + (ptEnd.Position.Y - ptStart.Position.Y));

                }
                else if (stampe.ActionType == eActionTypes.RotateBottomLeft)
                {
                    var ang = 180 / Math.PI * Math.Atan((ptStart.Position.Y - ptEnd.Position.Y) / (ptStart.Position.X - ptEnd.Position.X));
                    //var slope = (ptStart.Position.Y - ptEnd.Position.Y) / (ptStart.Position.X - ptEnd.Position.X);
                    //var new_slope = -1 / slope;
                    //var pt2x = (ptStart.Position.X + ptEnd.Position.X) / 2;
                    //var pt2y = (ptStart.Position.Y + ptEnd.Position.Y) / 2;
                    //var d_chord = Math.Sqrt( Math.Pow((ptStart.Position.X - ptEnd.Position.X),2) + Math.Pow((ptStart.Position.Y - ptEnd.Position.Y), 2));
                    //var d_perp = d_chord / (2 * Math.Tan(angle));

                    console2.Text = $"sx : {ptStart.Position.X}  { _stampStartX }  sy :  { ptStart.Position.Y } { _stampStartY }    angle : { ang }  ";

                }
                else if (stampe.ActionType == eActionTypes.ResizeBottomRight)
                {
                    ((FrameworkElement)_currentStamp).Width = _stampStartWidth + (ptEnd.Position.X - ptStart.Position.X);
                    ((FrameworkElement)_currentStamp).Height = _stampStartHeight + (ptEnd.Position.Y - ptStart.Position.Y);
                }
                else if (stampe.ActionType == eActionTypes.ResizeCenterRight)
                {
                    console2.Text = $"thickness : { ((ptEnd.Position.X - ptStart.Position.X) / 10) }  ";

                    var newThickness = Math.Abs(((ptEnd.Position.X - ptStart.Position.X) / 10));
                    //el
                    var circ = (Circle)_currentStamp;
                    var el = circ.FindName("el") as Shape;
                    el.StrokeThickness = newThickness;

                    //ptEnd.Position.X - ptStart.Position.X
                }


                return;
            }
            else
            {
                console1.Text = $"ex : {ptEnd.Position.X}   ey :  { ptEnd.Position.Y}     ";
                console2.Text = "";

                //moving artboard
                ptDifX = ptDifXStart + ptStart.Position.X - ptEnd.Position.X;
                ptDifY = ptDifYStart + ptStart.Position.Y - ptEnd.Position.Y;

                var ct = cvMain.RenderTransform as CompositeTransform;
                ct.TranslateX = -1 * ptDifX;
                ct.TranslateY = -1 * ptDifY;
            }






        }
    }


}
