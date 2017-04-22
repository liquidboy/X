
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
    public class TemperatureAndTintEffect : DependencyObject, ICompositionEffect
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
            DependencyProperty.Register(nameof(Source), typeof(string), typeof(TemperatureAndTintEffect), new PropertyMetadata(null, async (o,a)=> {
                
                if(a.NewValue!= null)
                {
                    TemperatureAndTintEffect s = (TemperatureAndTintEffect)o;
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
        private string EffectName = "TemperatureAndTintEffect";
        private string TemperatureEffectPath = "TemperatureAndTintEffect.Temperature";   //-1 to 1
        private string TintEffectPath = "TemperatureAndTintEffect.Tint";   //-1 to 1
        private Microsoft.Graphics.Canvas.Effects.TemperatureAndTintEffect effect;
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

            //DetectElementChange();

            isInitializedResources = true;

            return isInitializedResources;
        }

        public async Task<bool> Draw()
        {
            if (!isInitializedResources) return false;
            UpdateSpriteVisual();
            UpdateTemperature();
            UpdateTint();
            return true;
        }

        //#region Detect Properties Change
        //private void DetectElementChange()
        //{
        //    frameworkElement.SizeChanged += (s, e) =>
        //    {
        //        UpdateSpriteVisual();
        //    };
        //}
        //#endregion

        private void CreateImageFactory(UIElement element)
        {
            visual = GetVisual(element);
            compositor = GetCompositor(visual);
            imageFactory = CreateCompositionImageFactory(compositor);
        }

        private void CreateEffect()
        {
            effect = new Microsoft.Graphics.Canvas.Effects.TemperatureAndTintEffect()
            {
                Name = EffectName,
                Temperature = 0f,
                Tint = 0f,
                Source = new CompositionEffectSourceParameter(EffectSource)
            };

            UpdateTemperature();
            UpdateTint();
            effectFactory = compositor.CreateEffectFactory(effect, new[] { TemperatureEffectPath, TintEffectPath });
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
            effectBrush.Properties.InsertScalar(TemperatureEffectPath, 0f);
            effectBrush.Properties.InsertScalar(TintEffectPath, 0f);
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



        //Temperature
        public double Temperature
        {
            get { return (double)GetValue(TemperatureProperty); }
            set { SetValue(TemperatureProperty, value); }
        }

        public static readonly DependencyProperty TemperatureProperty =
            DependencyProperty.Register(nameof(Temperature), typeof(double), typeof(TemperatureAndTintEffect), new PropertyMetadata(0.0, TemperatureChanged));

        private static void TemperatureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TemperatureAndTintEffect).UpdateTemperature();
        }

        private void UpdateTemperature()
        {
            effectBrush?.Properties.InsertScalar(TemperatureEffectPath, (float)Temperature);
        }



        //Tint
        public double Tint
        {
            get { return (double)GetValue(TintProperty); }
            set { SetValue(TintProperty, value); }
        }

        public static readonly DependencyProperty TintProperty =
            DependencyProperty.Register(nameof(Tint), typeof(double), typeof(TemperatureAndTintEffect), new PropertyMetadata(0.0, TintChanged));

        private static void TintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TemperatureAndTintEffect).UpdateTint();
        }

        private void UpdateTint()
        {
            effectBrush?.Properties.InsertScalar(TintEffectPath, (float)Tint);
        }

    }

}
