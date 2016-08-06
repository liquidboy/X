using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.SL.Core;

namespace X.SL
{
    public class XamlLoader : IDisposable
    {
        enum XamlLoaderFlags
        {
            NONE,
            VALIDATE_TEMPLATES = 2,
            IMPORT_DEFAULT_XMLNS = 4
        };
        
        public DependencyObject CreateDependencyObjectFromString(string xaml, bool create_namescope, object element_type)
        {
            throw new NotImplementedException();
        }

        public DependencyObject CreateDependencyObjectFromFile(string xaml, bool create_namescope, object element_type)
        {

            return null;
        }

        public object CreateFromFileWithError(string xaml, bool create_namescope, out Type element_type, SLError error)
        {
            throw new NotImplementedException();
        }

        public object CreateFromStringWithError(string xaml, bool create_namescope, object element_type, int flags, SLError error, DependencyObject owner = null)
        {
            throw new NotImplementedException();
        }

        public object HydrateFromStringWithError(string xaml, object obj, bool create_namescope, object element_type, int flags, SLError error) {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
