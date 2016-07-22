using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace X.UI.Composition
{
    public sealed class ThumbnailLighting : Control
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

        public ThumbnailLighting()
        {
            
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


        private async Task FirstLoad() {

            //
            // Create the sperical normal map.  The normals will give the appearance of a sphere, and the alpha channel is used
            // for masking off the rectangular edges.
            //

            CompositionDrawingSurface normalMap = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Samples/SDK Insider/ThumbnailLighting/SphericalWithMask.png"));
            _circleNormalsBrush = _compositor.CreateSurfaceBrush(normalMap);
            _circleNormalsBrush.Stretch = CompositionStretch.Fill;


            // 
            // Create the flat normal map with beveled edges.  This should give the appearance of slanting of the surface along
            // the edges, flat in the middle.
            //

            normalMap = await SurfaceLoader.LoadFromUri(new Uri("ms-appx:///Samples/SDK Insider/ThumbnailLighting/BeveledEdges.jpg"));
            _flatNormalsBrush = _compositor.CreateSurfaceBrush(normalMap);
            _flatNormalsBrush.Stretch = CompositionStretch.Fill;

            // Update the effect brushes now that the normal maps are available.
            UpdateEffectBrush();
        }


        private void UpdateEffectBrush()
        {
            //if (ThumbnailList.ItemsPanelRoot != null)
            //{
            //    foreach (ListViewItem item in ThumbnailList.ItemsPanelRoot.Children)
            //    {
            //        CompositionImage image = item.ContentTemplateRoot.GetFirstDescendantOfType<CompositionImage>();
            //        SetImageEffect(image);
            //    }
            //}
        }

        LightingTypes _selectedLight = LightingTypes.SpotLightSpecular;
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
            switch(_selectedLight)
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

                        //// Set the light coordinate space and add the target
                        //Visual lightRoot = ElementCompositionPreview.GetElementVisual(ThumbnailList);
                        //_pointLight.CoordinateSpace = lightRoot;
                        //_pointLight.Targets.Add(lightRoot);
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

                        //// Set the light coordinate space and add the target
                        //Visual lightRoot = ElementCompositionPreview.GetElementVisual(ThumbnailList);
                        //_ambientLight.Targets.Add(lightRoot);
                        //_pointLight.CoordinateSpace = lightRoot;
                        //_pointLight.Targets.Add(lightRoot);
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

                        //// Set the light coordinate space and add the target
                        //Visual lightRoot = ElementCompositionPreview.GetElementVisual(ThumbnailList);
                        //_spotLight.CoordinateSpace = lightRoot;
                        //_spotLight.Targets.Add(lightRoot);
                        //_spotLight.InnerConeAngle = (float)(Math.PI / 15);
                        //_spotLight.OuterConeAngle = (float)(Math.PI / 10);
                        //_spotLight.Direction = new Vector3(0, 0, -1);
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

                        //// Set the light coordinate space and add the target
                        //Visual lightRoot = ElementCompositionPreview.GetElementVisual(ThumbnailList);
                        //_ambientLight.Targets.Add(lightRoot);
                        //_spotLight.CoordinateSpace = lightRoot;
                        //_spotLight.Targets.Add(lightRoot);
                        //_spotLight.InnerConeAngle = (float)(Math.PI / 15);
                        //_spotLight.OuterConeAngle = (float)(Math.PI / 10);
                        //_spotLight.Direction = new Vector3(0, 0, -1);
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

                        //Visual lightRoot = ElementCompositionPreview.GetElementVisual(ThumbnailList);
                        //_distantLight.CoordinateSpace = lightRoot;
                        //_distantLight.Targets.Add(lightRoot);
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

                        //Visual lightRoot = ElementCompositionPreview.GetElementVisual(ThumbnailList);
                        //_distantLight.CoordinateSpace = lightRoot;
                        //_distantLight.Targets.Add(lightRoot);
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
    }
}
