using System;
using System.Collections.Generic;
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
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.Xaml.Hosting;



namespace X.UI.Composition
{


    public sealed partial class LightPanel : UserControl
    {
        private Compositor _compositor;
        private CompositionEffectFactory _effectFactory;
        private CompositionSurfaceBrush _flatNormalsBrush;
        private CompositionSurfaceBrush _circleNormalsBrush;
        private AmbientLight _ambientLight;
        private PointLight _pointLight;
        private DistantLight _distantLight;
        private SpotLight _spotLight;



        public LightingTypes SelectedLight
        {
            get { return (LightingTypes)GetValue(SelectedLightProperty); }
            set { SetValue(SelectedLightProperty, value); }
        }

        public static readonly DependencyProperty SelectedLightProperty = DependencyProperty.Register("SelectedLight", typeof(LightingTypes), typeof(LightPanel), new PropertyMetadata(LightingTypes.PointDiffuse));



        public FrameworkElement Content
        {
            get { return (FrameworkElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(FrameworkElement), typeof(LightPanel), new PropertyMetadata(null));




        public enum LightingTypes
        {
            PointDiffuse,
            PointSpecular,
            SpotLightDiffuse,
            SpotLightSpecular,
            DistantDiffuse,
            DistantSpecular,
        }

        public LightPanel()
        {
            this.InitializeComponent();

            this.Loaded += LightPanel_Loaded;
            this.Unloaded += LightPanel_Unloaded;

            // Get the current compositor
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            SurfaceLoader.Initialize(_compositor);
            
            //
            // Create the lights
            //
            _ambientLight = _compositor.CreateAmbientLight();
            _pointLight = _compositor.CreatePointLight();
            _distantLight = _compositor.CreateDistantLight();
            _spotLight = _compositor.CreateSpotLight();
        }

        private void LightPanel_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private async void LightPanel_Loaded(object sender, RoutedEventArgs e)
        {
            await InitControl();

            UpdateLightingEffect();
        }

     
        private async Task InitControl()
        {

            //
            // Create the sperical normal map.  The normals will give the appearance of a sphere, and the alpha channel is used
            // for masking off the rectangular edges.
            //
            //CompositionDrawingSurface normalMap = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///X.UI.Composition.Assets/SphericalWithMask.png"));
            CompositionDrawingSurface normalMap = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///X.UI.Composition.Assets/BeveledEdges.jpg"));
            _circleNormalsBrush = _compositor.CreateSurfaceBrush(normalMap);
            _circleNormalsBrush.Stretch = CompositionStretch.Fill;


            // 
            // Create the flat normal map with beveled edges.  This should give the appearance of slanting of the surface along
            // the edges, flat in the middle.
            //

            normalMap = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///X.UI.Composition.Assets/BeveledEdges.jpg"));
            _flatNormalsBrush = _compositor.CreateSurfaceBrush(normalMap);
            _flatNormalsBrush.Stretch = CompositionStretch.Fill;

            // Update the effect brushes now that the normal maps are available.
            UpdateEffectBrush();
        }

        public void Redraw() {
            UpdateLightingEffect();
            UpdateEffectBrush();
        }

        private void UpdateEffectBrush()
        {
            if (ccMain.Content != null && ccMain.Content is CompositionImage)
            {
                //foreach (ListViewItem item in gvMain.ItemsPanelRoot.Children)
                //{
                //  CompositionImage image = item.ContentTemplateRoot.GetFirstDescendantOfType<CompositionImage>();
                    CompositionImage image = (CompositionImage)ccMain.Content;
                    //var imgs = item.ContentTemplateRoot.GetDescendantsOfType<CompositionImage>();
                    //CompositionImage image = imgs.Last();
                    SetImageEffect(image);
                //}
            }
        }




        //LightingTypes _selectedLight = LightingTypes.PointDiffuse;
        private void SetImageEffect(CompositionImage image)
        {
            if (_effectFactory == null) return;

            // Create the effect brush and bind the normal map
            CompositionEffectBrush brush = _effectFactory.CreateBrush();

            //ComboBoxItem item = LightingSelection.SelectedValue as ComboBoxItem;
            //switch ((LightingTypes)item.Tag)
            switch (SelectedLight)
            {
                case LightingTypes.SpotLightSpecular:
                case LightingTypes.PointSpecular:
                case LightingTypes.DistantDiffuse:
                case LightingTypes.DistantSpecular:
                    brush.SetSourceParameter("NormalMap", _circleNormalsBrush);
                    break;
                default:
                    brush.SetSourceParameter("NormalMap", _flatNormalsBrush);
                    break;
            }

            // Update the CompositionImage to use the custom effect brush
            image.Brush = brush;
        }

        public void UpdateLightingEffect_PointDiffuse(float AmbientAmount = 0, float DiffuseAmount = .75f, 
            float SpecularAmount = 0, string NormalMapSource = "NormalMap", bool forceUpdate = false) {
            
            var sceneLightingEffect = new SceneLightingEffect()
            {
                AmbientAmount = AmbientAmount,
                DiffuseAmount = DiffuseAmount,
                SpecularAmount = SpecularAmount,
                NormalMapSource = new CompositionEffectSourceParameter(NormalMapSource),
            };

            IGraphicsEffect graphicsEffect = new CompositeEffect()
            {
                Mode = CanvasComposite.Add,
                Sources =
                    {
                        new CompositionEffectSourceParameter("ImageSource"),
                        sceneLightingEffect
                    }
            };

            _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);


            if (forceUpdate) UpdateEffectBrush();
        }

        public void UpdateLightingEffect_PointSpecular(float AmbientAmount1 = 0.6f, float DiffuseAmount1 = 1f,
            float SpecularAmount1 = 0, string NormalMapSource1 = "NormalMap", float AmbientAmount2 = 0, 
            float DiffuseAmount2 = 0f, float SpecularAmount2 = 1, float SpecularShine2 = 100f, 
            string NormalMapSource2 = "NormalMap", bool forceUpdate = false)
        {

            IGraphicsEffect graphicsEffect = new CompositeEffect()
            {
                Mode = CanvasComposite.DestinationIn,
                Sources =
                            {
                                new ArithmeticCompositeEffect()
                                {
                                    Source1Amount = 1,
                                    Source2Amount = 1,
                                    MultiplyAmount = 0,

                                    Source1 = new ArithmeticCompositeEffect()
                                    {
                                        MultiplyAmount = 1,
                                        Source1Amount = 0,
                                        Source2Amount = 0,
                                        Source1 = new CompositionEffectSourceParameter("ImageSource"),
                                        Source2 = new SceneLightingEffect()
                                        {
                                            AmbientAmount = AmbientAmount1,
                                            DiffuseAmount = DiffuseAmount1,
                                            SpecularAmount = SpecularAmount1,
                                            NormalMapSource = new CompositionEffectSourceParameter(NormalMapSource1),
                                        }
                                    },
                                    Source2 = new SceneLightingEffect()
                                    {
                                        AmbientAmount = AmbientAmount2,
                                        DiffuseAmount = DiffuseAmount2,
                                        SpecularAmount = SpecularAmount2,
                                        SpecularShine = SpecularShine2,
                                        NormalMapSource = new CompositionEffectSourceParameter(NormalMapSource2),
                                    }
                                },
                                new CompositionEffectSourceParameter("NormalMap"),
                            }
            };

            _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);


            if (forceUpdate) UpdateEffectBrush();
        }

