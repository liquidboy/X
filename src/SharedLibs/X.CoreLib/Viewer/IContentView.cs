using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace X.Viewer
{
    public interface IContentView
    {

        event EventHandler<ContentViewEventArgs> SendMessage;
        void Unload();
        void Load();
    }
}
