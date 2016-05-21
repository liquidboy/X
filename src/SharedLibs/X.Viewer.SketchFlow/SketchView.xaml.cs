using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Input;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using X.Viewer.SketchFlow.Controls;
using X.Viewer.SketchFlow.Controls.Stamps;
using Windows.UI;
using Windows.Foundation;
using X.Services.Data;
using X.UI.ZoomCanvas;

namespace X.Viewer.SketchFlow
{
    public sealed partial class SketchView : UserControl, IContentView
    {
        public event EventHandler<ContentViewEventArgs> SendMessage;

        Sketch vm;
        UI.ZoomCanvas.Canvas cvMainContainer;
        Windows.UI.Xaml.Controls.Canvas cvMain;
        bool IsMouseDown = false;

        public SketchView()
        {
            this.InitializeComponent();

            this.Loaded += SketchView_Loaded;

        }


        private void SketchView_Loaded(object sender, RoutedEventArgs e)
        {
            //var ct = cvMain.RenderTransform as CompositeTransform;
            //cvMainContainer.Scale = ct.ScaleX;

            vm = new Sketch();
            //SampleData();
            //this.DataContext = vm;

            //foreach (var pg in vm.Pages) {
            //    StorageService.Instance.AzureStorage.Save<ISketchPageDataModel>(pg);
            //}

        }
        public void Load()
        {
            //changed to creating zcanvas by code rather than xaml, it was causing a memory leak
            cvMainContainer = new UI.ZoomCanvas.Canvas() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Background = new SolidColorBrush(Colors.White) , RenderTransformOrigin = new Point(0,0) };
            cvMain = new Windows.UI.Xaml.Controls.Canvas() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch, Background = new SolidColorBrush(Colors.White), RenderTransformOrigin = new Point(0.5d, 0.5d) };
            cvMain.RenderTransform = new CompositeTransform() { ScaleX = 1, ScaleY = 1, TranslateX = 0, TranslateY = 0 };
            cvMainContainer.Content = cvMain;
            layoutRoot.Children.Insert(0, cvMainContainer);

