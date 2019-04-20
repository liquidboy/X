using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace X.ModernDesktop
{

    public sealed partial class GlobalNodeTypeEditor : Page
    {
        Assembly _foundControls;
        Type[] _foundControlTypes;

        public ObservableCollection<ControlMetaData> AllControls { get; set; }
        public CollectionViewSource FilteredControlsCVS { get; set; }
        public ObservableCollection<ControlMetaData> FilterdControls { get; set; }

        public ObservableCollection<ControlMetaData> FilterdControlsEnums { get; set; }

        public GlobalNodeTypeEditor()
        {
            FilteredControlsCVS = new CollectionViewSource();
            FilteredControlsCVS.IsSourceGrouped = false;
            AllControls = new ObservableCollection<ControlMetaData>();

            FilterdControls = new ObservableCollection<ControlMetaData>();
            FilteredControlsCVS.Source = FilterdControls;

            FilterdControlsEnums = new ObservableCollection<ControlMetaData>();

            this.InitializeComponent();
            
            FillFromXaml();
            FilterData("");
        }


        public void FillFromXaml()
        {
            _foundControls = Assembly.GetAssembly(typeof(Control));
            _foundControlTypes = _foundControls.GetTypes();
            foreach (var foundControl in _foundControlTypes)
            {
                var ncmd = new ControlMetaData() { Name = foundControl.Name, FullName = foundControl.FullName };
                AllControls.Add(ncmd);
                //_nodeTypeMetadata.Add(new CloudNodeTypeMetadata(foundControl.Name, foundControl.FullName));

                if (foundControl.IsEnum)
                {
                    FilterdControlsEnums.Add(ncmd);
                }
            }
        }

        public void FilterData(string filterBy)
        {
            var query = from item in AllControls
                        where item.Name.Contains(filterBy, StringComparison.InvariantCultureIgnoreCase)
                        orderby item.Name
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
            var cmd = (ControlMetaData)lbItems.SelectedItem;
            if (cmd == null) return;
            var foundControlType = _foundControlTypes.Where(x => x.Name.Equals(cmd.Name)).FirstOrDefault();
            grdSelectedItem.DataContext = foundControlType;

           // var xaml = Clone<Button>(new Button());

        }

        public static T Clone<T>(T source)
        {
            //string objXaml = XamlWriter.Save(source); //Serialization
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, source);
            string objXaml = stringWriter.ToString();

            //StringReader stringReader = new StringReader(objXaml);
            //XmlReader xmlReader = XmlReader.Create(stringReader);
            //T t = (T)XamlReader.Load(xmlReader); //Deserialization
            T t = (T)XamlReader.Load(objXaml); //Deserialization
            return t;
        }

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