        public void UpdateLightingEffect_SpotLightDiffuse(float AmbientAmount = 0, float DiffuseAmount = .75f,
            float SpecularAmount = 0, string NormalMapSource = "NormalMap", float InnerConeAngle = 15,
            float OuterConeAngle = 10, bool forceUpdate = false)
        {

            var sceneLightingEffect = new SceneLightingEffect()
            {
                AmbientAmount = AmbientAmount,
                DiffuseAmount = DiffuseAmount,
                SpecularAmount = SpecularAmount,
                NormalMapSource = new CompositionEffectSourceParameter(NormalMapSource),
            };

            IGraphicsEffect graphicsEffect = new CompositeEffect()
            {
                Mode = CanvasComposite.Add,
                Sources =
                {
                    new CompositionEffectSourceParameter("ImageSource"),
                    sceneLightingEffect
                }
            };

            _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

            _spotLight.InnerConeAngle = (float)(Math.PI / InnerConeAngle);
            _spotLight.OuterConeAngle = (float)(Math.PI / OuterConeAngle);

            if (forceUpdate) UpdateEffectBrush();
        }

        public void UpdateLightingEffect_SpotLightSpecular(float AmbientAmount1 = 0.6f, float DiffuseAmount1 = 1f,
            float SpecularAmount1 = 0, string NormalMapSource1 = "NormalMap", float AmbientAmount2 = 0,
            float DiffuseAmount2 = 0f, float SpecularAmount2 = 1, float SpecularShine2 = 100f,
            string NormalMapSource2 = "NormalMap", float InnerConeAngle = 15,
            float OuterConeAngle = 10, bool forceUpdate = false)
        {

            IGraphicsEffect graphicsEffect = new CompositeEffect()
            {
                Mode = CanvasComposite.DestinationIn,
                Sources =
                            {
                                new ArithmeticCompositeEffect()
                                {
                                    Source1Amount = 1,
                                    Source2Amount = 1,
                                    MultiplyAmount = 0,

                                    Source1 = new ArithmeticCompositeEffect()
                                    {
                                        MultiplyAmount = 1,
                                        Source1Amount = 0,
                                        Source2Amount = 0,
                                        Source1 = new CompositionEffectSourceParameter("ImageSource"),
                                        Source2 = new SceneLightingEffect()
                                        {
                                            AmbientAmount = AmbientAmount1,
                                            DiffuseAmount = DiffuseAmount1,
                                            SpecularAmount = SpecularAmount1,
                                            NormalMapSource = new CompositionEffectSourceParameter(NormalMapSource1),
                                        }
                                    },
                                    Source2 = new SceneLightingEffect()
                                    {
                                        AmbientAmount = AmbientAmount2,
                                        DiffuseAmount = DiffuseAmount2,
                                        SpecularAmount = SpecularAmount2,
                                        SpecularShine = SpecularShine2,
                                        NormalMapSource = new CompositionEffectSourceParameter(NormalMapSource2),
                                    }
                                },
                                new CompositionEffectSourceParameter("NormalMap"),
                            }
            };

            _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

            _spotLight.InnerConeAngle = (float)(Math.PI / InnerConeAngle);
            _spotLight.OuterConeAngle = (float)(Math.PI / OuterConeAngle);

            if (forceUpdate) UpdateEffectBrush();
        }