            var ct = cvMain.RenderTransform as CompositeTransform;
            cvMainContainer.Scale = ct.ScaleX;
        }

        public void Unload()
        {
            if (vm != null)
            {

                if (cvMain.Children.Count > 0)
                {
                    foreach (var nc in cvMain.Children)
                    {
                        if (nc is PageLayout)
                        {
                            var ncpl = nc as PageLayout;
                            ncpl.PerformAction -= PageLayout_PerformAction;
                            ncpl.DataContext = null;
                        }
                    }
                    cvMain.Children.Clear();
                }

                if (vm.Pages != null)
                {
                    foreach (var pg in vm.Pages)
                    {
                        pg.Layers.Clear();
                    }
                    vm.Pages.Clear();
                }
                if (cvMain.Children.Count > 0)
                {
                    //foreach (var nc in cvMain.Children) {
                    //    if (nc is PageLayout) {
                    //        var ncpl = nc as PageLayout;
                    //        ncpl.PerformAction -= PageLayout_PerformAction;
                    //    }
                    //}
                    cvMain.Children.Clear();
                }

                vm = null;
            }

            cvMainContainer.Content = null;
            cvMain.RenderTransform = null;
            cvMain = null;
            


            layoutRoot.Children.Remove(cvMainContainer);
            //cvMainContainer.Content = null;
            cvMainContainer = null;
        }
        
        private void LoadSketch(int sketchId)
        {
            var foundS = StorageService.Instance.Storage.RetrieveById<SketchDataModel>(sketchId);
            if (foundS != null && foundS.Count() > 0)
            {
                var fs = foundS.First();
                var foundSP = StorageService.Instance.Storage.RetrieveByField<SketchPageDataModel>("SketchId", fs.Id.ToString());
                if (foundSP != null && foundSP.Count() > 0)
                {
                    foreach (var fsp in foundSP)
                    {
                        var pg = new SketchPage() { Title = fsp.Title, Width = (int)fsp.Width, Height = (int)fsp.Height, Top = (int)fsp.Top, Left = (int)fsp.Left };

                        var foundSPL = StorageService.Instance.Storage.RetrieveByField<SketchPageLayerDataModel>("SketchPageId", fsp.Id.ToString());
                        if (foundSPL != null && foundSP.Count() > 0)
                        {
                            foreach (var fspl in foundSPL)
                            {
                                var pl = new PageLayer();

                                var foundSPLXF = StorageService.Instance.Storage.RetrieveByField<SketchPageLayerXamlFragmentDataModel>("SketchPageLayerId", fspl.Id.ToString());
                                if (foundSPLXF != null && foundSPLXF.Count() > 0)
                                {
                                    foreach (var fsplxf in foundSPLXF)
                                    {
                                        var xf = new XamlFragment() { Uid = fsplxf.Uid, Xaml = fsplxf.Xaml };

                                        pl.XamlFragments.Add(xf);
                                    }
                                }
                                pg.Layers.Add(pl);
                                
                            }

                        }


                        var nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
                        nc.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, pg.Left);
                        nc.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, pg.Top);
                        nc.PerformAction += PageLayout_PerformAction;
                        cvMain.Children.Add(nc);

                        vm.Pages.Add(pg);
                    }
                }
                
            }
            //else
            //{
            //    StorageService.Instance.Storage.Truncate<SketchDataModel>();
            //    StorageService.Instance.Storage.Truncate<SketchPageDataModel>();
            //    StorageService.Instance.Storage.Truncate<SketchPageLayerDataModel>();
            //    StorageService.Instance.Storage.Truncate<SketchPageLayerXamlFragmentDataModel>();
            //    SampleData();
            //}

        }

        private void SampleData() {
            var pg = new SketchPage() { Title = "Splash", Width = 360, Height = 640, Top = 100, Left = 100 };
            pg.Layers.Add(new PageLayer());
            var nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, pg.Left);
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Title = "Shell", Width = 360, Height = 640, Top = 100, Left = 600 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, pg.Left);
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Title = "Landing Page", Width = 360, Height = 640, Top = 100, Left = 1100 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, pg.Left);
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            pg = new SketchPage() { Title = "Profile", Width = 360, Height = 640, Top = 100, Left = 1600 };
            pg.Layers.Add(new PageLayer());
            pg.Layers.Add(new PageLayer());
            nc = new Controls.PageLayout() { DataContext = pg, Width = pg.Width, Height = pg.Height };
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, pg.Left);
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            vm.Pages[1].Layers[0].XamlFragments.Add(new XamlFragment() { Uid = "xx1", Xaml = @"<Rectangle HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Fill=""Black""></Rectangle>" });
            vm.Pages[1].Layers[1].XamlFragments.Add(new XamlFragment() { Uid = "xx2", Xaml = @"<Rectangle Fill=""#FF252525"" Height=""40"" Opacity=""0.8"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Bottom""/><StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Right"" VerticalAlignment=""Bottom"" Margin=""0,0,5,5"" ><StackPanel Orientation=""Vertical""><TextBlock Text=""4:49 PM"" Margin=""7,0,0,0"" FontSize=""12"" Foreground=""White"" /><TextBlock Text=""3/04/2016"" FontSize=""12"" Foreground=""White"" /></StackPanel><xuip:Path PathType=""More"" Rotation=""90"" Width=""20"" Height=""30"" Foreground=""White"" /></StackPanel>" });
            vm.Pages[1].ExternalPC("Layers");

            vm.Pages[2].Layers[0].XamlFragments.Add(new XamlFragment() { Uid = "xx3", Xaml = @"<Rectangle Fill=""Black"" />" });
            vm.Pages[2].Layers[1].XamlFragments.Add(new XamlFragment() { Uid = "xx4", Xaml = @"<Rectangle Fill=""#FF252525"" Height=""160"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Top""/>" });
            vm.Pages[2].Layers[2].XamlFragments.Add(new XamlFragment() { Uid = "xx5", Xaml = @"<TextBlock x:Name=""textBlock"" HorizontalAlignment=""Center"" TextWrapping=""Wrap"" Text=""Jose Fajardo"" VerticalAlignment=""Top"" Foreground=""White"" Margin=""0,120,0,0"" /><Ellipse Height=""85"" Margin=""0,15,0,0"" VerticalAlignment=""Top"" Width=""85"" HorizontalAlignment=""Center""><Ellipse.Fill><ImageBrush ImageSource=""http://art.ngfiles.com/images/378000/378294_kukatoo_minecraft-aqua-blue-avatar.png"" Stretch=""UniformToFill"" /></Ellipse.Fill></Ellipse>" });
            vm.Pages[2].ExternalPC("Layers");

            vm.Pages[3].Layers[0].XamlFragments.Add(new XamlFragment() { Uid = "xx6", Xaml = @"<Rectangle Fill=""Black"" />" });
            vm.Pages[3].Layers[0].XamlFragments.Add(new XamlFragment() { Uid = "xx7", Xaml = @"<Rectangle Fill=""#FF252525"" Height=""30"" Opacity=""0.4"" HorizontalAlignment=""Stretch"" VerticalAlignment=""Top""/>" });
            vm.Pages[3].Layers[1].XamlFragments.Add(new XamlFragment() { Uid = "xx8", Xaml = @"<StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Left"" VerticalAlignment=""Top"" Margin=""10,5,0,0""><xuip:Path PathType=""Wifi2"" PathWidth=""30"" PathHeight=""15"" Width=""30"" Height=""20"" Foreground=""White"" Margin=""0,0,2,0"" /><xuip:Path PathType=""Wifi1"" Width=""20"" Height=""20"" Foreground=""White"" /></StackPanel>" });
            vm.Pages[3].Layers[1].XamlFragments.Add(new XamlFragment() { Uid = "xx9", Xaml = @"<StackPanel Orientation=""Horizontal"" HorizontalAlignment=""Right"" VerticalAlignment=""Top"" Margin=""0,5,10,0""><xuip:Path PathType=""Sound"" Width=""20"" Height=""20"" Foreground=""White""  /><xuip:Path PathType=""BatteryLow"" Width=""35"" Height=""22"" Foreground=""White""  /></StackPanel>" });
            vm.Pages[3].ExternalPC("Layers");


        }

        private void toolbar_PerformAction(object sender, EventArgs e)
        {
            var actionToPerform = (string)sender;

            if (actionToPerform == "SnapViewer") SendMessage?.Invoke(null, new ContentViewEventArgs() { Type = actionToPerform });
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
            else if (actionToPerform == "SaveSketch")
            {
                var tbea = e as ToolbarEventArgs;
                
                var ni = new SketchDataModel() { Title = tbea.Data };
                StorageService.Instance.Storage.Insert(ni);

                foreach (var pg in vm.Pages) {
                    var nip = new SketchPageDataModel() { SketchId = ni.Id, Title = pg.Title, Left = pg.Left, Top = pg.Top, Width = pg.Width, Height = pg.Height };
                    StorageService.Instance.Storage.Insert(nip);

                    foreach (var pgl in pg.Layers) {
                        var npgl = new SketchPageLayerDataModel() { SketchPageId = nip.Id };
                        StorageService.Instance.Storage.Insert(npgl);

                        foreach (var xf in pgl.XamlFragments) {
                            var nxf = new SketchPageLayerXamlFragmentDataModel() { SketchPageLayerId = npgl.Id, Uid = xf.Uid, Xaml = xf.Xaml };
                            StorageService.Instance.Storage.Insert(nxf);

                        }
                    }

                }
            }
            else if (actionToPerform == "GetAllSketchs")
            {

            }
            else if (actionToPerform == "DeleteAllSketchs")
            {
                StorageService.Instance.Storage.Truncate<SketchDataModel>();
                StorageService.Instance.Storage.Truncate<SketchPageDataModel>();
                StorageService.Instance.Storage.Truncate<SketchPageLayerDataModel>();
                StorageService.Instance.Storage.Truncate<SketchPageLayerXamlFragmentDataModel>();
            }
            else if (actionToPerform == "LoadSketch")
            {
                var tbea = e as ToolbarEventArgs;
                var idToLoad = int.Parse( tbea.Data);
                LoadSketch(idToLoad);    
            }

            if (e is Controls.ToolbarEventArgs)
            {
                var ea = e as Controls.ToolbarEventArgs;
                if (ea.ActionType == "AddStamp" ||  ea.ActionType == "AddImage" || ea.ActionType == "AddText")
                    CreateStamp(ea.StampType, ea.StartPoint.X, ea.StartPoint.Y, 85, 85, data: ea.Data);
            }


        }





        private void CreateStamp(Type type, double x, double y, double w, double h, UIElement template = null, string data = "") {

            FrameworkElement nc = null;
            if (string.IsNullOrEmpty(data))  
                nc = (FrameworkElement)Activator.CreateInstance(type, new object[] { });  //Controls.Stamps.Circle();
            else { 
                nc = new Controls.Stamps.Shape();
                ((Controls.Stamps.Shape)nc).StampData = data;
                ((Controls.Stamps.Shape)nc).StampType = type;
            }

            nc.Width = w; nc.Height = h;
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, Math.Abs(x));
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, Math.Abs(y));
            if (template != null) ((IStamp)nc).GenerateFromXAML(template);

            ((IStamp)nc).PerformAction += Stamp_PerformAction;
            cvMainAdorner.Children.Add(nc);
        }


        bool IsMovingStamp = false;
        bool IsResizingStamp = false;
        UIElement _currentStamp;
        double _stampStartWidth = 0;
        double _stampStartHeight = 0;
        double _stampStartX = 0;
        double _stampStartY = 0;
        double _stampStartRotation = 0;
        EventArgs _stampEA;
        private void Stamp_PerformAction(object sender, EventArgs e)
        {
            _currentStamp = (UIElement)sender;
            _stampStartX = (double)_currentStamp.GetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty);
            _stampStartY = (double)_currentStamp.GetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty);

            if (e is Controls.Stamps.ResizeMoveEdgesEventArgs)
            {
                var te = e as Controls.Stamps.ResizeMoveEdgesEventArgs;
                
                _stampStartWidth = ((FrameworkElement)_currentStamp).Width;
                _stampStartHeight = ((FrameworkElement)_currentStamp).Height;
                
                _stampEA = e;
                
                IsResizingStamp = true;
            }
            else if (e is IStampEventArgs)
            {
                var te = e as IStampEventArgs;
                var stamp = ((IStamp)_currentStamp);

                if (te.ActionType == eActionTypes.CloseStamp)
                {
                    stamp.PerformAction -= Stamp_PerformAction;
                    cvMainAdorner.Children.Remove((UIElement)stamp);

                    return;
                }
                else if (te.ActionType == eActionTypes.RotateBottomLeft)
                {
                    _stampStartRotation = ((CompositeTransform)((UIElement)stamp).RenderTransform).Rotation;
                }
                if (te.ActionType == eActionTypes.CreateFromStamp)
                {
                    if (_currentPageLayoutForStamps != null)
                    {
                        var plvm = _currentPageLayoutForStamps.DataContext as SketchPage;
                        var npl = new PageLayer();
                        var gt = ((FrameworkElement)stamp).TransformToVisual(_currentPageLayoutForStamps);
                        npl.HasChildContainerCanvas = true;
                        var ptCenter = gt.TransformPoint(new Windows.Foundation.Point(0, 0));
                        var uid = RandomString(15);
                        var str = stamp.GenerateXAML(uid, cvMainContainer.Scale, cvMainContainer.Scale, ptCenter.X, ptCenter.Y);
                        npl.XamlFragments.Add(new XamlFragment() { Uid = uid, Xaml = str, Type = stamp.GetType(), Data = stamp.GetData() });

                        plvm.Layers.Add(npl);
                        plvm.ExternalPC("Layers");
                    }
                }

            }
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var ret =  new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return "stamp_" + ret;
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
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, pg.Left);
            nc.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, pg.Top);
            nc.PerformAction += PageLayout_PerformAction;
            cvMain.Children.Add(nc);
            vm.Pages.Add(pg);

            return rect;
        }

        bool IsMovingPage = false;
        bool IsResizingPage = false;
        Controls.PageLayout _currentPageLayout;
        Controls.PageLayout _currentPageLayoutForStamps;

        private void PageLayout_PerformAction(object sender, EventArgs e)
        {
            if (e is Controls.PageLayoutEventArgs)
            {
                var ea = e as Controls.PageLayoutEventArgs;
                _currentPageLayout = sender as Controls.PageLayout;
                _currentPageLayoutForStamps = sender as Controls.PageLayout;
                if (ea.ActionType == "PageSelected")
                {
                    if (sender is PageLayout)
                        vm.SelectedPage = ((FrameworkElement)sender).DataContext as SketchPage;
                }
                else if (ea.ActionType == "MovePageLayoutStarted") IsMovingPage = true;
                else if (ea.ActionType == "MovePageLayoutFinished") IsMovingPage = false;
                else if (ea.ActionType == "ResizePageLayoutStarted") IsResizingPage = true;
                else if (ea.ActionType == "ResizePageLayoutFinished") IsResizingPage = false;


                ptStartPt.X = (double)_currentPageLayout.GetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty);
                ptStartPt.Y = (double)_currentPageLayout.GetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty);

            }
            else if (e is PageLayerEventArgs)
            {
                var plea = e as PageLayerEventArgs;
                if (plea.ActionType == "EditLayer")
                {
                    foreach (var ci in cvMain.Children)
                    {
                        if (ci is PageLayout)
                        {
                            var pg = ((FrameworkElement)ci).DataContext as SketchPage;
                            ((PageLayout)ci).SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, pg.Left);
                            //((PageLayout)ci).SetValue(Canvas.TopProperty, pg.Top);  //<-- for some reason the last page always repositions very low (need to resolve before uncommenting)
                        }
                    }

                }
                else if (plea.ActionType == "EditXamlFragment")
                {
                    //plea.Layer.IsEnabled = !plea.Layer.IsEnabled;
                    //foreach (var frag in plea.Layer.XamlFragments)
                    //{

                    var frag = plea.Fragment;

                    var found = _currentPageLayoutForStamps?.FindContentElementByName(frag.Uid) as FrameworkElement;
                    if (found != null)
                    {
                        var gtFound = cvMainAdorner.TransformToVisual(found);
                        var ptFound = gtFound.TransformPoint(new Windows.Foundation.Point(0, 0));

                        var gtPL = cvMainAdorner.TransformToVisual(_currentPageLayoutForStamps);
                        var ptPL = gtPL.TransformPoint(new Windows.Foundation.Point(0, 0));

                        var width = found.Width * cvMainContainer.Scale;
                        var height = found.Height * cvMainContainer.Scale;
                        var left = ((ptPL.X * -1) + (ptFound.X * -1)) * cvMainContainer.Scale;
                        var top = ((ptPL.Y * -1) + (ptFound.Y * -1) + 80) * cvMainContainer.Scale;  //70 = tabs

                        //var el = new Ellipse() { Width = 10, Height = 10, Fill = new SolidColorBrush(Colors.Red) };
                        //el.SetValue(Canvas.LeftProperty, left);
                        //el.SetValue(Canvas.TopProperty, top);
                        //cvMainAdorner.Children.Add(el);

                        //only if the stamp is being created in the viewable area
                        if (top > 20) CreateStamp(frag.Type, left, top, width, height, template: found, data: frag.Data);
                    }


                    //};
                }
                else if (plea.ActionType == "DeleteXamlFragment")
                {
                    //var frag = plea.Fragment;
                    //foreach (var layer in vm.SelectedPage.Layers) {
                    //    var found = layer.XamlFragments.Where(x => x.Uid == frag.Uid).FirstOrDefault();
                    //    if (found != null) layer.XamlFragments.Remove(found);
                    //}

                    //vm.SelectedPage.ExternalPC("Layers");
                }
            }

        }



        private void layoutRoot_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {


            cvMainContainer.Zoom(sender, e);
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
                    newX = (newX * (1 / cvMainContainer.Scale));  //take into account the scale factor
                    newX = ptStartPt.X + newX; // add to current canvas position

                    //console1.Text = $"right  : {ptStartPt.X + newX}   ey :  { 0 }     ";
                }
                else
                {
                    newX = Math.Abs(ptStart.Position.X - ptEnd.Position.X); //diff from start
                    newX = (newX * (1 / cvMainContainer.Scale)); //take into account the scale factor
                    newX = ptStartPt.X - newX; // add to current canvas position

                    //console1.Text = $"left : {ptStartPt.X - newX}   ey :  { 0 }     ";
                }

                _currentPageLayout.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, newX);
                lvm.Left = (int)newX;
                lvm.ExternalPC("Left");

                if (ptEnd.Position.Y > ptStart.Position.Y)
                {
                    newY = Math.Abs(ptEnd.Position.Y - ptStart.Position.Y); //diff from start
                    newY = (newY * (1 / cvMainContainer.Scale));  //take into account the scale factor
                    newY = ptStartPt.Y + newY; // add to current canvas position

                    //console1.Text = $"right  : {ptStartPt.X + newY}   ey :  { 0 }     ";
                }
                else
                {
                    newY = Math.Abs(ptStart.Position.Y - ptEnd.Position.Y); //diff from start
                    newY = (newY * (1 / cvMainContainer.Scale)); //take into account the scale factor
                    newY = ptStartPt.Y - newY; // add to current canvas position

                    //console1.Text = $"left : {ptStartPt.Y - newY}   ey :  { 0 }     ";
                }

                _currentPageLayout.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, newY);
                lvm.Top = (int)newX;
                lvm.ExternalPC("Top");

                //_currentPageLayout.SetValue(Canvas.TopProperty, (ptStartPt.Y - (ptStart.Position.Y - ptEnd.Position.Y)));
                return;
            }
            else if (IsResizingPage)
            {

                return;
            }
            else if (IsResizingStamp)
            {
                console2.Text = $"sx : {ptStart.Position.X}   sy :  { ptStart.Position.Y }     ";
                console3.Text = $"deltax : {ptEnd.Position.X - ptStart.Position.X}   deltay :  { ptEnd.Position.Y - ptStart.Position.Y}     ";

                var stampe = _stampEA as Controls.Stamps.ResizeMoveEdgesEventArgs;

                if (stampe.ActionType == eActionTypes.MoveTopLeft)
                {
                    console2.Text = $"sx : {ptStart.Position.X}  { _stampStartX }  sy :  { ptStart.Position.Y } { _stampStartY }    ";

                    _currentStamp.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, _stampStartX + (ptEnd.Position.X - ptStart.Position.X));
                    _currentStamp.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, _stampStartY + (ptEnd.Position.Y - ptStart.Position.Y));

                }
                else if (stampe.ActionType == eActionTypes.RotateBottomLeft)
                {
                    var ang = 180 / Math.PI * Math.Atan((ptStart.Position.Y - ptEnd.Position.Y) / (ptStart.Position.X - ptEnd.Position.X));

                    ((IStamp)_currentStamp).UpdateRotation(_stampStartRotation - (ptEnd.Position.X - ptStart.Position.X));
                    //((CompositeTransform)_currentStamp.RenderTransform).Rotation = _stampStartRotation - (ptEnd.Position.X - ptStart.Position.X);

                    console2.Text = $"sx : {ptStart.Position.X}  { _stampStartX }  sy :  { ptStart.Position.Y } { _stampStartY }    angle : { ang }  ";

                }
                else if (stampe.ActionType == eActionTypes.ResizeBottomRight)
                {
                    ((FrameworkElement)_currentStamp).Width = _stampStartWidth + (ptEnd.Position.X - ptStart.Position.X);
                    ((FrameworkElement)_currentStamp).Height = _stampStartHeight + (ptEnd.Position.Y - ptStart.Position.Y);
                }
                else if (stampe.ActionType == eActionTypes.ResizeCenterRight)
                {
                    console2.Text = $"thickness : { ((ptEnd.Position.X - ptStart.Position.X) / 10) }";
                    var newThickness = Math.Abs(((ptEnd.Position.X - ptStart.Position.X) / 10));
                    if (_currentStamp is IStamp) ((IStamp)_currentStamp).UpdateStrokeThickness(newThickness);
                }

                return;
            }
            else
            {
                //moving artboard
                var ct = cvMain.RenderTransform as CompositeTransform;

                line0.Text = $"sx : {ptStart.Position.X}   sy :  { ptStart.Position.Y} ";
                line1.Text = $"ex : {ptEnd.Position.X}   ey :  { ptEnd.Position.Y} ";
                line2.Text = $"scale : {cvMainContainer.Scale}   ptDifXStart :  { ptDifXStart}  ptDifYStart :  { ptDifYStart} ";
                line3.Text = $"translateX : {ct.TranslateX}   translateY :  { ct.TranslateY} ";

                ptDifX = ptDifXStart + ((ptStart.Position.X - ptEnd.Position.X) / cvMainContainer.Scale);
                ptDifY = ptDifYStart + ((ptStart.Position.Y - ptEnd.Position.Y) / cvMainContainer.Scale);

                line4.Text = $"s-e X : {ptStart.Position.X - ptEnd.Position.X}   s-e Y :  { ptStart.Position.Y - ptEnd.Position.Y}  ";


                ct.TranslateX = -1 * ptDifX;
                ct.TranslateY = -1 * ptDifY;


            }






        }
    }


}


//http://plnkr.co/edit/II6lgj511fsQ7l0QCoRi?p=preview  <== i used this in the end
//http://stackoverflow.com/questions/2916081/zoom-in-on-a-point-using-scale-and-translate