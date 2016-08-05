using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.SL.Core;

namespace X.SL
{
    public class Xap : IDisposable
    {
        
        private XamlLoader loader;
        private DependencyObject root;
        private string xap_dir;
        private ZipArchive zipFile;


        public Xap(XamlLoader loader, string xap_dir, DependencyObject root) {

        }

        public DependencyObject getRoot() { return root; }

        public Xap createFromFile(XamlLoader loader, string filename) {
            throw new NotImplementedException();
        }

        public void Unpack(string fname) {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
