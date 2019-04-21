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
using X.Viewer.NodeGraph;

namespace X.ModernDesktop
{

    public sealed partial class GlobalNodeTypeEditor : Page
    {
        //Assembly _foundControls;
        //Type[] _foundControlTypes;
        //private static List<CloudNodeTypeEntity> _nodeTypeMetadata;

        public ObservableCollection<CloudNodeTypeEntity> AllControls { get; set; }
        public CollectionViewSource FilteredControlsCVS { get; set; }
        public ObservableCollection<CloudNodeTypeEntity> FilterdControls { get; set; }

        //public ObservableCollection<ControlMetaData> FilterdControlsEnums { get; set; }

        public GlobalNodeTypeEditor()
        {
            //_nodeTypeMetadata = new List<CloudNodeTypeEntity>();
            NodeGraphGlobalStorage.Current.InitializeGlobalStorage(App.AzureConnectionString);

            FilteredControlsCVS = new CollectionViewSource();
            FilteredControlsCVS.IsSourceGrouped = false;
            AllControls = new ObservableCollection<CloudNodeTypeEntity>();

            FilterdControls = new ObservableCollection<CloudNodeTypeEntity>();
            FilteredControlsCVS.Source = FilterdControls;

            //FilterdControlsEnums = new ObservableCollection<ControlMetaData>();

            this.InitializeComponent();

            Task.Run(async () => {
                await FillFromGlobalStorage();
                await Dispatcher.RunAsync( Windows.UI.Core.CoreDispatcherPriority.Normal, ()=> FilterData(""));  //<-- back to UI
            }); 
            
        }


        private async Task<bool> FillFromGlobalStorage()
        {
            //var globalData = await RetrieveGlobalNodeTypes("Dots");
            var globalData = await NodeGraphGlobalStorage.Current.RetrieveAllGlobalNodeTypes();
            if (globalData.Count > 0) { 
                
                foreach (CloudNodeTypeEntity item in globalData.Results)
                {
                    //_nodeTypeMetadata.Add(new CloudNodeTypeMetadata(item.RowKey, item.PartitionKey, item.InputNodeSlots, item.InputNodeSlotCount, item.OutputNodeSlots, item.OutputNodeSlotCount, item.Color, item.View));

                    //var ncmd = new ControlMetaData() { Name = item.RowKey, FullName = $"{item.RowKey} ({item.PartitionKey})" };

                    AllControls.Add(item);
                    //FilterdControlsEnums.Add(ncmd);
                }
            }
            return true;
        }


        public void FilterData(string filterBy)
        {
            var query = from item in AllControls
                        where item.RowKey.Contains(filterBy, StringComparison.InvariantCultureIgnoreCase)
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
            var selectedItem = (CloudNodeTypeEntity)grdSelectedItem.DataContext;
           
            //entity.Color = selItem.
            await NodeGraphGlobalStorage.Current.SaveGlobalNodeType(selectedItem);
        }

        public void CreateNewItem()
        {

        }

        public void DeleteSelectedItem()
        {

        }

        //public static T Clone<T>(T source)
        //{
        //    //string objXaml = XamlWriter.Save(source); //Serialization
        //    XmlSerializer serializer = new XmlSerializer(typeof(T));
        //    StringWriter stringWriter = new StringWriter();
        //    serializer.Serialize(stringWriter, source);
        //    string objXaml = stringWriter.ToString();

        //    //StringReader stringReader = new StringReader(objXaml);
        //    //XmlReader xmlReader = XmlReader.Create(stringReader);
        //    //T t = (T)XamlReader.Load(xmlReader); //Deserialization
        //    T t = (T)XamlReader.Load(objXaml); //Deserialization
        //    return t;
        //}

        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/de2b3198-74e9-4465-a0e1-7b435014ccdd/is-there-any-way-to-serialize-the-style-?forum=wpf
        //https://blogs.u2u.be/diederik/post/serializing-and-deserializing-data-in-a-windows-store-app
    }

    //public class FilterByCollection<T> : ObservableCollection<T>
    //{
    //    public object Key { get; set; }

    //    public new IEnumerator<T> GetEnumerator()
    //    {
    //        return (IEnumerator<T>)base.GetEnumerator();
    //    }
    //}

    //public class ControlMetaData : INotifyPropertyChanged {
    //    public string Name { get; set; }
    //    public string FullName { get; set; }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //}

}
