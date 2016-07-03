
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
    public class GrayscaleEffect : DependencyObject, ICompositionEffect, IDisposable
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
            DependencyProperty.Register(nameof(Source), typeof(string), typeof(GrayscaleEffect), new PropertyMetadata(string.Empty));

        
        //Composition
        private ContainerVisual visual;
        private Compositor compositor; 
        private CompositionImageFactory imageFactory;

        //Effect
        private string EffectSource = "EffectSource"; 
        private string EffectName = "GrayscaleEffect";
        private Microsoft.Graphics.Canvas.Effects.GrayscaleEffect effect;
        private CompositionEffectFactory effectFactory;


        //CompositionImage
        private CompositionSurfaceBrush surfaceBrush;
        private CompositionImage imageSource;


        //Brush & Sprite
        private CompositionEffectBrush effectBrush;
        private SpriteVisual spriteVisual;


        public async void Initialize(UIElement attachedElement)
        {
            element = attachedElement;
            frameworkElement = element as FrameworkElement;

            try
            {

                await InitializeResources();
                await Draw();
            }
            catch //(Exception ex)
            {
            }
            

        }

        public void Uninitialize()
        {
            effectFactory = null;
            compositor = null;
        }

        private async Task<bool> InitializeResources() {

            ////if (isInitializedResources) return isInitializedResources;

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

        public async Task<bool> Draw() {
            if (!isInitializedResources) return false;
            //if(!spriteVisual.IsVisible) spriteVisual.IsVisible = true;
            UpdateSpriteVisual();
            //element.UpdateLayout();
            return true;
        }

        private void CreateImageFactory(UIElement element)
        {
            if (visual == null) visual = GetVisual(element);
            compositor = GetCompositor(visual);
            //visual.Clip = compositor.CreateInsetClip(0, 0, 0, 0);
            if (imageFactory == null) imageFactory = CreateCompositionImageFactory(compositor);  //<=== this causes a hige growth in memory footprint
        }

        private void CreateEffect()
        {
            effect = new Microsoft.Graphics.Canvas.Effects.GrayscaleEffect()
            {
                Name = EffectName,
                Source = new CompositionEffectSourceParameter(EffectSource)
            };

            effectFactory = compositor.CreateEffectFactory(effect);
        }

        private async Task<bool> CreateSurface()
        {
            try
            {
                surfaceBrush = compositor.CreateSurfaceBrush();
                //var uriSource = UriManager.GetFilUriFromString(Source);
                //imageSource = imageFactory.CreateImageFromUri(uriSource);

                var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(Source);
                if (file != null)
                {
                    imageSource = imageFactory.CreateImageFromFile(file);
                    surfaceBrush.Surface = imageSource.Surface;
                    surfaceBrush.Stretch = CompositionStretch.UniformToFill;
                }
            }
            catch (Exception ex) {
                return false;
            }

            return true;
        }

        private void CreateBrush()
        {
            effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter(EffectSource, surfaceBrush);
        }

        private void CreateSpriteVisual()
        {
            spriteVisual = compositor.CreateSpriteVisual();
            spriteVisual.Brush = effectBrush;
            spriteVisual.Size = new Vector2(GetElementWidth(), GetElementHeight());
        }

        private void UpdateSpriteVisual()
        {
            if (spriteVisual == null) return;
            spriteVisual.Brush = effectBrush;
            spriteVisual.Size = new Vector2(GetElementWidth(), GetElementHeight());

        }

        private void Insert()
        {
            visual?.Children.InsertAtBottom(spriteVisual);
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

        public void Dispose()
        {
            imageFactory = null;
            imageSource = null;


            if (effect != null)
            {
                effect.Dispose();
                effect = null;
            }

            if (surfaceBrush != null)
            {
                surfaceBrush.Surface = null;
                surfaceBrush.Dispose();
                surfaceBrush = null;
            }

            //if (visual != null)
            //{
            //    if(spriteVisual!=null) visual.Children.Remove(spriteVisual);
            //    visual.Dispose();
            //    visual = null;
            //}
            
            if (effectBrush != null)
            {
                effectBrush.SetSourceParameter(EffectSource, null);
                effectBrush.Dispose();
                effectBrush = null;
            }

            if (spriteVisual != null) {
                spriteVisual.Brush = null;
                spriteVisual.Dispose();
                spriteVisual = null;
            }
            
        }
    }

}
