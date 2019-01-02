using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
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
            
            var nodeVisuals = new Dictionary<string, NodeVisual>();
            var nodeVisual = new NodeVisual();

            if (nodeTypeInt > 100 && nodeTypeInt <= 1000) { //EFFECT NODES
                var compositor = ElementCompositionPreview.GetElementVisual(parentRootOfVisual).Compositor;

                nodeVisual.ContainerVisual = compositor.CreateContainerVisual();
                nodeVisual.SpriteVisual = compositor.CreateSpriteVisual();

                ElementCompositionPreview.SetElementChildVisual(parentRootOfVisual, nodeVisual.ContainerVisual);

                //get input sources and pass them to effect graph 
                nodeVisual.Brush = CreateGraphicsBrush(compositor, nodeType, nodeNodeLinkModel.InputNodeLinks.ToArray());
                
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
            double newWidth = availableWidth;
            double newHeight = availableHeight;
            double imageAspectRatio = availableWidth / availableHeight;

            spriteVisual.Offset = new Vector3(10f, 10.0f, 0.0f);
            spriteVisual.Size = new Vector2((float)newWidth, (float)newHeight);
        }




        private CompositionBrush CreateGraphicsBrush(Compositor compositor, NodeType effectType, object[] inputSlotSources) {

            switch (effectType) {
                case NodeType.TextureAsset:
                    if (inputSlotSources.Length == 0) return null;
                    var brushNoEffect = CreateBrushFromAsset(compositor, (string)((NodeLink)inputSlotSources[0]).Value1);
                    return brushNoEffect;
                case NodeType.AlphaMaskEffect:
                    if (inputSlotSources.Length < 2) return null;
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

                    var source1 = (NodeLink)inputSlotSources[0];
                    var source2 = (NodeLink)inputSlotSources[1];
                    
                    brushAlphaMask.SetSourceParameter("Image", (CompositionBrush)_nodeVisuals[source1.OutputNodeKey].Brush); 
                    brushAlphaMask.SetSourceParameter("Mask", (CompositionBrush)_nodeVisuals[source2.OutputNodeKey].Brush);
                    return brushAlphaMask;
                default:
                    throw new NotImplementedException();
            }
        }

        private CompositionSurfaceBrush CreateBrushFromAsset(Compositor compositor, string name)
        {
            //ImageLoader.Initialize(compositor);
            //CompositionDrawingSurface surface = ImageLoader.Instance.LoadFromUri(new Uri("ms-appx:///Assets/" + name)).Surface;
            
            SurfaceLoader.Initialize(compositor);
            var task = Task.Run(() => SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Assets/" + name)));
            task.Wait();
            var surface = task.Result;
            
            return compositor.CreateSurfaceBrush(surface);
        }

        public void ClearCompositor()
        {
            foreach (var nodeVisual in _nodeVisuals)
            {
                nodeVisual.Value.ContainerVisual?.Dispose();
                nodeVisual.Value.ContainerVisual = null;
                nodeVisual.Value.SpriteVisual?.Dispose();
                nodeVisual.Value.SpriteVisual = null;
                nodeVisual.Value.Brush?.Dispose();
                nodeVisual.Value.Brush = null;
            }
            _nodeVisuals.Clear();

            SurfaceLoader.Uninitialize();
        }
    }
}
