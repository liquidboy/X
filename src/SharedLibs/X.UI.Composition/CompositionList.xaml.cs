﻿using System;
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


    public sealed partial class CompositionList : UserControl
    {
        private Compositor _compositor;
        private CompositionEffectFactory _effectFactory;
        private CompositionSurfaceBrush _flatNormalsBrush;
        private CompositionSurfaceBrush _circleNormalsBrush;
        private AmbientLight _ambientLight;
        private PointLight _pointLight;
        private DistantLight _distantLight;
        private SpotLight _spotLight;

        public enum LightingTypes
        {
            PointDiffuse,
            PointSpecular,
            SpotLightDiffuse,
            SpotLightSpecular,
            DistantDiffuse,
            DistantSpecular,
        }

        public CompositionList()
        {
            this.InitializeComponent();

            this.Loaded += CompositionList_Loaded;
            this.Unloaded += CompositionList_Unloaded;

            SurfaceLoader.Initialize(ElementCompositionPreview.GetElementVisual(this).Compositor);
            
            // Get the current compositor
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;


            //
            // Create the lights
            //
            _ambientLight = _compositor.CreateAmbientLight();
            _pointLight = _compositor.CreatePointLight();
            _distantLight = _compositor.CreateDistantLight();
            _spotLight = _compositor.CreateSpotLight();
        }

        private void CompositionList_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private async void CompositionList_Loaded(object sender, RoutedEventArgs e)
        {
            await InitControl();

            UpdateLightingEffect();
        }

        public Visibility ListVisibility
        {
            get { return (Visibility)GetValue(ListVisibilityProperty); }
            set { SetValue(ListVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ListVisibilityProperty =
            DependencyProperty.Register("ListVisibility", typeof(Visibility), typeof(CompositionList), new PropertyMetadata(Visibility.Visible));




        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(CompositionList), new PropertyMetadata(null));



        private async Task InitControl()
        {

            //
            // Create the sperical normal map.  The normals will give the appearance of a sphere, and the alpha channel is used
            // for masking off the rectangular edges.
            //
            CompositionDrawingSurface normalMap = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///X.UI.Composition.Assets/SphericalWithMask.png"));
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

        private void UpdateEffectBrush()
        {
            if (gvMain.ItemsPanelRoot != null)
            {
                foreach (ListViewItem item in gvMain.ItemsPanelRoot.Children)
                {
                    CompositionImage image = item.ContentTemplateRoot.GetFirstDescendantOfType<CompositionImage>();
                    SetImageEffect(image);
                }
            }
        }

        LightingTypes _selectedLight = LightingTypes.PointDiffuse;
        private void SetImageEffect(CompositionImage image)
        {
            // Create the effect brush and bind the normal map
            CompositionEffectBrush brush = _effectFactory.CreateBrush();

            //ComboBoxItem item = LightingSelection.SelectedValue as ComboBoxItem;
            //switch ((LightingTypes)item.Tag)
            switch (_selectedLight)
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


        private void UpdateLightingEffect()
        {
            _ambientLight.Targets.RemoveAll();
            _pointLight.Targets.RemoveAll();
            _distantLight.Targets.RemoveAll();
            _spotLight.Targets.RemoveAll();

            //ComboBoxItem item = LightingSelection.SelectedValue as ComboBoxItem;
            //switch ((LightingTypes)item.Tag)
            switch (_selectedLight)
            {
                case LightingTypes.PointDiffuse:
                    {
                        //
                        // Result = Ambient +       Diffuse
                        // Result = (Image) + (.75 * Diffuse color)
                        //

                        IGraphicsEffect graphicsEffect = new CompositeEffect()
                        {
                            Mode = CanvasComposite.Add,
                            Sources =
                            {
                                new CompositionEffectSourceParameter("ImageSource"),
                                new SceneLightingEffect()
                                {
                                    AmbientAmount = 0,
                                    DiffuseAmount = .75f,
                                    SpecularAmount = 0,
                                    NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
                                }
                            }
                        };

                        _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

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

                        IGraphicsEffect graphicsEffect = new CompositeEffect()
                        {
                            Mode = CanvasComposite.Add,
                            Sources =
                            {
                                new CompositionEffectSourceParameter("ImageSource"),
                                new SceneLightingEffect()
                                {
                                    AmbientAmount = 0,
                                    DiffuseAmount = .75f,
                                    SpecularAmount = 0,
                                    NormalMapSource = new CompositionEffectSourceParameter("NormalMap"),
                                }
                            }
                        };

                        _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

                        // Set the light coordinate space and add the target
                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _spotLight.CoordinateSpace = lightRoot;
                        _spotLight.Targets.Add(lightRoot);
                        _spotLight.InnerConeAngle = (float)(Math.PI / 15);
                        _spotLight.OuterConeAngle = (float)(Math.PI / 10);
                        _spotLight.Direction = new Vector3(0, 0, -1);
                    };
                    break;

                case LightingTypes.SpotLightSpecular:
                    {
                        //
                        // Result =    Ambient   +       Diffuse           +     Specular
                        // Result = (Image * .6) + (Image * Diffuse color) + (Specular color)
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

                        // Create the effect factory
                        _effectFactory = _compositor.CreateEffectFactory(graphicsEffect);

                        // Set the light coordinate space and add the target
                        Visual lightRoot = ElementCompositionPreview.GetElementVisual(gvMain);
                        _ambientLight.Targets.Add(lightRoot);
                        _spotLight.CoordinateSpace = lightRoot;
                        _spotLight.Targets.Add(lightRoot);
                        _spotLight.InnerConeAngle = (float)(Math.PI / 15);
                        _spotLight.OuterConeAngle = (float)(Math.PI / 10);
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

        private void gvMain_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            CompositionImage image = args.ItemContainer.ContentTemplateRoot.GetFirstDescendantOfType<CompositionImage>();
            dynamic thumbnail = args.Item as dynamic;
            Uri uri = new Uri(thumbnail.SquareThumbnailUrl);

            // Setup the brush for this image
            SetImageEffect(image);

            // Update the image URI
            image.Source = uri;
        }

        private void gvMain_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Vector2 offset = e.GetCurrentPoint(gvMain).Position.ToVector2();
            //ComboBoxItem item = LightingSelection.SelectedValue as ComboBoxItem;
            //switch ((LightingTypes)item.Tag)
            switch(_selectedLight)
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
