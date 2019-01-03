using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeSelector
    {
        
        public void InitializeNodeSelector()
        {
            List<ComboBoxItem> cbItems = new List<ComboBoxItem>();

            var values = Enum.GetValues(typeof(NodeType));

            foreach(var value in values){
                cbItems.Add(new ComboBoxItem() { Content = Enum.Parse(typeof(NodeType), value.ToString()), Tag = (int)value});
            }

            cbNodes.ItemsSource = cbItems;


        }

        public void OnNodeTypeSelected(string nodeTypeName)
        {
            var nt = Enum.Parse(typeof(NodeType), nodeTypeName);
        }
    }
}
