//using Microsoft.UI.Composition.Toolkit;
using Microsoft.UI.Composition.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace CoreLib.Effects
{
    public class CompositionManager
    {
        public static ContainerVisual GetVisual(UIElement element)
        {
            var hostVisual = ElementCompositionPreview.GetElementVisual(element);
            ContainerVisual root = hostVisual.Compositor.CreateContainerVisual();
            ElementCompositionPreview.SetElementChildVisual(element, root);
            return root;
        }

        public static Compositor GetCompositor(Visual visual)
        {
            return visual.Compositor;
        }

        public static CompositionImageFactory CreateCompositionImageFactory(Compositor compositor)
        {
            
            return CompositionImageFactory.CreateCompositionImageFactory(compositor);
        }

    }

}
