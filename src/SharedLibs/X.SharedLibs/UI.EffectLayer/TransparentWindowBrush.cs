
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;
using Windows.Graphics.Effects;

namespace X.UI.EffectLayer
{
    public class TransparentWindowBrush : XamlCompositionBrushBase
    {
        SpriteVisual _hostSprite;

        protected override void OnConnected()
        {
            Compositor compositor = Window.Current.Compositor;
            
            // CompositionCapabilities: Are Effects supported?
            bool usingFallback = !CompositionCapabilities.GetForCurrentView().AreEffectsSupported();
            FallbackColor = Color.FromArgb(100, 100, 100, 100);

            if (usingFallback)
            {
                // If Effects are not supported, use Fallback Solid Color
                CompositionBrush = compositor.CreateColorBrush(FallbackColor);
                return;
            }

            // Define Effect graph
            // COLOR SWAP
            Matrix5x4 mat = new Matrix5x4()
            {
                M11 = 0,  M12 = 0f, M13 = 1f, M14 = 0,
                M21 = 0f, M22 = 1f, M23 = 0f, M24 = 0,
                M31 = 1f, M32 = 0f, M33 = 0f, M34 = 0,
                M41 = 0f, M42 = 0f, M43 = 0f, M44 = 1,
                M51 = 0,  M52 = 0,  M53 = 0,  M54 = 0
            };

            //// COLOR MASK (DOESNT WORK ???)
            // Matrix5x4 mat = new Matrix5x4()
            //            {
            //                M11 = 1,  M12 = 0f, M13 = 0f, M14 = 2,
            //                M21 = 0f, M22 = 1f, M23 = 0f, M24 = -1,
            //                M31 = 0f, M32 = 0f, M33 = 1f, M34 = -1,
            //                M41 = 0f, M42 = 0f, M43 = 0f, M44 = 0,
            //                M51 = 0,  M52 = 0,  M53 = 0,  M54 = 0
            //            };

            //// REDFILTER
            //Matrix5x4 mat = new Matrix5x4()
            //            {
            //                M11 = 1,  M12 = 0f, M13 = 0f, M14 = 0,
            //                M21 = 0f, M22 = 0f, M23 = 0f, M24 = 0,
            //                M31 = 0f, M32 = 0f, M33 = 0f, M34 = 0,
            //                M41 = 0f, M42 = 0f, M43 = 0f, M44 = 1,
            //                M51 = 0,  M52 = 0,  M53 = 0,  M54 = 0
            //            };

            IGraphicsEffect graphicsEffect = new ColorMatrixEffect()
            {
                ColorMatrix = mat,
                Source = new CompositionEffectSourceParameter("ImageSource")
            };


            //// COLORHIGHLIGHT
            //IGraphicsEffect graphicsEffect = new ArithmeticCompositeEffect()
            //{
            //    MultiplyAmount = 0,
            //    Source1Amount = 1,
            //    Source2Amount = 1,
            //    Source1 = new GammaTransferEffect()
            //    {
            //        RedExponent = 7,
            //        BlueExponent = 7,
            //        GreenExponent = 7,
            //        RedAmplitude = 3,
            //        GreenAmplitude = 3,
            //        BlueAmplitude = 3,
            //        Source = new CompositionEffectSourceParameter("ImageSource")
            //    },
            //    Source2 = new SaturationEffect()
            //    {
            //        Saturation = 0,
            //        Source = new CompositionEffectSourceParameter("ImageSource")
            //    }
            //};



            // Create the effect factory and instantiate a brush
            CompositionEffectFactory _effectFactory = compositor.CreateEffectFactory(graphicsEffect, null);
            CompositionEffectBrush effectBrush = _effectFactory.CreateBrush();
            
            // Create BackdropBrush
            CompositionBackdropBrush backdrop = compositor.CreateHostBackdropBrush();
            effectBrush.SetSourceParameter("ImageSource", backdrop);

            // Set EffectBrush as the brush that XamlCompBrushBase paints onto Xaml UIElement
            CompositionBrush = effectBrush;
            
        }

        protected override void OnDisconnected()
        {
            // Dispose CompositionBrushes if XamlCompBrushBase is removed from tree
            if (CompositionBrush != null)
            {
                CompositionBrush.Dispose();
                CompositionBrush = null;
            }
        }
    }
}
