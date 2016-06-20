using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
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

        //EffectLayer.EffectLayer _bkgLayer;//x

        public Orientation Orientation { get; set; }


        ItemsControl _icTabList;
        //int RenderTargetIndexFor_icTabList = 0;
        List<X.UI.LiteTab.Tab> _data = new List<Tab>();

        Tab _selectedTab;
        public event EventHandler OnTabChanged;
        
        
      
        public void Invalidate() { 
            //_bkgLayer?.DrawUIElements(_icTabList, RenderTargetIndexFor_icTabList); 
        }



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
        
        public Color GlowColor
        {
            get { return (Color)GetValue(GlowColorProperty); }
            set { SetValue(GlowColorProperty, value); }
        }

        public ICommand TabChangedCommand
        {
            get { return (ICommand)GetValue(TabChangedCommandProperty); }
            set { SetValue(TabChangedCommandProperty, value); }
        }








        public static readonly DependencyProperty TabChangedCommandProperty =
            DependencyProperty.Register("TabChangedCommand", typeof(ICommand), typeof(TabList), new PropertyMetadata(null));

        public static readonly DependencyProperty GlowColorProperty =
            DependencyProperty.Register("GlowColor", typeof(Color), typeof(TabList), new PropertyMetadata(Colors.Black));

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

            Invalidate();

            OnTabChanged?.Invoke(_selectedTab, EventArgs.Empty);
            TabChangedCommand?.Execute(null);
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
            //if (_bkgLayer != null)
            //{
            //    _bkgLayer.DrawUIElements(_icTabList);  //will draw at index 0 (RenderTargetIndexFor_icTabList)
            //    _bkgLayer.InitLayer(_icTabList.ActualWidth, _icTabList.ActualHeight);
            //}
        }
        

        protected override void OnApplyTemplate()
        {
            //if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;

            if (_icTabList == null) { 

                _icTabList = GetTemplateChild("icTabList") as ItemsControl;

                if (this.ItemsSource != null) _icTabList.ItemsSource = this.ItemsSource;
                else _icTabList.ItemsSource = _data;

                if(this.ItemTemplate!= null) _icTabList.ItemTemplate = this.ItemTemplate;

                
            }

            //if (_bkgLayer != null && _icTabList != null && _icTabList.ActualWidth != 0) _bkgLayer.InitLayer(_icTabList.ActualWidth, _icTabList.ActualHeight);

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