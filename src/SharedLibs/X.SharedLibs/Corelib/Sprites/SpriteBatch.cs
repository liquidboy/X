using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace CoreLib.Sprites
{
    public class SpriteBatch :IVisualTree
    {
        public ICollection<IVisualTreeElement> Elements { get; } = new LinkedList<IVisualTreeElement>();

        private bool _isVisible = false;
        public bool IsVisible { get { return _isVisible; } set { _isVisible = value; if (_surface == null) return; if (_isVisible) { _surface.Visibility = Visibility.Visible; } else { _surface.Visibility = Visibility.Collapsed; } } }

        private UIElement _surface;
        public SpriteBatch(UIElement surface) {
            _surface = surface;
        }
        
        public void Draw() {
            if (IsVisible)
                Elements.ToList().ForEach(x => x.Draw(ref _surface));
            
            GarbageCollect();
        }

        public void DeleteAll() {
            //Elements.ToList().ForEach(x => x.Delete(ref _surface));
            Elements.Clear();
        }

        public void Add(IVisualTreeElement element)
        {
            Elements.Add(element);
        }

        public void GarbageCollect() {
            Elements.Where(x=>x.CanDeleteFromTree).ToList().ForEach(delegate(IVisualTreeElement x) { Elements.Remove(x); });
        }

    }
}
