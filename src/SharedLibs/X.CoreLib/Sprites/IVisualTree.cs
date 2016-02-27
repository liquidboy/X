using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace CoreLib.Sprites
{
    public interface IVisualTree
    {
        ICollection<IVisualTreeElement> Elements { get; }
        bool IsVisible { get; set; }


        void Add(IVisualTreeElement element);

        void Draw();

        void GarbageCollect();
    }
}
