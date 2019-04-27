using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
using X.CoreLib.Shared.Framework.Services.DataEntity;
using X.Viewer.NodeGraph;

namespace X.Viewer.FileExplorer
{
    public sealed partial class Explorer : UserControl
    {
        private ObservableCollection<Item> DataSource = new ObservableCollection<Item>();

        public Explorer()
        {
            this.InitializeComponent();

            InitializeStorage();
            CreateSampleData();
        }


        private void DoTestStorage(object sender, RoutedEventArgs e)
        {
            FileExplorerGlobalStorage.Current.TestStorage();
        }


        private void CreateSampleData() {
            var found = RetrieveFolders();
            if (found.Count == 0)
            {
                var newGlobalFolder = new SavedFolder("Global", DateTime.Now, DateTime.Now, string.Empty);
                Save(newGlobalFolder);
                var newPrivateFolder = new SavedFolder("Private", DateTime.Now, DateTime.Now, string.Empty);
                Save(newPrivateFolder);
                var newglTFFolder = new SavedFolder("glTF", DateTime.Now, DateTime.Now, newPrivateFolder.UniqueId.ToString());
                Save(newglTFFolder);
                var newImagesFolder = new SavedFolder("Images", DateTime.Now, DateTime.Now, newPrivateFolder.UniqueId.ToString());
                Save(newImagesFolder);
                var newFontsFolder = new SavedFolder("Fonts", DateTime.Now, DateTime.Now, newPrivateFolder.UniqueId.ToString());
                Save(newFontsFolder);

                found = RetrieveFolders();
            }

            DataSource = GetChildren(found, string.Empty);            
        }

        private ObservableCollection<Item> GetChildren(List<SavedFolder> nodes, string grouping) {

            var rootItems = nodes.Where(x => x.Grouping.Equals(grouping, StringComparison.InvariantCultureIgnoreCase));
            var col = new ObservableCollection<Item>();
            foreach (var rootItem in rootItems)
            {
                col.Add(new Item() { Folder = rootItem, Children = GetChildren(nodes, rootItem.UniqueId.ToString())});
            }
            return col;
        }

        private void DoClearStorage(object sender, RoutedEventArgs e)
        {
            ClearStorage();
        }
    }


    public class Item
    {
        public SavedFolder Folder { get; set; }

        public ObservableCollection<Item> Children { get; set; } = new ObservableCollection<Item>();

        public override string ToString()
        {
            return Folder.Name;
        }
    }
}
