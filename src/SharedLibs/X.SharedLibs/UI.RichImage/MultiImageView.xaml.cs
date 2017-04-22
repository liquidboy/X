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
using X.Browser;

namespace X.UI.RichImage
{
    public sealed partial class MultiImageView : UserControl, IDisposable
    {
        int currentIndex = 0;
        DispatcherTimer dt;



        public MultiImageView()
        {
            this.InitializeComponent();

        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //var effect = (ICompositionEffect)grdImgMain.GetValue(Composition.EffectProperty);
            //effect.Draw();


            return base.ArrangeOverride(finalSize);
        }


        private void Dt_Tick(object sender, object e)
        {
            ChangeMainImage(currentIndex + 1);
        }


        private void ChangeMainImage(int index)
        {
            var max = ChildrenTabs?.Count;
            if (max > 0)
            {
                if (index > max - 1) currentIndex = 0;
                else currentIndex = index;

                DefaultUri = ChildrenTabs[currentIndex].ThumbUri;
            }
        }


        public void UnloadResources()
        {
            if (dt != null)
            {
                dt.Stop();
                dt.Tick -= Dt_Tick;
                dt = null;
            }

            //var effect = (ICompositionEffect)grdImgMain.GetValue(Composition.EffectProperty);
            //if (effect != null) {
            //    effect.Uninitialize();
            //    ((IDisposable)effect).Dispose();
            //    effect = null;
            //}

            ////grdImgMain.Children.Clear();
            ////rootElement.Children.Remove(grdImgMain);


        }


        public IList<TabViewModel> ChildrenTabs
        {
            get { return (IList<TabViewModel>)GetValue(ChildrenTabsProperty); }
            set { SetValue(ChildrenTabsProperty, value); }
        }


        public static readonly DependencyProperty ChildrenTabsProperty =
            DependencyProperty.Register("ChildrenTabs", typeof(IList<TabViewModel>), typeof(MultiImageView), new PropertyMetadata(null));



        public string DefaultUri
        {
            get { return (string)GetValue(DefaultUriProperty); }
            set { SetValue(DefaultUriProperty, value); }
        }

        public static readonly DependencyProperty DefaultUriProperty =
            DependencyProperty.Register("DefaultUri", typeof(string), typeof(MultiImageView), new PropertyMetadata("", DefaultUriChanged));
        
        private static void DefaultUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            MultiImageView el = (MultiImageView)d;

            if (string.IsNullOrEmpty(el.DefaultUri))
            {
                //el.LoadGridEffect(el);  // <== DANGEROUS as this can cause memory leaks if effects arnt correctly disposed
                //there is a known issue with virtualized items in the gridview, where reordered items are causing the same
                //uielement to be reused BUT the attached composition visual (via the effect attached property) is not 
                //being disposed .. This can lead to many orphaned UIElements with attached Composition Visuals...
                //TODO : try to work out what the hell is going on and how to dispose!!!!
            }

        }






        public string BackgroundColor
        {
            get { return (string)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(string), typeof(MultiImageView), new PropertyMetadata("#FFCCCCCC"));









        private void Grid_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var tvm = (TabViewModel)((Grid)sender).DataContext;
            var index = ChildrenTabs.IndexOf(tvm);
            ChangeMainImage(index);
        }








        public int ThumbTemplateToUse
        {
            get { return (int)GetValue(ThumbTemplateToUseProperty); }
            set { SetValue(ThumbTemplateToUseProperty, value); }
        }

        public static readonly DependencyProperty ThumbTemplateToUseProperty =
            DependencyProperty.Register("ThumbTemplateToUse", typeof(int), typeof(MultiImageView), new PropertyMetadata(0, ThumbTemplateChanged));

        private static void ThumbTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //MultiImageView el = (MultiImageView)d;

            //var type = (int)e.NewValue;

            //if (!string.IsNullOrEmpty(el.DefaultUri)) {
            //    el.LoadGridEffect(el);
            //}

            //
            //
            //effect.Source = (string)parameter;

            //var grd = new Grid();
            //grd.SetValue(CoreLib.Effects.Composition.EffectProperty, effect);

            //return grd;

        }



        //public void LoadGridEffect( MultiImageView el) {
        //    if (el.ThumbTemplateToUse == 1)
        //    {
        //        var foundEffect = el.grdImgMain.GetValue(CoreLib.Effects.Composition.EffectProperty);
        //        if (foundEffect == null)
        //        {
        //            var effect = new CoreLib.Effects.SaturationEffect();
        //            effect.Level = 150;
        //            effect.Source = el.DefaultUri;
        //            el.grdImgMain.SetValue(CoreLib.Effects.Composition.EffectProperty, effect);
        //        }
        //        else {
        //            ((ICompositionEffect)foundEffect).Draw();
        //        }
        //    }
        //    else if (el.ThumbTemplateToUse == 2)
        //    {
        //        var foundEffect = el.grdImgMain.GetValue(CoreLib.Effects.Composition.EffectProperty);
        //        if (foundEffect == null) {
        //            var effect2 = new CoreLib.Effects.GrayscaleEffect();
        //            effect2.Source = el.DefaultUri;
        //            el.grdImgMain.SetValue(CoreLib.Effects.Composition.EffectProperty, effect2);
        //        }
        //        else
        //        {
        //            ((ICompositionEffect)foundEffect).Draw();
        //        }

        //    }
        //    else
        //    {

        //    }
        //}


        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(5);
            dt.Tick += Dt_Tick;
            dt.Start();
        }




        private void rootElement_Unloaded(object sender, RoutedEventArgs e)
        {
            UnloadResources();
        }

        public void Dispose()
        {
            //var effect = (ICompositionEffect)grdImgMain.GetValue(Composition.EffectProperty);
            //effect.Uninitialize();
        }
    }
}
