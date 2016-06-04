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
    public sealed partial class SketchView
    {
        private void DeleteSketch(int sketchId)
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
                        var foundSPL = StorageService.Instance.Storage.RetrieveByField<SketchPageLayerDataModel>("SketchPageId", fsp.Id.ToString());
                        if (foundSPL != null && foundSPL.Count() > 0)
                        {
                            foreach (var fspl in foundSPL)
                            {
                                StorageService.Instance.Storage.DeleteByField<SketchPageLayerXamlFragmentDataModel>("SketchPageLayerId", fspl.Id.ToString());
                            }
                        }
                        StorageService.Instance.Storage.DeleteByField<SketchPageLayerDataModel>("SketchPageId", fsp.Id.ToString());
                    }
                }
                StorageService.Instance.Storage.DeleteByField<SketchPageDataModel>("SketchId", fs.Id.ToString());
                StorageService.Instance.Storage.DeleteById<SketchDataModel>(sketchId.ToString());
            }
        }


        private async void LoadSketch(int sketchId)
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
                        if (foundSPL != null && foundSPL.Count() > 0)
                        {
                            foreach (var fspl in foundSPL)
                            {
                                var pl = new PageLayer() { PersistedId = fspl.Id, HasChildContainerCanvas = fspl.HasChildContainerCanvas };

                                //add xamlfrags
                                var foundSPLXF = StorageService.Instance.Storage.RetrieveByField<SketchPageLayerXamlFragmentDataModel>("SketchPageLayerId", fspl.Id.ToString());
                                if (foundSPLXF != null && foundSPLXF.Count() > 0)
                                {
                                    foreach (var fsplxf in foundSPLXF)
                                    {
                                        var dt = string.IsNullOrEmpty(fsplxf.DataType) ? null : Type.GetType(fsplxf.DataType);
                                        var xf = new XamlFragment() { Uid = fsplxf.Uid, Xaml = fsplxf.Xaml, Namespaces = fsplxf.Namespaces, Resources = fsplxf.Resources,  Data = fsplxf.Data, Type = dt };
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


                        //// add xaml frags
                        //foreach (var pl in pg.Layers)
                        //{
                        //    var foundSPLXF = StorageService.Instance.Storage.RetrieveByField<SketchPageLayerXamlFragmentDataModel>("SketchPageLayerId", pl.PersistedId.ToString());
                        //    if (foundSPLXF != null && foundSPLXF.Count() > 0)
                        //    {
                        //        foreach (var fsplxf in foundSPLXF)
                        //        {
                        //            var dt =  string.IsNullOrEmpty(fsplxf.DataType) ? null: Type.GetType(fsplxf.DataType);
                        //            var xf = new XamlFragment() { Uid = fsplxf.Uid, Xaml = fsplxf.Xaml, Data = fsplxf.Data, Type = dt };
                        //            pl.XamlFragments.Add(xf);
                        //        }
                        //    }
                        //    pl.ExternalPC("Layers");
                        //}
                    }
                }

             
                //    foreach (var pgg in vm.Pages)
                //    {
                //        pgg.ExternalPC("Layers");
                //    }
            }



        }

        private void SaveSketch(string title)
        {
            var ni = new SketchDataModel() { Title = title };
            StorageService.Instance.Storage.Insert(ni);

            foreach (var pg in vm.Pages)
            {
                var nip = new SketchPageDataModel() { SketchId = ni.Id, Title = pg.Title, Left = pg.Left, Top = pg.Top, Width = pg.Width, Height = pg.Height };
                StorageService.Instance.Storage.Insert(nip);

                foreach (var pgl in pg.Layers)
                {
                    var npgl = new SketchPageLayerDataModel() { SketchPageId = nip.Id, HasChildContainerCanvas = pgl.HasChildContainerCanvas };
                    StorageService.Instance.Storage.Insert(npgl);
                    
                    foreach (var xf in pgl.XamlFragments)
                    {
                        var nxf = new SketchPageLayerXamlFragmentDataModel() { SketchPageLayerId = npgl.Id, Uid = xf.Uid, Xaml = xf.Xaml, Namespaces = xf.Namespaces, Resources = xf.Resources, Data = xf.Data, DataType = xf.Type != null? xf.Type.ToString(): null };
                        StorageService.Instance.Storage.Insert(nxf);

                    }
                }

            }
        }

        private void DeleteALLSketchs()
        {
            StorageService.Instance.Storage.Truncate<SketchDataModel>();
            StorageService.Instance.Storage.Truncate<SketchPageDataModel>();
            StorageService.Instance.Storage.Truncate<SketchPageLayerDataModel>();
            StorageService.Instance.Storage.Truncate<SketchPageLayerXamlFragmentDataModel>();
            toolbar.ClearSketchs();
        }

        private void LoadAllSketchs() {
            var foundS = StorageService.Instance.Storage.RetrieveList<SketchDataModel>();
            toolbar.LoadSketchs(foundS);
        }

        private void LoadSampleSketch()
        {
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

    }
}
