using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.SL.Core;

namespace X.SL
{
    public class Xap : IDisposable
    {
        public string xap_dir;
        public XamlLoader loader;
        public DependencyObject root;


        public Xap(XamlLoader loader, string xap_dir, DependencyObject root) {

        }

        public DependencyObject getRoot() { return root; }

        public Xap createFromFile(XamlLoader loader, string filename) {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
