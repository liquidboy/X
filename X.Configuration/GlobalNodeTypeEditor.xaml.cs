using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.Configuration;
using X.UI.NodeGraph;
using X.Viewer.NodeGraph;

namespace X.ModernDesktop
{

    public sealed partial class GlobalNodeTypeEditor : Page
    {

        public ObservableCollection<CloudNodeTypeEntity> AllControls { get; set; }
        public CollectionViewSource FilteredControlsCVS { get; set; }
        public ObservableCollection<CloudNodeTypeEntity> FilterdControls { get; set; }

        public GlobalNodeTypeEditor()
        {
            NodeGraphGlobalStorage.Current.InitializeGlobalStorage(App.AzureConnectionString);

            FilteredControlsCVS = new CollectionViewSource();
            FilteredControlsCVS.IsSourceGrouped = false;
            AllControls = new ObservableCollection<CloudNodeTypeEntity>();

            FilterdControls = new ObservableCollection<CloudNodeTypeEntity>();
            FilteredControlsCVS.Source = FilterdControls;

            this.InitializeComponent();

            LoadPageData();
        }

        private void LoadPageData()
        {
            AllControls.Clear();
            FilterdControls.Clear();

            Task.Run(async () => {
                await FillFromGlobalStorage();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => FilterData(""));  //<-- back to UI
            });
        }



        private async Task<bool> FillFromGlobalStorage()
        {
            var globalData = await NodeGraphGlobalStorage.Current.RetrieveAllGlobalNodeTypes();
            if (globalData.Count > 0) { 
                
                foreach (CloudNodeTypeEntity item in globalData.Results)
                {
                    AllControls.Add(item);
                }
            }
            return true;
        }


        public void FilterData(string filterBy)
        {
            var query = from item in AllControls
                        where item.RowKey.Contains(filterBy, StringComparison.InvariantCultureIgnoreCase) || item.PartitionKey.Contains(filterBy, StringComparison.InvariantCultureIgnoreCase)
                        orderby item.RowKey
                        select item;

            FilterdControls?.Clear();
            foreach (var foundItem in query) {
                FilterdControls.Add(foundItem);
            }   
        }

        public void TextChanged() {
            FilterData(tbFilterBy.Text);
        }

        public void ItemSelected() { 
            grdSelectedItem.DataContext = lbItems.SelectedItem;
        }

        public async void SaveSelectedItem()
        {
            await NodeGraphGlobalStorage.Current.SaveGlobalNodeType((CloudNodeTypeEntity)grdSelectedItem.DataContext);
            LoadPageData();
        }

        public void CreateNewItem()
        {
            tbRowKey.IsReadOnly = false;
            tbRowKey.SetBinding(TextBox.TextProperty, new Binding() { Mode = BindingMode.TwoWay, Path = new PropertyPath("RowKey") });
            tbPartitionKey.IsReadOnly = false;
            tbPartitionKey.SetBinding(TextBox.TextProperty, new Binding() { Mode = BindingMode.TwoWay, Path = new PropertyPath("PartitionKey") });
        }

        public async void DeleteSelectedItem()
        {
            await NodeGraphGlobalStorage.Current.DeleteGlobalNodeType((CloudNodeTypeEntity)grdSelectedItem.DataContext);
            LoadPageData();
        }
    }

}
