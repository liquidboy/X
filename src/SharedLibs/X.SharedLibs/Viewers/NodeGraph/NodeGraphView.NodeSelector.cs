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
            var defaultWidth = 200d;
            var nt = Enum.Parse(typeof(NodeType), nodeTypeName);
            var newId = $"Node-{Guid.NewGuid().ToString()}";
            var nodePosX = 1500d;
            var nodePosY = 1200d;
            var groupingGuid = IsGraphSelected ? SelectedGraphGuid : Guid.Empty.ToString();

            switch (nt) {
                case NodeType.TextboxValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.TextboxValue, "Value"));
                    break;
                case NodeType.TextureAsset:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 1, "filename", 1, "image", groupingGuid, (int)NodeType.TextureAsset, "Texture Asset"));
                    break;
                case NodeType.AlphaMaskEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 2, "source,mask", 1, "", groupingGuid, (int)NodeType.AlphaMaskEffect, "Alpha Mask"));
                    break;
                case NodeType.GrayscaleEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 1, "source", 1, "", groupingGuid, (int)NodeType.GrayscaleEffect, "Grayscale"));
                    break;
                case NodeType.HueRotationEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 2, "source,angle", 1, "", groupingGuid, (int)NodeType.HueRotationEffect, "Hue Rotation"));
                    break;
                case NodeType.SliderValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 300d, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.SliderValue, "Value", 0d, 1d, 0.1d));
                    break;
                case NodeType.ContrastEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 2, "source,contrast", 1, "", groupingGuid, (int)NodeType.ContrastEffect, "Contrast"));
                    break;
                case NodeType.ArithmeticEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 6, "source1,source1Amount,source2,source2Amount,multiplyAmount,offset", 1, "", groupingGuid, (int)NodeType.ArithmeticEffect, "Arithmentic Composite")); break;
                case NodeType.BlendEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 3, "background,foreground,mode", 1, "", groupingGuid, (int)NodeType.BlendEffect, "Blend"));
                    break;
                case NodeType.BlendEffectModeValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 600d, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.BlendEffectModeValue, "Value", 0d, 25d, 1d));
                    break;
            }
        }
    }
}
