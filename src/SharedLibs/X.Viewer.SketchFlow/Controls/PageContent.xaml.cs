using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace X.Viewer.SketchFlow.Controls
{
    public sealed partial class PageContent : UserControl
    {
        public PageContent()
        {
            this.InitializeComponent();
        }

        public object FindContentElementByName(string name) {

            if (cc.Content is Grid) {
                var grd = cc.Content as Grid;


                List<DependencyObject> found = new List<DependencyObject>();
                DumpVisualTree(grd, ref found);

                foreach (var el in found) {
                    if (((FrameworkElement)el).Name == name) return el;
                }
                //return found.Where(x => ((FrameworkElement)x).Name == name).FirstOrDefault();
                //return grd.Children.OfType<FrameworkElement>().Single(f => f.Name == name);
            }

            return null;
        }


        private void DumpVisualTree(DependencyObject parent, ref List<DependencyObject> objects)
        {
            string typeName = parent.GetType().Name;
            string name = (string)(parent.GetValue(FrameworkElement.NameProperty) ?? "");
            
            if (parent == null) return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                objects.Add(child);
                DumpVisualTree(child, ref objects);
            }
        }
    }
}
