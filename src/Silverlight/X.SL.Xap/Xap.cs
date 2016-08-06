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
        private string xapDirectory;
        //private ZipArchive zipFile;

        public Xap(XamlLoader loader, string xapDirectory, DependencyObject root) {
            this.loader = loader;
            this.xapDirectory = xapDirectory;
            this.root = root;
        }

        public DependencyObject getRoot() { return root; }

        public async Task<Xap> createFromFile(XamlLoader loader, string filename) {

            var xap_dir = await this.Unpack(filename);
            object element_type = null;
            DependencyObject element = null;

            if (string.IsNullOrEmpty(xap_dir)) return null;

            // Load the AppManifest file
            string manifest = string.Concat(xap_dir, "appmanifest.xaml");
            element = loader.CreateDependencyObjectFromFile(manifest, false, element_type);
            //g_free(manifest);

            if (!(element_type is Deployment))
                return null;

            // TODO: Create a DependencyObject from the root node.

            Xap xap = new Xap(loader, xap_dir, element);
            return xap;


        }

        public async Task<string> Unpack(string fname) {

            var sf = await Utils.CreateTempDir(fname);
            var xap_dir = sf.Path;
            string extractPath = $@"{xap_dir}\{fname}\extract";
            
            //todo : remove dependence on ZipArchive and move to ZLib (X.Zlib) when possible

            using (ZipArchive archive = ZipFile.Open(xap_dir, ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(extractPath);
            }

            return xap_dir;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
