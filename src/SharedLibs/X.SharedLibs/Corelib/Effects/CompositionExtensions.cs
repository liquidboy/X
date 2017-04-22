using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace CoreLib.Effects
{
    public class Composition
    {


        //Effect
        public static void SetEffect(UIElement element, ICompositionEffect value) { element.SetValue(EffectProperty, value); }
        public static ICompositionEffect GetEffect(UIElement element) { return (ICompositionEffect)element.GetValue(EffectProperty); }

        public static readonly DependencyProperty EffectProperty =
            DependencyProperty.RegisterAttached(nameof(EffectProperty), typeof(ICompositionEffect), typeof(Extensions), new PropertyMetadata(null, EffectChanged));

        private static void EffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is ICompositionEffect && e.OldValue is ICompositionEffect)
            {
                //when moving items around (reordering) newvalue & oldvalue are populated
                var effectOld = e.OldValue as ICompositionEffect;
                var effectNew = e.NewValue as ICompositionEffect;
                effectOld.Draw();
                effectNew.Draw();
            }
            else
            { 
                //when setting up for the first time (where oldvalue is null) ... 
                
                var uiSender = d as UIElement;
                var effect = e.NewValue as ICompositionEffect;
                effect.Initialize(uiSender);
            } 
        }

        

    }

    public class Extensions
    {
    
    
    }
}