        private void UpdateLightingEffect()
        {
            if (_effectFactory != null) { 
                _effectFactory.Dispose();
                _effectFactory = null;
            }

            _ambientLight.Targets.RemoveAll();
            _pointLight.Targets.RemoveAll();
            _distantLight.Targets.RemoveAll();
            _spotLight.Targets.RemoveAll();

            //ComboBoxItem item = LightingSelection.SelectedValue as ComboBoxItem;
            //switch ((LightingTypes)item.Tag)
            switch (SelectedLight)
            {
                case LightingTypes.PointDiffuse:
                    {
                        //
                        // Result = Ambient +       Diffuse
                        // Result = (Image) + (.75 * Diffuse color)
                        //
                        UpdateLightingEffect_PointDiffuse();
                        
                        // Set the light coordinate space and add the target
                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _pointLight.CoordinateSpace = lightRoot;
                        _pointLight.Targets.Add(lightRoot);
                    }
                    break;

                case LightingTypes.PointSpecular:
                    {
                        //
                        // Result =    Ambient   +       Diffuse           +     Specular
                        // Result = (Image * .6) + (Image * Diffuse color) + (Specular color)
                        //
                        UpdateLightingEffect_PointSpecular();

                        // Set the light coordinate space and add the target
                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _ambientLight.Targets.Add(lightRoot);
                        _pointLight.CoordinateSpace = lightRoot;
                        _pointLight.Targets.Add(lightRoot);
                    }
                    break;

                case LightingTypes.SpotLightDiffuse:
                    {
                        //
                        // Result = Ambient +      Diffuse
                        // Result =  Image  + (Diffuse color * .75)
                        //
                        UpdateLightingEffect_SpotLightDiffuse();
                        

                        // Set the light coordinate space and add the target
                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _spotLight.CoordinateSpace = lightRoot;
                        _spotLight.Targets.Add(lightRoot);
                        _spotLight.Direction = new Vector3(0, 0, -1);
                    };
                    break;

                case LightingTypes.SpotLightSpecular:
                    {
                        //
                        // Result =    Ambient   +       Diffuse           +     Specular
                        // Result = (Image * .6) + (Image * Diffuse color) + (Specular color)
                        //

                        UpdateLightingEffect_SpotLightSpecular();
                        

                        // Set the light coordinate space and add the target
                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _ambientLight.Targets.Add(lightRoot);
                        _spotLight.CoordinateSpace = lightRoot;
                        _spotLight.Targets.Add(lightRoot);
                        
                        _spotLight.Direction = new Vector3(0, 0, -1);
                    };
                    break;

                case LightingTypes.DistantDiffuse:
                    {
                        //
                        // Result = Ambient +       Diffuse
                        // Result = (Image) + (.5 * Diffuse color)
                        //

                        IGraphicsEffect graphicsEffect = new CompositeEffect()
                        {
                            Mode = CanvasComposite.DestinationIn,
                            Sources =
                            {
                                new CompositeEffect()
                                {
                                    Mode = CanvasComposite.Add,
                                    Sources =
                                    {
                                        new CompositionEffectSourceParameter("ImageSource"),
                                        new SceneLightingEffect()
                                        {
                                            AmbientAmount = 0,
                                            DiffuseAmount = .5f,
                                            SpecularAmount = 0,
                                            NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
                                        }
                                    }
                                },
                                new CompositionEffectSourceParameter("NormalMap"),
                            }
                        };

                        _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _distantLight.CoordinateSpace = lightRoot;
                        _distantLight.Targets.Add(lightRoot);
                    };
                    break;

                case LightingTypes.DistantSpecular:
                    {
                        //
                        // Result =          Diffuse        +    Specular
                        // Result = (Image * Diffuse color) + (Specular color)
                        //

                        IGraphicsEffect graphicsEffect = new CompositeEffect()
                        {
                            Mode = CanvasComposite.DestinationIn,
                            Sources =
                            {
                                new ArithmeticCompositeEffect()
                                {
                                    Source1Amount = 1,
                                    Source2Amount = 1,
                                    MultiplyAmount = 0,

                                    Source1 = new ArithmeticCompositeEffect()
                                    {
                                        MultiplyAmount = 1,
                                        Source1Amount = 0,
                                        Source2Amount = 0,
                                        Source1 = new CompositionEffectSourceParameter("ImageSource"),
                                        Source2 = new SceneLightingEffect()
                                        {
                                            AmbientAmount = .6f,
                                            DiffuseAmount = 1f,
                                            SpecularAmount = 0f,
                                            NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
                                        }
                                    },
                                    Source2 = new SceneLightingEffect()
                                    {
                                        AmbientAmount = 0,
                                        DiffuseAmount = 0f,
                                        SpecularAmount = 1f,
                                        SpecularShine = 100,
                                        NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
                                    }
                                },
                                new CompositionEffectSourceParameter("NormalMap"),
                            }
                        };

                        _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _distantLight.CoordinateSpace = lightRoot;
                        _distantLight.Targets.Add(lightRoot);
                    };
                    break;

                default:
                    break;
            }

            //// Update the animations
            //UpdateAnimations();

            // Update all the image to have the new effect
            UpdateEffectBrush();
        }
        

