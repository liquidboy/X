using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation.Collections;

namespace X.Services.Extensions
{
    public static class ExtensionHelper
    {

        public static IEnumerable<XElement> GetElement(string nodeName, XDocument xmlData)
        {
            //NullReferenceException because xmlData is not initializied yet
            return xmlData.Descendants(nodeName).ToList();
        }

        public static KeyValuePair<string, object> GetPropertyFromResults(ValueSet valueSet, string key) {
            return valueSet.Where(x => x.Key == key).FirstOrDefault();
        }

    }
}


//eg. X.Viewer.SketchFlow.Controls.Stamps.Picture.LoadPictureLibrary
//   ---> X.Viewer.SketchFlow.Controls.Pickers.ImagePicker.LoadData