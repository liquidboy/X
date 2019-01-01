using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using SamplesCommon;
using System;
using System.Collections.Generic;
using System.Numerics;
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
            public CompositionBrush Brush;
        }
        
        public void InitializeCompositor(UIElement rootVisualElement)
        {
            _rootCompositor = ElementCompositionPreview.GetElementVisual(rootVisualElement).Compositor;
            _nodeVisuals = new Dictionary<string, NodeVisual>();
        }

        public void CreateNodeVisual(string nodeKey, UIElement parentRootOfVisual, EffectType effectType)
        {
            FrameworkElement fe = (FrameworkElement)parentRootOfVisual;
            var compositor = ElementCompositionPreview.GetElementVisual(parentRootOfVisual).Compositor;
            var nodeVisual = new NodeVisual()
            {
                ContainerVisual = compositor.CreateContainerVisual(),
                SpriteVisual = compositor.CreateSpriteVisual()
            };
            ElementCompositionPreview.SetElementChildVisual(parentRootOfVisual, nodeVisual.ContainerVisual);

            //todo : get input slots sources and put into array to build the effect
            object[] sources = new object[] { "xxx.jpg" };
            nodeVisual.Brush = GetGraphicsEffect(compositor, effectType, sources);
            ResizeSpriteBrush(fe.ActualWidth, fe.ActualHeight, nodeVisual.SpriteVisual, 1);
            nodeVisual.SpriteVisual.Brush = nodeVisual.Brush;
            nodeVisual.ContainerVisual.Children.InsertAtTop(nodeVisual.SpriteVisual);



            _nodeVisuals.Add(nodeKey, nodeVisual);
        }

        private void ResizeSpriteBrush(double visibleWidth, double visibleHeight, SpriteVisual spriteVisual, double imageAspectRatio)
        {
            double newWidth = visibleWidth;
            double newHeight = visibleHeight;

            //newWidth = newHeight * imageAspectRatio;
            //if (newWidth > visibleWidth)
            //{
            //    newWidth = visibleWidth;
            //    newHeight = newWidth / imageAspectRatio;
            //}

            spriteVisual.Offset = new Vector3(0f, 0f, 0.0f);
            spriteVisual.Size = new Vector2((float)newWidth, (float)newHeight);
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

        public void ClearCompositor()
        {
            foreach (var nodeVisual in _nodeVisuals) {
                nodeVisual.Value.ContainerVisual.Dispose();
                nodeVisual.Value.ContainerVisual = null; 
                nodeVisual.Value.SpriteVisual.Dispose();
                nodeVisual.Value.SpriteVisual = null;
                nodeVisual.Value.Brush.Dispose();
                nodeVisual.Value.Brush = null;
                _nodeVisuals.Remove(nodeVisual.Key);
            }
            _nodeVisuals.Clear();
        }
    }
}
