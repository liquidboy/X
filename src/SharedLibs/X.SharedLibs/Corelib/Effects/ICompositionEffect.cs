using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace CoreLib.Effects
{
    public interface ICompositionEffect
    {
        void Initialize(UIElement element);
        void Uninitialize();
        Task<bool> Draw();
    }
}
