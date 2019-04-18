﻿using System;
using System.Collections.Generic;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeSelector
    {
        private static List<NodeTypeMetadata> _nodeTypeMetadata;
        
        public void InitializeNodeSelector()
        {
            _nodeTypeMetadata = new List<NodeTypeMetadata>();
            FillFromNodeTypeEnum();
            FillFromGlobalStorage();
        }

        private void FillFromNodeTypeEnum() {
            var values = Enum.GetValues(typeof(NodeType));
            foreach (var value in values)
            {
                NodeType nt = (NodeType)value;
                if (nt != NodeType.CloudNodeType) {
                    _nodeTypeMetadata.Add(new NodeTypeMetadata(nt));
                }
            }
        }

        private void FillFromGlobalStorage() {
            // todo: pull from cloud and put in sqlite storage (if applicable)
            _nodeTypeMetadata.Add(new CloudNodeTypeMetadata("Entity", "dots"));
            _nodeTypeMetadata.Add(new CloudNodeTypeMetadata("Component", "dots"));
            _nodeTypeMetadata.Add(new CloudNodeTypeMetadata("System", "dots"));
        }

        public IEnumerable<NodeTypeMetadata> GetNodeTypeMetaData()
        {
            if (_nodeTypeMetadata == null) InitializeNodeSelector();
            return _nodeTypeMetadata;
        }

        public void OnNodeTypeSelected(NodeType nodeType)
        {
            var defaultWidth = 200d;
            var newId = $"Node-{Guid.NewGuid().ToString()}";
            var nodePosX = 1500d;
            var nodePosY = 1200d;
            var groupingGuid = IsGraphSelected ? SelectedGraphGuid : Guid.Empty.ToString();

            switch (nodeType) {
                case NodeType.TextboxValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.TextboxValue, "Text Value"));
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
                case NodeType.SaturationEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 2, "source,contrast", 1, "", groupingGuid, (int)NodeType.SaturationEffect, "Saturation"));
                    break;
                case NodeType.ArithmeticEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 6, "source1,source1Amount,source2,source2Amount,multiplyAmount,offset", 1, "", groupingGuid, (int)NodeType.ArithmeticEffect, "Arithmentic Composite")); break;
                case NodeType.BlendEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 3, "background,foreground,mode", 1, "", groupingGuid, (int)NodeType.BlendEffect, "Blend"));
                    break;
                case NodeType.BlendEffectModeValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 600d, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.BlendEffectModeValue, "Belnd Mode Value", 0d, 25d, 1d));
                    break;
                case NodeType.ColorSliderValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 300d, "WhiteSmoke", 0, "", 4, "R,G,B,A", groupingGuid, (int)NodeType.ColorSliderValue, "Color Value"));
                    break;
                case NodeType.ColorSourceEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 4, "R,G,B,A", 1, "", groupingGuid, (int)NodeType.ColorSourceEffect, "Color"));
                    break;
                case NodeType.ExposureSliderValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 300d, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.SliderValue, "Exposure Value", -2d, 2d, 0.1d));
                    break;
                case NodeType.ExposureEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 2, "source,exposure", 1, "", groupingGuid, (int)NodeType.ExposureEffect, "Exposure"));
                    break;
                case NodeType.GammaTransferValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 350d, "WhiteSmoke", 0, "", 12, "red amplitude,red exponent,red offset,green amplitude,green exponent,green offset,blue amplitude,blue exponent,blue offset,alpha amplitude,alpha exponent,alpha offset", groupingGuid, (int)NodeType.GammaTransferValue, "Gamma Transfer Value", 50d, 35d));
                    break;
                case NodeType.GammaTransferEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 13, "source,red amplitude,red exponent,red offset,green amplitude,green exponent,green offset,blue amplitude,blue exponent,blue offset,alpha amplitude,alpha exponent,alpha offset", 1, "", groupingGuid, (int)NodeType.GammaTransferEffect, "Gamma Transfer"));
                    break;
                case NodeType.InvertEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 1, "source", 1, "", groupingGuid, (int)NodeType.InvertEffect, "Invert"));
                    break;
                case NodeType.CanvasAlphaModeValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.CanvasAlphaModeValue, "Canvas Alpha Mode Value", 0d, 1d, 1d));
                    break;
                case NodeType.SepiaEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 3, "source,intensity,canvas alpha mode", 1, "", groupingGuid, (int)NodeType.SepiaEffect, "Sepia"));
                    break;
                case NodeType.TemperatureAndTintEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 3, "source,temperature,tint", 1, "", groupingGuid, (int)NodeType.TemperatureAndTintEffect, "Temperature " +
                        "Tint"));
                    break;
                case NodeType.TintValue:
                case NodeType.TemperatureValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 300d, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.SliderValue, "Tint/Temp Value", -1d, 1d, 0.1d));
                    break;
                case NodeType.BorderModeValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.BorderModeValue, "Border Mode Value", 0d, 1d, 1d));
                    break;
                case NodeType.Transform2DEffect:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 5, "source,transform matrix,border mode,interpolation mode,sharpness", 1, "", groupingGuid, (int)NodeType.Transform2DEffect, "Transform2D"));
                    break;
                case NodeType.TransformMatrixValue:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, 400d, "WhiteSmoke", 0, "", 1, "", groupingGuid, (int)NodeType.TransformMatrixValue, "Matrix Value"));
                    break;


                case NodeType.XamlFragment:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 1, "xaml", 1, "", groupingGuid, (int)NodeType.XamlFragment, "Xaml Fragment"));
                    break;
                case NodeType.PathScene:
                    AddNodeToGraph(new Node(newId, nodePosX, nodePosY, defaultWidth, "WhiteSmoke", 2, "xaml fragment,path", 1, "", groupingGuid, (int)NodeType.PathScene, "Path Scene"));
                    break;
            }
        }
    }
}