        //private void gvMain_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        //{
        //    if (string.IsNullOrEmpty(ThumbnailFieldName)) return;

        //    try
        //    {
        //        //var imgs = args.ItemContainer.ContentTemplateRoot.GetFirstDescendantOfType<CompositionImage>();
        //        //CompositionImage image = imgs.First();

        //        CompositionImage image = args.ItemContainer.ContentTemplateRoot.GetFirstDescendantOfType<CompositionImage>();
        //        dynamic thumbnail = args.Item as dynamic;
        //        //Uri uri = new Uri(thumbnail.SquareThumbnailUrl);
        //        var propertyInfo = thumbnail.GetType().GetProperty(ThumbnailFieldName);
        //        var value = propertyInfo.GetValue(thumbnail, null);
        //        Uri uri = new Uri(value);

        //        // Setup the brush for this image
        //        SetImageEffect(image);

        //        // Update the image URI
        //        image.Source = uri;
        //    }
        //    catch (Exception ex) { }

        //}

        private void gvMain_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Vector2 offset = e.GetCurrentPoint(gvMain).Position.ToVector2();
            //ComboBoxItem item = LightingSelection.SelectedValue as ComboBoxItem;
            //switch ((LightingTypes)item.Tag)
            //System.Diagnostics.Debug.Write($"x:{offset.X}  y:{offset.Y}");
            switch(SelectedLight)
            {
                case LightingTypes.PointDiffuse:
                case LightingTypes.PointSpecular:
                    _pointLight.Offset = new Vector3(offset.X, offset.Y, 75);
                    break;

                case LightingTypes.SpotLightDiffuse:
                case LightingTypes.SpotLightSpecular:
                    _spotLight.Offset = new Vector3(offset.X, offset.Y, 100);
                    break;

                case LightingTypes.DistantDiffuse:
                case LightingTypes.DistantSpecular:
                    Vector3 position = new Vector3((float)gvMain.ActualWidth / 2, (float)gvMain.ActualHeight / 2, 200);
                    Vector3 lookAt = new Vector3((float)gvMain.ActualWidth - offset.X, (float)gvMain.ActualHeight - offset.Y, 0);
                    _distantLight.Direction = Vector3.Normalize(lookAt - position);
                    break;

                default:
                    break;
            }
        }
    }
}
