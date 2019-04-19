using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.ModernDesktop
{

    public sealed partial class XamlReflection : Page
    {
        public ObservableCollection<ControlMetaData> AllControls { get; set; }
        public CollectionViewSource FilteredControlsCVS { get; set; }
        public ObservableCollection<ControlMetaData> FilterdControls { get; set; }

        public XamlReflection()
        {
            FilteredControlsCVS = new CollectionViewSource();
            FilteredControlsCVS.IsSourceGrouped = false;
            AllControls = new ObservableCollection<ControlMetaData>();

            FilterdControls = new ObservableCollection<ControlMetaData>();
            FilteredControlsCVS.Source = FilterdControls;

            this.InitializeComponent();
            
            FillFromXaml();
            FilterData("");
        }


        public void FillFromXaml()
        {
            Assembly foundControls = Assembly.GetAssembly(typeof(Control));
            var foundControlTypes = foundControls.GetTypes();
            foreach (var foundControl in foundControlTypes)
            {
                AllControls.Add(new ControlMetaData() { Name = foundControl.Name, FullName = foundControl.FullName });
                //_nodeTypeMetadata.Add(new CloudNodeTypeMetadata(foundControl.Name, foundControl.FullName));
            }
        }

        public void FilterData(string filterBy)
        {
            var query = from item in AllControls
                        where item.Name.Contains(filterBy, StringComparison.InvariantCultureIgnoreCase)
                        orderby item.Name
                        select item;

            FilterdControls.Clear();
            foreach (var foundItem in query) {
                FilterdControls.Add(foundItem);
            }   
        }

        public void TextChanged() {
            FilterData(tbFilterBy.Text);
        }
    }

    public class FilterByCollection<T> : ObservableCollection<T>
    {
        public object Key { get; set; }

        public new IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)base.GetEnumerator();
        }
    }

    public class ControlMetaData : INotifyPropertyChanged {
        public string Name { get; set; }
        public string FullName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
