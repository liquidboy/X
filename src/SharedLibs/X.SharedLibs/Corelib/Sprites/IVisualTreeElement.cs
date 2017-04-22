using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace CoreLib.Sprites
{
    public interface IVisualTreeElement
    {
        string ID { get; set; }
        Rect Layout { get; set; }
        bool IsVisible { get; set; }
        string TextureBackgroundUri { get; set; }
        bool CanDeleteFromTree { get; set; }

        void Draw(ref UIElement surface);
        void Delete(ref UIElement surface);
    }
}
