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
                
        public void InitializeCompositor(UIElement rootVisualElement)
        {
            _rootCompositor = ElementCompositionPreview.GetElementVisual(rootVisualElement).Compositor;
            _nodeVisuals = new Dictionary<string, NodeVisual>();
        }

        public void UpdateNodeVisual(NodeNodeLinkModel nodeNodeLinkModel, UIElement parentRootOfVisual) {
            var nodeTypeInt = (int)nodeNodeLinkModel.Node.NodeType;
            if (nodeTypeInt > 100 && nodeTypeInt <= 1000) { //EFFECT NODES 
                var nodeVisual = _nodeVisuals[nodeNodeLinkModel.Node.Key];
                if (nodeVisual != null) {
                    var compositor = ElementCompositionPreview.GetElementVisual(nodeVisual.AssociatedObject).Compositor;
                    var effectType = (NodeType)nodeNodeLinkModel.Node.NodeType;
                    switch (effectType)
                    {
                        case NodeType.TextureAsset:
                            var spriteVisualFound = ElementCompositionPreview.GetElementChildVisual(nodeVisual.AssociatedObject) as SpriteVisual;
                            if (spriteVisualFound != null) ClearNodeVisual(nodeVisual);
                            nodeVisual.Brush = CreateGraphicsBrush(compositor, effectType, nodeNodeLinkModel.InputNodeLinks.ToArray());
                            ResizeSpriteBrush(nodeVisual.AssociatedObject.ActualWidth - 20, nodeVisual.AssociatedObject.ActualHeight - 20, spriteVisualFound);
                            spriteVisualFound.Brush = nodeVisual.Brush;
                            break;
                        default:
                            localUpdateGraphicsBrush(compositor, effectType, nodeNodeLinkModel.InputNodeLinks.ToArray(), nodeVisual.Brush);
                            break;
                    }
                }
            }
        }


        public void CreateNodeVisual(NodeNodeLinkModel nodeNodeLinkModel, UIElement parentRootOfVisual, NodeType nodeType)
        {
            var nodeVisual = new NodeVisual();
            var nodeTypeInt = (int)nodeType;
            nodeVisual.AssociatedObject = (FrameworkElement)parentRootOfVisual;
            
            var nodeVisuals = new Dictionary<string, NodeVisual>();
            

            if (nodeTypeInt > 100 && nodeTypeInt <= 1000) { //EFFECT NODES
                var compositor = ElementCompositionPreview.GetElementVisual(nodeVisual.AssociatedObject).Compositor;

                // check to see if the visual already has an effect applied.
                var spriteVisualFound = ElementCompositionPreview.GetElementChildVisual(nodeVisual.AssociatedObject) as SpriteVisual;
                var foundBrush = spriteVisualFound?.Brush as CompositionEffectBrush;
                if (foundBrush == null)
                {
                    var spriteVisualNew = compositor.CreateSpriteVisual();

                    ElementCompositionPreview.SetElementChildVisual(nodeVisual.AssociatedObject, spriteVisualNew);

                    //get input sources and pass them to effect graph 
                    nodeVisual.Brush = CreateGraphicsBrush(compositor, nodeType, nodeNodeLinkModel.InputNodeLinks.ToArray());

                    ResizeSpriteBrush(nodeVisual.AssociatedObject.ActualWidth - 20, nodeVisual.AssociatedObject.ActualHeight - 20, spriteVisualNew);
                    spriteVisualNew.Brush = nodeVisual.Brush;
                }
                    
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
                    var assetName = ((NodeLink)inputSlotSources[0]).Value1;
                    if (string.IsNullOrEmpty(assetName)) return null;
                    return CreateBrushFromAsset(compositor, assetName);
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
                    localUpdateGraphicsBrush(compositor, effectType, inputSlotSources, brushAlphaMask);
                    return brushAlphaMask;
                case NodeType.GrayscaleEffect:
                    var grayscaleEffectDesc = new GrayscaleEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var brushGrayscale = compositor.CreateEffectFactory(
                        grayscaleEffectDesc
                    ).CreateBrush();
                    localUpdateGraphicsBrush(compositor, effectType, inputSlotSources, brushGrayscale);
                    return brushGrayscale;
                case NodeType.HueRotationEffect:
                    var hueRotationEffectDesc = new HueRotationEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var brushHueRotationEffect = compositor.CreateEffectFactory(
                        hueRotationEffectDesc,
                        new[] { "effect.Angle" }
                    ).CreateBrush();
                    localUpdateGraphicsBrush(compositor, effectType, inputSlotSources, brushHueRotationEffect);
                    return brushHueRotationEffect;
                default:
                    throw new NotImplementedException();
            }
        }


        private void localUpdateGraphicsBrush(Compositor compositor, NodeType effectType, object[] inputSlotSources, CompositionBrush brushToUpdate)
        {
            switch (effectType)
            {
                case NodeType.TextureAsset:
                    //var assetName = ((NodeLink)inputSlotSources[0]).Value1;
                    //if (string.IsNullOrEmpty(assetName)) return;
                    //brushToUpdate = CreateBrushFromAsset(compositor, assetName);
                    return;
                case NodeType.AlphaMaskEffect:
                    var alphaMaskEffectBrush = (CompositionEffectBrush)brushToUpdate;
                    alphaMaskEffectBrush.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                    alphaMaskEffectBrush.SetSourceParameter("Mask", _nodeVisuals[((NodeLink)inputSlotSources[1]).OutputNodeKey].Brush);
                    return;
                case NodeType.GrayscaleEffect:
                    var brushGrayscale = (CompositionEffectBrush)brushToUpdate;
                    brushGrayscale.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                    return;
                case NodeType.HueRotationEffect:
                    var hueRotationEffectDesc = new HueRotationEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var brushHueRotationEffect = (CompositionEffectBrush)brushToUpdate;
                    brushHueRotationEffect.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                    var hueEffectAngle = ((NodeLink)inputSlotSources[1]).Value1;
                    if (string.IsNullOrEmpty(hueEffectAngle)) hueEffectAngle = "0";
                    brushHueRotationEffect.Properties.InsertScalar("effect.Angle", (float)(float.Parse(hueEffectAngle) * Math.PI * 2));
                    return;
                default:
                    throw new NotImplementedException();
            }
        }



        private CompositionSurfaceBrush CreateBrushFromAsset(Compositor compositor, string name)
        {
            //ImageLoader.Initialize(compositor);
            //CompositionDrawingSurface surface = ImageLoader.Instance.LoadFromUri(new Uri("ms-appx:///Assets/" + name)).Surface;
            try {
                SurfaceLoader.Initialize(compositor);
                var task = Task.Run(() => SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Assets/" + name)));
                task.Wait();
                var surface = task.Result;

                return compositor.CreateSurfaceBrush(surface);
            }
            catch { return null; }
            
        }

        public void ClearCompositor()
        {
            foreach (var nodeVisual in _nodeVisuals)
            {
                ClearNodeVisual(nodeVisual.Value);
            }
            _nodeVisuals.Clear();

            SurfaceLoader.Uninitialize();
        }

        private void ClearNodeVisual(NodeVisual nodeVisual) {
            var spriteVisual = ElementCompositionPreview.GetElementChildVisual(nodeVisual.AssociatedObject) as SpriteVisual;
            var brush = spriteVisual?.Brush as CompositionEffectBrush;
            if (brush != null)
            {
                spriteVisual.Brush = null;
            }

            nodeVisual.Brush?.Dispose();
            nodeVisual.Brush = null;
        }
    }
}
