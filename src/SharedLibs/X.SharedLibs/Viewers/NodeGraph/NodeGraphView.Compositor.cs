using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace X.Viewer.NodeGraph
{
    public partial class NodeGraphView : INodeGraphCompositor
    {
        IDictionary<string, NodeVisual> _nodeVisuals;
                
        public void InitializeCompositor(UIElement rootVisualElement)
        {
            _nodeVisuals = new Dictionary<string, NodeVisual>();
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
        
        public void UpdateNodeVisual(NodeNodeLinkModel nodeNodeLinkModel, UIElement parentRootOfVisual)
        {
            var nodeTypeInt = (int)nodeNodeLinkModel.Node.NodeType;
            if (nodeTypeInt > 100 && nodeTypeInt <= 1000)
            { //EFFECT NODES 
                var nodeVisual = _nodeVisuals[nodeNodeLinkModel.Node.Key];
                if (nodeVisual != null)
                {
                    var compositor = ElementCompositionPreview.GetElementVisual(nodeVisual.AssociatedObject).Compositor;
                    var effectType = (NodeType)nodeNodeLinkModel.Node.NodeType;
                    switch (effectType)
                    {
                        case NodeType.TextureAsset:
                        case NodeType.BlendEffect:
                        case NodeType.SepiaEffect:
                            var spriteVisualFound = ElementCompositionPreview.GetElementChildVisual(nodeVisual.AssociatedObject) as SpriteVisual;
                            if (spriteVisualFound != null) ClearNodeVisual(nodeVisual);

                            if (effectType == NodeType.TextureAsset)
                            {
                                nodeVisual.Brush = CreateGraphicsBrush(compositor, effectType, nodeNodeLinkModel.InputNodeLinks.ToArray());
                            }
                            else if (effectType == NodeType.BlendEffect)
                            {
                                var inputSlotSources = nodeNodeLinkModel.InputNodeLinks.ToArray();
                                if (inputSlotSources.Length < 3) nodeVisual.Brush = null;
                                var nl = inputSlotSources[2];
                                int.TryParse(nl.Value1, out int bl);
                                var blendEffectDesc = new BlendEffect
                                {
                                    Mode = (BlendEffectMode)Enum.Parse(typeof(BlendEffectMode), bl.ToString()),
                                    Background = new CompositionEffectSourceParameter("Background"),
                                    Foreground = new CompositionEffectSourceParameter("Foreground")
                                };
                                try
                                {
                                    var blendEffectBrush = compositor.CreateEffectFactory(blendEffectDesc).CreateBrush();
                                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, blendEffectBrush);
                                    nodeVisual.Brush = blendEffectBrush;
                                }
                                catch { }
                            }
                            else if (effectType == NodeType.SepiaEffect)
                            {
                                var inputSlotSources = nodeNodeLinkModel.InputNodeLinks.ToArray();
                                if (inputSlotSources.Length < 3) nodeVisual.Brush = null;
                                var nl = inputSlotSources[2];
                                int.TryParse(nl.Value1, out int am);
                                var sepiaEffectDesc = new SepiaEffect
                                {
                                    Name = "effect",
                                    AlphaMode = (CanvasAlphaMode)Enum.Parse(typeof(CanvasAlphaMode), am.ToString()),
                                    Source = new CompositionEffectSourceParameter("Image")
                                };
                                try
                                {
                                    var sepiaEffectBrush = compositor.CreateEffectFactory(sepiaEffectDesc, new[] { "effect.Intensity" }).CreateBrush();
                                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, sepiaEffectBrush);
                                    nodeVisual.Brush = sepiaEffectBrush;
                                }
                                catch { }
                            }

                            ResizeSpriteBrush(nodeVisual.AssociatedObject.ActualWidth - 20, nodeVisual.AssociatedObject.ActualHeight - 20, spriteVisualFound);
                            spriteVisualFound.Brush = nodeVisual.Brush;
                            break;
                        default:
                            UpdateGraphicsBrush(compositor, effectType, nodeNodeLinkModel.InputNodeLinks.ToArray(), nodeVisual.Brush);
                            break;
                    }
                }
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
                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, brushAlphaMask);
                    return brushAlphaMask;
                case NodeType.ArithmeticEffect:
                    if (inputSlotSources.Length < 6) return null;
                    var arithmeticEffectDesc = new ArithmeticCompositeEffect
                    {
                        Name = "effect",
                        ClampOutput = false,
                        Source1 = new CompositionEffectSourceParameter("Source1"),
                        Source2 = new CompositionEffectSourceParameter("Source2")
                    };
                    var arithmeticEffectBrush = compositor.CreateEffectFactory(
                        arithmeticEffectDesc,
                        new[]
                        {
                            "effect.MultiplyAmount",
                            "effect.Source1Amount",
                            "effect.Source2Amount",
                            "effect.Offset"
                        }
                    ).CreateBrush();
                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, arithmeticEffectBrush);
                    return arithmeticEffectBrush;
                case NodeType.BlendEffect:
                    if (inputSlotSources.Length < 3) return null;
                    var nl = (NodeLink)inputSlotSources[2];
                    int bl = 0; int.TryParse(nl.Value1, out bl);
                    var blendEffectDesc = new BlendEffect
                    {
                        Mode = (BlendEffectMode)Enum.Parse(typeof(BlendEffectMode), bl.ToString()),
                        Background = new CompositionEffectSourceParameter("Background"),
                        Foreground = new CompositionEffectSourceParameter("Foreground")
                    };
                    try {
                        var blendEffectBrush = compositor.CreateEffectFactory(blendEffectDesc).CreateBrush();
                        UpdateGraphicsBrush(compositor, effectType, inputSlotSources, blendEffectBrush);
                        return blendEffectBrush;
                    }
                    catch { }
                    return null;
                case NodeType.ColorSourceEffect:
                    if (inputSlotSources.Length < 4) return null;
                    var colorSourceEffectDesc = new ColorSourceEffect // FloodEffect
                    {
                        Name = "effect"
                    };
                    var colorSourceEffectBrush = compositor.CreateEffectFactory(
                        colorSourceEffectDesc,
                        new[] { "effect.Color" }
                    ).CreateBrush();
                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, colorSourceEffectBrush);
                    return colorSourceEffectBrush;
                case NodeType.ContrastEffect:
                    // Changes the contrast of an image.
                    if (inputSlotSources.Length == 0) return null;
                    var contrastEffectDesc = new ContrastEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var brushContrastEffect = compositor.CreateEffectFactory(
                        contrastEffectDesc,
                        new[] { "effect.Contrast" }
                    ).CreateBrush();
                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, brushContrastEffect);
                    return brushContrastEffect;
                case NodeType.ExposureEffect:
                    var exposureEffectDesc = new ExposureEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var exposureEffectBrush = compositor.CreateEffectFactory(
                        exposureEffectDesc,
                        new[] { "effect.Exposure" }
                    ).CreateBrush();
                    return exposureEffectBrush;
                case NodeType.GrayscaleEffect:
                    if (inputSlotSources.Length == 0) return null;
                    var grayscaleEffectDesc = new GrayscaleEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var brushGrayscale = compositor.CreateEffectFactory(
                        grayscaleEffectDesc
                    ).CreateBrush();
                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, brushGrayscale);
                    return brushGrayscale;
                case NodeType.GammaTransferEffect:
                    var gammaTransferEffectDesc = new GammaTransferEffect
                    {
                        Name = "effect",
                        RedDisable = false,
                        GreenDisable = false,
                        BlueDisable = false,
                        AlphaDisable = false,
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var gammaTransferEffectBrush = compositor.CreateEffectFactory(
                        gammaTransferEffectDesc,
                        new[]
                        {
                            "effect.RedAmplitude",
                            "effect.RedExponent",
                            "effect.RedOffset",
                            "effect.GreenAmplitude",
                            "effect.GreenExponent",
                            "effect.GreenOffset",
                            "effect.BlueAmplitude",
                            "effect.BlueExponent",
                            "effect.BlueOffset",
                            "effect.AlphaAmplitude",
                            "effect.AlphaExponent",
                            "effect.AlphaOffset"
                        }
                    ).CreateBrush();
                    
                    return gammaTransferEffectBrush;
                case NodeType.HueRotationEffect:
                    if (inputSlotSources.Length == 0) return null;
                    var hueRotationEffectDesc = new HueRotationEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var brushHueRotationEffect = compositor.CreateEffectFactory(
                        hueRotationEffectDesc,
                        new[] { "effect.Angle" }
                    ).CreateBrush();
                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, brushHueRotationEffect);
                    return brushHueRotationEffect;
                case NodeType.InvertEffect:
                    var invertEffectDesc = new InvertEffect
                    {
                        Name = "effect",
                        Source = new CompositionEffectSourceParameter("Image")
                    };
                    var invertEffectBrush = compositor.CreateEffectFactory(invertEffectDesc).CreateBrush();
                    UpdateGraphicsBrush(compositor, effectType, inputSlotSources, invertEffectBrush);
                    return invertEffectBrush;
                case NodeType.SepiaEffect:
                    if (inputSlotSources.Length < 3) return null;
                    var nlam = (NodeLink)inputSlotSources[2];
                    int am = 0; int.TryParse(nlam.Value1, out am);
                    var sepiaEffectDesc = new SepiaEffect
                    {
                      Name = "effect",
                      AlphaMode = (CanvasAlphaMode)Enum.Parse(typeof(CanvasAlphaMode), am.ToString()),
                      Source = new CompositionEffectSourceParameter("Image")
                    };
                    var sepiaEffectBrush = compositor.CreateEffectFactory(
                        sepiaEffectDesc,
                        new[] { "effect.Intensity" }
                    ).CreateBrush();
                    return sepiaEffectBrush;
                default:
                    throw new NotImplementedException();
            }
        }


        private void UpdateGraphicsBrush(Compositor compositor, NodeType effectType, object[] inputSlotSources, CompositionBrush brushToUpdate)
        {
            switch (effectType)
            {
                case NodeType.TextureAsset:
                    //done a layer above as too late here ????
                    return;
                case NodeType.AlphaMaskEffect:
                    var alphaMaskEffectBrush = (CompositionEffectBrush)brushToUpdate;
                    alphaMaskEffectBrush.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                    alphaMaskEffectBrush.SetSourceParameter("Mask", _nodeVisuals[((NodeLink)inputSlotSources[1]).OutputNodeKey].Brush);
                    return;
                case NodeType.ArithmeticEffect:
                    try
                    {
                        var arithmeticEffectBrush = (CompositionEffectBrush)brushToUpdate;
                        arithmeticEffectBrush.SetSourceParameter("Source1", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                        arithmeticEffectBrush.SetSourceParameter("Source2", _nodeVisuals[((NodeLink)inputSlotSources[2]).OutputNodeKey].Brush);

                        var multiplyAmountValue = ((NodeLink)inputSlotSources[4]).Value1;
                        var source1AmountValue = ((NodeLink)inputSlotSources[1]).Value1;
                        var source2AmountValue = ((NodeLink)inputSlotSources[3]).Value1;
                        var offsetValue = ((NodeLink)inputSlotSources[5]).Value1;
                        if (string.IsNullOrEmpty(multiplyAmountValue)) multiplyAmountValue = "1";
                        if (string.IsNullOrEmpty(source1AmountValue)) source1AmountValue = "0";
                        if (string.IsNullOrEmpty(source2AmountValue)) source2AmountValue = "0";
                        if (string.IsNullOrEmpty(offsetValue)) offsetValue = "0";
                        arithmeticEffectBrush.Properties.InsertScalar("effect.MultiplyAmount", (float)(float.Parse(multiplyAmountValue)));
                        arithmeticEffectBrush.Properties.InsertScalar("effect.Source1Amount", (float)(float.Parse(source1AmountValue)));
                        arithmeticEffectBrush.Properties.InsertScalar("effect.Source2Amount", (float)(float.Parse(source2AmountValue)));
                        arithmeticEffectBrush.Properties.InsertScalar("effect.Offset", (float)(float.Parse(offsetValue)));
                    }
                    catch { };
                    
                    return;
                case NodeType.BlendEffect:
                    try {
                        var blendEffectBrush = (CompositionEffectBrush)brushToUpdate;
                        blendEffectBrush.SetSourceParameter("Background", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                        blendEffectBrush.SetSourceParameter("Foreground", _nodeVisuals[((NodeLink)inputSlotSources[1]).OutputNodeKey].Brush);
                    } catch { }
                    
                    return;
                case NodeType.ColorSourceEffect:
                    var colorSourceEffectBrush = (CompositionEffectBrush)brushToUpdate;
                    var redValue = ((NodeLink)inputSlotSources[0]).Value1;
                    var greenValue = ((NodeLink)inputSlotSources[1]).Value1;
                    var blueValue = ((NodeLink)inputSlotSources[2]).Value1;
                    var alphaValue = ((NodeLink)inputSlotSources[3]).Value1;

                    if (string.IsNullOrEmpty(redValue)) redValue = "0";
                    if (string.IsNullOrEmpty(greenValue)) greenValue = "0";
                    if (string.IsNullOrEmpty(blueValue)) blueValue = "0";
                    if (string.IsNullOrEmpty(alphaValue)) alphaValue = "0";
                    
                    var newColor = new Color();
                    newColor.R = (byte)(int.Parse(redValue));
                    newColor.G = (byte)(int.Parse(greenValue));
                    newColor.B = (byte)(int.Parse(blueValue));
                    newColor.A = (byte)(int.Parse(alphaValue));

                    colorSourceEffectBrush.Properties.InsertColor( "effect.Color", newColor);

                    return;
                case NodeType.ContrastEffect:
                    var brushContrastEffect = (CompositionEffectBrush)brushToUpdate;
                    brushContrastEffect.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                    var contrastValue = ((NodeLink)inputSlotSources[1]).Value1;
                    if (string.IsNullOrEmpty(contrastValue)) contrastValue = "0";
                    brushContrastEffect.Properties.InsertScalar("effect.Contrast", (float)(float.Parse(contrastValue)));
                    return;
                case NodeType.ExposureEffect:
                    var exposureEffectBrush = (CompositionEffectBrush)brushToUpdate;
                    exposureEffectBrush.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);

                    var exposureValue = ((NodeLink)inputSlotSources[1]).Value1;
                    if (string.IsNullOrEmpty(exposureValue)) exposureValue = "0";
                    exposureEffectBrush.Properties.InsertScalar("effect.Exposure", (float)(float.Parse(exposureValue)));
                    return;
                case NodeType.GrayscaleEffect:
                    var brushGrayscale = (CompositionEffectBrush)brushToUpdate;
                    brushGrayscale.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                    return;
                case NodeType.GammaTransferEffect:
                    var gammaTransferEffectBrush = (CompositionEffectBrush)brushToUpdate;
                    gammaTransferEffectBrush.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);


                    var ramp = ((NodeLink)inputSlotSources[1]).Value1;
                    var rexp = ((NodeLink)inputSlotSources[2]).Value1;
                    var rofs = ((NodeLink)inputSlotSources[3]).Value1;
                    var gamp = ((NodeLink)inputSlotSources[4]).Value1;
                    var gexp = ((NodeLink)inputSlotSources[5]).Value1;
                    var gofs = ((NodeLink)inputSlotSources[6]).Value1;
                    var bamp = ((NodeLink)inputSlotSources[7]).Value1;
                    var bexp = ((NodeLink)inputSlotSources[8]).Value1;
                    var bofs = ((NodeLink)inputSlotSources[9]).Value1;
                    var aamp = ((NodeLink)inputSlotSources[10]).Value1;
                    var aexp = ((NodeLink)inputSlotSources[11]).Value1;
                    var aofs = ((NodeLink)inputSlotSources[12]).Value1;

                    if (string.IsNullOrEmpty(ramp)) ramp = "1";
                    if (string.IsNullOrEmpty(rexp)) rexp = "1";
                    if (string.IsNullOrEmpty(rofs)) rofs = "0";
                    if (string.IsNullOrEmpty(gamp)) gamp = "1";
                    if (string.IsNullOrEmpty(gexp)) gexp = "1";
                    if (string.IsNullOrEmpty(gofs)) gofs = "0";
                    if (string.IsNullOrEmpty(bamp)) bamp = "1";
                    if (string.IsNullOrEmpty(bexp)) bexp = "1";
                    if (string.IsNullOrEmpty(bofs)) bofs = "0";
                    if (string.IsNullOrEmpty(aamp)) aamp = "1";
                    if (string.IsNullOrEmpty(aexp)) aexp = "1";
                    if (string.IsNullOrEmpty(aofs)) aofs = "0";

                    gammaTransferEffectBrush.Properties.InsertScalar("effect.RedAmplitude", float.Parse(ramp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.RedExponent", float.Parse(rexp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.RedOffset", float.Parse(rofs));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.GreenAmplitude", float.Parse(gamp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.GreenExponent", float.Parse(gexp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.GreenOffset", float.Parse(gofs));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.BlueAmplitude", float.Parse(bamp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.BlueExponent", float.Parse(bexp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.BlueOffset", float.Parse(bofs));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.AlphaAmplitude", float.Parse(aamp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.AlphaExponent", float.Parse(aexp));
                    gammaTransferEffectBrush.Properties.InsertScalar("effect.AlphaOffset", float.Parse(aofs));

                    return;
                case NodeType.HueRotationEffect:
                    var brushHueRotationEffect = (CompositionEffectBrush)brushToUpdate;
                    brushHueRotationEffect.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);
                    var hueEffectAngle = ((NodeLink)inputSlotSources[1]).Value1;
                    if (string.IsNullOrEmpty(hueEffectAngle)) hueEffectAngle = "0";
                    brushHueRotationEffect.Properties.InsertScalar("effect.Angle", (float)(float.Parse(hueEffectAngle) * Math.PI * 2));
                    return;
                case NodeType.InvertEffect:
                    var invertEffectBrush = (CompositionEffectBrush)brushToUpdate;
                    //invertEffectBrush.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);  <-- causing a crash on 18305
                    return;
                case NodeType.SepiaEffect:
                    var sepiaEffectBrushe = (CompositionEffectBrush)brushToUpdate;
                    sepiaEffectBrushe.SetSourceParameter("Image", _nodeVisuals[((NodeLink)inputSlotSources[0]).OutputNodeKey].Brush);

                    var sepiaIntensityValue = ((NodeLink)inputSlotSources[1]).Value1;
                    if (string.IsNullOrEmpty(sepiaIntensityValue)) hueEffectAngle = "0.5";
                    sepiaEffectBrushe.Properties.InsertScalar("effect.Intensity", float.Parse(sepiaIntensityValue));
                    return;
                default:
                    throw new NotImplementedException();
            }
        }



        private CompositionSurfaceBrush CreateBrushFromAsset(Compositor compositor, string name)
        {
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
            if (spriteVisual?.Brush is CompositionEffectBrush brush)
            {
                spriteVisual.Brush = null;
            }

            nodeVisual.Brush?.Dispose();
            nodeVisual.Brush = null;
        }
    }
}


// d2d supported effects https://docs.microsoft.com/en-us/windows/desktop/direct2d/built-in-effects
// win2d supported effects https://microsoft.github.io/Win2D/html/N_Microsoft_Graphics_Canvas_Effects.htm