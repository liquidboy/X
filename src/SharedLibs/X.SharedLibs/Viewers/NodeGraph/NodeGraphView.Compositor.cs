using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using SamplesCommon;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            public CompositionBrush CompositionEffectBrush;
        }
        
        public void InitializeCompositor(UIElement rootVisualElement)
        {
            _rootCompositor = ElementCompositionPreview.GetElementVisual(rootVisualElement).Compositor;
            _nodeVisuals = new Dictionary<string, NodeVisual>();
        }

        public void CreateNodeVisual(string nodeKey, UIElement parentRootOfVisual, EffectType effectType)
        {
            Panel panel = (Panel)parentRootOfVisual;
            var compositor = ElementCompositionPreview.GetElementVisual(parentRootOfVisual).Compositor;
            var nodeVisual = new NodeVisual()
            {
                ContainerVisual = compositor.CreateContainerVisual(),
                SpriteVisual = compositor.CreateSpriteVisual()
            };

            //todo : get input slots sources and put into array to build the effect
            nodeVisual.CompositionEffectBrush = GetGraphicsEffect(compositor, effectType, null);
           
            nodeVisual.ContainerVisual.Children.InsertAtTop(nodeVisual.SpriteVisual);
            _nodeVisuals.Add(nodeKey, nodeVisual);
        }

        private CompositionBrush GetGraphicsEffect(Compositor compositor, EffectType effectType, object[] inputSlotSources) {

            switch (effectType) {
                case EffectType.NoEffect:
                    Size imageSize;
                    var brushNoEffect = CreateBrushFromAsset(compositor, (string)inputSlotSources[0], out imageSize);
                    var imageAspectRatio = (imageSize.Width == 0 && imageSize.Height == 0) ? 1 : imageSize.Width / imageSize.Height;
                    return brushNoEffect;
                case EffectType.AlphaMask:
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
            }

            return null;
        }

        private CompositionSurfaceBrush CreateBrushFromAsset(Compositor compositor, string name, out Size size)
        {
            ImageLoader.Initialize(compositor);
            CompositionDrawingSurface surface = ImageLoader.Instance.LoadFromUri(new Uri("ms-appx:///Assets/" + name)).Surface;
            size = surface.Size;
            return compositor.CreateSurfaceBrush(surface);
        }
    }
}
