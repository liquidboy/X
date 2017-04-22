
using Microsoft.UI.Composition.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using static CoreLib.Effects.CompositionManager;

namespace CoreLib.Effects
{
    public class GammaTransferEffect : DependencyObject, ICompositionEffect
    {
        //xaml
        private UIElement element;
        private FrameworkElement frameworkElement;
        private bool isInitializedResources;

         
        //source
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(string), typeof(GammaTransferEffect), new PropertyMetadata(null, async (o,a)=> {
                
                if(a.NewValue!= null)
                {
                    GammaTransferEffect s = (GammaTransferEffect)o;
                    await s.InitializeResources();
                    s.Draw();
                }

            }));
        


        //Composition
        private ContainerVisual visual;
        private Compositor compositor;
        private CompositionImageFactory imageFactory;

        //Effect
        private string EffectSource = "EffectSource"; 
        private string EffectName = "GammaTransferEffect";
        private string RedExponentEffectPath = "GammaTransferEffect.RedExponent";
        private string GreenExponentEffectPath = "GammaTransferEffect.GreenExponent";
        private string BlueExponentEffectPath = "GammaTransferEffect.BlueExponent";
        private Microsoft.Graphics.Canvas.Effects.GammaTransferEffect effect; 
        private CompositionEffectFactory effectFactory;


        //CompositionImage
        private CompositionSurfaceBrush surfaceBrush;
        private CompositionImage imageSource;


        //Brush & Sprite
        private CompositionEffectBrush effectBrush;
        private SpriteVisual spriteVisual;


        public void Initialize(UIElement attachedElement)
        {
            element = attachedElement;
            frameworkElement = element as FrameworkElement;
        }

        public void Uninitialize()
        {
            effectFactory = null;
            compositor = null;
        }

        public async Task<bool> InitializeResources() {

            if (isInitializedResources) return isInitializedResources;

            if (string.IsNullOrEmpty(Source)) return false;

            CreateImageFactory(element);

            CreateEffect();

            var isSurfaceCreated = await CreateSurface();
            if (!isSurfaceCreated) return isSurfaceCreated;

            CreateBrush();

            CreateSpriteVisual();

            Insert();
            
            isInitializedResources = true;

            return isInitializedResources;
        }

        public async Task<bool> Draw()
        {
            
            if (!isInitializedResources) return false;
            UpdateSpriteVisual();
            UpdateRedExponent();
            return true;
        }

        private void CreateImageFactory(UIElement element)
        {
            visual = GetVisual(element);
            compositor = GetCompositor(visual);
            imageFactory = CreateCompositionImageFactory(compositor);
        }

        private void CreateEffect()
        {
            effect = new Microsoft.Graphics.Canvas.Effects.GammaTransferEffect()
            {
                Name = EffectName,
                BlueExponent = 1.0f,
                GreenExponent = 1.0f,
                RedExponent = 1.0f,
                Source = new CompositionEffectSourceParameter(EffectSource)
            };

            UpdateRedExponent();
            effectFactory = compositor.CreateEffectFactory(effect, new[] { BlueExponentEffectPath, GreenExponentEffectPath, RedExponentEffectPath });
        }

        private async Task<bool> CreateSurface()
        {
            surfaceBrush = compositor.CreateSurfaceBrush();
            //var uriSource = UriManager.GetFilUriFromString(Source);
            //imageSource = imageFactory.CreateImageFromUri(uriSource);
            try
            {
                var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(Source);
                if (file != null)
                {
                    imageSource = imageFactory.CreateImageFromFile(file);
                    surfaceBrush.Surface = imageSource.Surface;
                    surfaceBrush.Stretch = CompositionStretch.UniformToFill;
                }
            }
            catch //(Exception ex) 
            {
                return false;
            }

            return true;
        }

        private void CreateBrush()
        {
            effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter(EffectSource, surfaceBrush);
            effectBrush.Properties.InsertScalar(RedExponentEffectPath, 1f);
            effectBrush.Properties.InsertScalar(GreenExponentEffectPath, 1f);
            effectBrush.Properties.InsertScalar(BlueExponentEffectPath, 1f);
        }

        private void CreateSpriteVisual()
        {
            spriteVisual = compositor.CreateSpriteVisual();
            spriteVisual.Brush = effectBrush;
            spriteVisual.Size = new Vector2(GetElementWidth(), GetElementHeight());
        }

        private void UpdateSpriteVisual()
        {
            spriteVisual.Brush = effectBrush;
            spriteVisual.Size = new Vector2(GetElementWidth(), GetElementHeight());
        }

        private void Insert()
        {
            visual.Children.InsertAtBottom(spriteVisual);
        }

        private float GetElementWidth()
        {
            if (frameworkElement.ActualWidth.Equals(double.NaN))
            {
                return 0;
            }
            return (float)frameworkElement.ActualWidth;
        }

        private float GetElementHeight()
        {
            if (frameworkElement.ActualHeight.Equals(double.NaN))
            {
                return 0;
            }
            return (float)frameworkElement.ActualHeight;
        }



        //RedExponent
        public double RedExponent
        {
            get { return (double)GetValue(RedExponentProperty); }
            set { SetValue(RedExponentProperty, value); }
        }

        public static readonly DependencyProperty RedExponentProperty =
            DependencyProperty.Register(nameof(RedExponent), typeof(double), typeof(GammaTransferEffect), new PropertyMetadata(1.0, RedExponentChanged));

        private static void RedExponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GammaTransferEffect).UpdateRedExponent();
        }

        private void UpdateRedExponent()
        {
            effectBrush?.Properties.InsertScalar(RedExponentEffectPath, (float)RedExponent);
        }

        //GreenExponent
        public double GreenExponent
        {
            get { return (double)GetValue(GreenExponentProperty); }
            set { SetValue(GreenExponentProperty, value); }
        }

        public static readonly DependencyProperty GreenExponentProperty =
            DependencyProperty.Register(nameof(GreenExponent), typeof(double), typeof(GammaTransferEffect), new PropertyMetadata(1.0, GreenExponentChanged));

        private static void GreenExponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GammaTransferEffect).UpdateGreenExponent();
        }

        private void UpdateGreenExponent()
        {
            effectBrush?.Properties.InsertScalar(GreenExponentEffectPath, (float)GreenExponent);
        }

        //BlueExponent
        public double BlueExponent
        {
            get { return (double)GetValue(BlueExponentProperty); }
            set { SetValue(BlueExponentProperty, value); }
        }

        public static readonly DependencyProperty BlueExponentProperty =
            DependencyProperty.Register(nameof(BlueExponent), typeof(double), typeof(GammaTransferEffect), new PropertyMetadata(1.0, BlueExponentChanged));

        private static void BlueExponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as GammaTransferEffect).UpdateBlueExponent();
        }

        private void UpdateBlueExponent()
        {
            effectBrush?.Properties.InsertScalar(BlueExponentEffectPath, (float)BlueExponent);
        }
    }

}
