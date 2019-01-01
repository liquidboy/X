using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using SamplesCommon;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphCompositor
    {
        Compositor _rootCompositor;
        
        IDictionary<string, NodeVisual> _nodeVisuals;
        
        public class NodeVisual {
            public string NodeKey { get; set; }
            public ContainerVisual ContainerVisual;
            public SpriteVisual SpriteVisual;
            public CompositionBrush Brush;
        }
        
        public void InitializeCompositor(UIElement rootVisualElement)
        {
            _rootCompositor = ElementCompositionPreview.GetElementVisual(rootVisualElement).Compositor;
            _nodeVisuals = new Dictionary<string, NodeVisual>();
        }

        public void CreateNodeVisual(NodeNodeLinkModel nodeNodeLinkModel, UIElement parentRootOfVisual, NodeType nodeType)
        {
            var nodeTypeInt = (int)nodeType;
            FrameworkElement fe = (FrameworkElement)parentRootOfVisual;
            var nodeVisual = new NodeVisual();

            if (nodeTypeInt > 100 && nodeTypeInt <= 1000) { //EFFECT NODES
                var compositor = ElementCompositionPreview.GetElementVisual(parentRootOfVisual).Compositor;

                nodeVisual.ContainerVisual = compositor.CreateContainerVisual();
                nodeVisual.SpriteVisual = compositor.CreateSpriteVisual();

                ElementCompositionPreview.SetElementChildVisual(parentRootOfVisual, nodeVisual.ContainerVisual);

                //get input sources and pass them to effect graph
                object[] sources = new object[nodeNodeLinkModel.Node.InputSlotCount];
                if (nodeNodeLinkModel.InputNodeLinks.Count > 0) sources[0] = nodeNodeLinkModel.InputNodeLinks[0];
                nodeVisual.Brush = CreateGraphicsBrush(compositor, nodeType, sources);
                ResizeSpriteBrush(fe.ActualWidth - 20, fe.ActualHeight - 20, nodeVisual.SpriteVisual);
                nodeVisual.SpriteVisual.Brush = nodeVisual.Brush;

                nodeVisual.ContainerVisual.Children.InsertAtTop(nodeVisual.SpriteVisual);
                _nodeVisuals.Add(nodeNodeLinkModel.Node.Key, nodeVisual);
            } else if (nodeTypeInt > 1000) //VALUE NODES
            {
                
            }

            
        }

        private void ResizeSpriteBrush(double availableWidth, double availableHeight, SpriteVisual spriteVisual)
        {
            //double newWidth = visibleWidth;
            //double newHeight = visibleHeight;

            //spriteVisual.Offset = new Vector3(0f, 0f, 0.0f);
            //spriteVisual.Size = new Vector2((float)newWidth, (float)newHeight);

            double newWidth = availableWidth;
            double newHeight = availableHeight;
            double imageAspectRatio = availableWidth / availableHeight;

            //newWidth = availableWidth * imageAspectRatio;
            //newHeight = newHeight * imageAspectRatio;

            spriteVisual.Offset = new Vector3(10f, 10.0f, 0.0f);
            spriteVisual.Size = new Vector2((float)newWidth, (float)newHeight);
        }




        private CompositionBrush CreateGraphicsBrush(Compositor compositor, NodeType effectType, object[] inputSlotSources) {

            switch (effectType) {
                case NodeType.ImageEffect:
                    Size imageSize;
                    var brushNoEffect = CreateBrushFromAsset(compositor, (string)((NodeLink)inputSlotSources[0]).Value1, out imageSize);
                    var imageAspectRatio = (imageSize.Width == 0 && imageSize.Height == 0) ? 1 : imageSize.Width / imageSize.Height;
                    return brushNoEffect;
                case NodeType.AlphaMaskEffect:
                    var desc = new CompositeEffect
                    {
                        Mode = CanvasComposite.DestinationIn,
                        Sources = {
                            new CompositionEffectSourceParameter("Image"),
                            new Transform2DEffect
                            {
                                Name = "MaskTransform",
                                Source = new CompositionEffectSourceParameter("Mask")
                            }
                        }
                    };
                    var brushAlphaMask = compositor.CreateEffectFactory(
                        desc, 
                        new[] { "MaskTransform.TransformMatrix" }
                        ).CreateBrush();
                    brushAlphaMask.SetSourceParameter("Image", (CompositionBrush)inputSlotSources[0]); //m_noEffectBrush);
                    brushAlphaMask.SetSourceParameter("Mask", (CompositionBrush)inputSlotSources[1]); //CreateBrushFromAsset("CircleMask.png"));
                    return brushAlphaMask;
                default:
                    throw new NotImplementedException();
            }
        }

        private CompositionSurfaceBrush CreateBrushFromAsset(Compositor compositor, string name, out Size size)
        {
            ImageLoader.Initialize(compositor);
            CompositionDrawingSurface surface = ImageLoader.Instance.LoadFromUri(new Uri("ms-appx:///Assets/" + name)).Surface;
            size = surface.Size;
            return compositor.CreateSurfaceBrush(surface);
        }

        public void ClearCompositor()
        {
            foreach (var nodeVisual in _nodeVisuals) {
                nodeVisual.Value.ContainerVisual.Dispose();
                nodeVisual.Value.ContainerVisual = null; 
                nodeVisual.Value.SpriteVisual.Dispose();
                nodeVisual.Value.SpriteVisual = null;
                nodeVisual.Value.Brush.Dispose();
                nodeVisual.Value.Brush = null;
            }
            _nodeVisuals.Clear();
        }
    }
}
