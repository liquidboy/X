﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;


namespace X.UI.LiteTab
{
    public sealed class TabList : Control
    {
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
   
        }


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_icTabList == null) { 

                _icTabList = GetTemplateChild("icTabList") as ItemsControl;

                if (this.ItemsSource != null) _icTabList.ItemsSource = this.ItemsSource;
                else _icTabList.ItemsSource = _data;

                if(this.ItemTemplate!= null) _icTabList.ItemTemplate = this.ItemTemplate;

                
            }


        }



        public void AddTab(string name, bool isSelected = false) {
            var newTab = new Tab() { Name = name, IsSelected = isSelected };

            _data.Add(newTab);
            if (isSelected) _selectedTab = newTab; 
        }
    }



}



//https://social.msdn.microsoft.com/Forums/silverlight/en-US/4a7b8135-5850-45a4-a149-a83496a311ce/setting-itemtemplates-events-in-a-template-control?forum=silverlightnet