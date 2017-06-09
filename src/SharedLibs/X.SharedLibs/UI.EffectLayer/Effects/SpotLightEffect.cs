using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace X.SharedLibs.UI.EffectLayer.Effects
{
    public class SpotLightEffect : XamlLight
    {
        // Register an attached proeprty that enables apps to set a UIElement or Brush as a target for this light type in markup.
        public static readonly DependencyProperty IsTargetProperty =
            DependencyProperty.RegisterAttached(
            "IsTarget",
            typeof(Boolean),
            typeof(SpotLightEffect),
            new PropertyMetadata(null, OnIsTargetChanged)
        );
        public static void SetIsTarget(DependencyObject target, Boolean value)
        {
            target.SetValue(IsTargetProperty, value);
        }
        public static Boolean GetIsTarget(DependencyObject target)
        {
            return (Boolean)target.GetValue(IsTargetProperty);
        }

        // Handle attached property changed to automatically target and untarget UIElements and Brushes.    
        private static void OnIsTargetChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var isAdding = (Boolean)e.NewValue;

            if (isAdding)
            {
                if (obj is UIElement)
                {
                    AddTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    AddTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
            else
            {
                if (obj is UIElement)
                {
                    RemoveTargetElement(GetIdStatic(), obj as UIElement);
                }
                else if (obj is Brush)
                {
                    RemoveTargetBrush(GetIdStatic(), obj as Brush);
                }
            }
        }

        protected override void OnConnected(UIElement newElement)
        {
            // OnConnected is called when the first target UIElement is shown on the screen. This enables delaying composition object creation until it's actually necessary.
            CompositionLight = Window.Current.Compositor.CreateSpotLight();

            //newElement.PointerMoved += (s, e) =>
            //{
            //    var pos = e.GetCurrentPoint(newElement).Position;
            //    ((SpotLight)CompositionLight).Offset = new System.Numerics.Vector3((float)pos.X, (float)pos.Y, 40);
            //    ((SpotLight)CompositionLight).Direction = new System.Numerics.Vector3((float)pos.X, (float)pos.Y, 40);
            //};
        }

        protected override void OnDisconnected(UIElement oldElement)
        {
            // OnDisconnected is called when there are no more target UIElements on the screen. The CompositionLight should be disposed when no longer required.
            CompositionLight.Dispose();
            CompositionLight = null;
        }

        protected override string GetId()
        {
            return GetIdStatic();
        }

        private static string GetIdStatic()
        {
            // This specifies the unique name of the light. In most cases you should use the type's FullName.
            return typeof(SpotLightEffect).FullName;
        }



        #region offset



        public static float GetOffsetX(DependencyObject obj)
        {
            return (float)obj.GetValue(OffsetXProperty);
        }

        public static void SetOffsetX(DependencyObject obj, float value)
        {
            obj.SetValue(OffsetXProperty, value);
        }

        public static readonly DependencyProperty OffsetXProperty =
            DependencyProperty.RegisterAttached(
                "OffsetX", 
                typeof(float), 
                typeof(SpotLightEffect), 
                new PropertyMetadata(0, OnOffsetXChanged));


        
        private static void OnOffsetXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            
            if (obj is UIElement)
            {
                var xxx = (UIElement)obj;
                var sle = (SpotLightEffect)xxx.Lights[0];
                var sl = (SpotLight)sle.CompositionLight;
                sl.Offset = new System.Numerics.Vector3((float)e.NewValue, (float)100, 40);
            }
            else if (obj is Brush)
            {
                
            }

            // 
        }
        #endregion


    }
}
