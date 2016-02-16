using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace X.UI.LiteTab
{
    public sealed class TabList : Control
    {

        EffectLayer.EffectLayer _bkgLayer;//x

        public Orientation Orientation { get; set; }


        ItemsControl _icTabList;
        List<X.UI.LiteTab.Tab> _data = new List<Tab>();

        Tab _selectedTab;
        public event EventHandler OnTabChanged;







        public Object ItemsSource
        {
            get { return (Object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        
        public Brush TabItemBorderColor
        {
            get { return (Brush)GetValue(TabItemBorderColorProperty); }
            set { SetValue(TabItemBorderColorProperty, value); }
        }

        
        



        public static readonly DependencyProperty TabItemBorderColorProperty =
            DependencyProperty.Register("TabItemBorderColor", typeof(Brush), typeof(TabList), new PropertyMetadata(Colors.LightGray));
        
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(Object), typeof(TabList), new PropertyMetadata(null));
        
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(TabList), new PropertyMetadata(null));








       
       

        public void ChangeSelectedTab(Tab tab) {

            if (_selectedTab != null)
            {
                _selectedTab.IsSelected = false;
            }

            _selectedTab = tab;
            
            if (this.OnTabChanged != null) this.OnTabChanged(_selectedTab, EventArgs.Empty);
        }



        public TabList()
        {
            this.DefaultStyleKey = typeof(TabList);

            this.Loaded += TabList_Loaded;
            this.Unloaded += TabList_Unloaded;
        }

        private void TabList_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_icTabList != null)
            {

            }
        }

        private async void TabList_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bkgLayer != null)
            {
                //_bkgLayer.DrawPath("M19.9036423248472,31.7099608139106L19.9036423248472,36.3499754623481 52.4696690124692,36.3499754623481 52.4696690124692,31.7099608139106z M13.8494345230283,31.5500487045356C12.4793964904966,31.5500487045356 11.3713909668149,32.6600340560981 11.3713909668149,34.0300291732856 11.3713909668149,35.4000242904731 12.4793964904966,36.5100096420356 13.8494345230283,36.5100096420356 15.2155052704038,36.5100096420356 16.3275410217466,35.4000242904731 16.3275410217466,34.0300291732856 16.3275410217466,32.6600340560981 15.2155052704038,31.5500487045356 13.8494345230283,31.5500487045356z M19.9036423248472,20.5700682357856L19.9036423248472,25.2000731185981 52.4696690124692,25.2000731185981 52.4696690124692,20.5700682357856z M13.8494345230283,20.4100340560981C12.4793964904966,20.4100340560981 11.3713909668149,21.5100096420356 11.3713909668149,22.8800047592231 11.3713909668149,24.2499998764106 12.4793964904966,25.3599852279731 13.8494345230283,25.3599852279731 15.2155052704038,25.3599852279731 16.3275410217466,24.2499998764106 16.3275410217466,22.8800047592231 16.3275410217466,21.5100096420356 15.2155052704038,20.4100340560981 13.8494345230283,20.4100340560981z M0,0L24.0007549804868,0 32.0010067505063,7.99999987641058 63.9999996704282,7.99999987641058 63.9999996704282,18.0200194076606 63.9999996704282,28.1199949935981 63.9999996704282,47.9999998764106 29.6959415954771,47.9999998764106 21.9386784118833,47.9999998764106 0,47.9999998764106z");

                _bkgLayer.DrawUIElements(_icTabList);
                _bkgLayer.InitLayer(_icTabList.ActualWidth, _icTabList.ActualHeight);

                
            }
        }


        //private async Task<bool> RenderUIElement() {

        //    using (var stream = await RenderToRandomAccessStream())
        //    {
        //        var device = new CanvasDevice();
        //        var bitmap = await CanvasBitmap.LoadAsync(device, stream);

        //        var renderer = new CanvasRenderTarget(device,
        //                                              bitmap.SizeInPixels.Width,
        //                                              bitmap.SizeInPixels.Height, bitmap.Dpi);

        //        using (var ds = renderer.CreateDrawingSession())
        //        {
        //            var blur = new GaussianBlurEffect();
        //            blur.BlurAmount = 5.0f;
        //            blur.Source = bitmap;
        //            ds.DrawImage(blur);
        //        }

        //        stream.Seek(0);
        //        await renderer.SaveAsync(stream, CanvasBitmapFileFormat.Png);

        //        BitmapImage image = new BitmapImage();
        //        image.SetSource(stream);
        //        paneBackground.ImageSource = image;
        //    }

        //    return true;
        //}



        protected override void OnApplyTemplate()
        {
            if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;

            if (_icTabList == null) { 

                _icTabList = GetTemplateChild("icTabList") as ItemsControl;

                if (this.ItemsSource != null) _icTabList.ItemsSource = this.ItemsSource;
                else _icTabList.ItemsSource = _data;

                if(this.ItemTemplate!= null) _icTabList.ItemTemplate = this.ItemTemplate;

                
            }

            if (_bkgLayer != null && _icTabList != null && _icTabList.ActualWidth != 0) _bkgLayer.InitLayer(_icTabList.ActualWidth, _icTabList.ActualHeight);

            base.OnApplyTemplate();
        }



        public void AddTab(string name, bool isSelected = false) {
            var newTab = new Tab() { Name = name, IsSelected = isSelected };

            _data.Add(newTab);
            if (isSelected) _selectedTab = newTab; 
        }
    }



}



//https://social.msdn.microsoft.com/Forums/silverlight/en-US/4a7b8135-5850-45a4-a149-a83496a311ce/setting-itemtemplates-events-in-a-template-control?forum=silverlightnet