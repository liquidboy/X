using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Viewer.SketchFlow
{
    public class Sketch :ViewModelBase
    {
        public ObservableCollection<SketchPage> Pages { get; set; }

        public Sketch() {
            Pages = new ObservableCollection<SketchPage>();
        }
    }

    public class SketchPage : ViewModelBase
    {

        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ObservableCollection<PageLayer> Layers { get; set; }
        public SketchPage()
        {
            Layers = new ObservableCollection<PageLayer>();
        }
        public void ExternalPC(string propertyName) {
            RaisePropertyChanged(propertyName);
        }
    }

    public class PageLayer : ViewModelBase
    {
        public bool IsEnabled { get; set; } = true;
        public ObservableCollection<string> XamlFragments { get; set; }

        public PageLayer() {
            XamlFragments = new ObservableCollection<string>();
        }

        public void ExternalPC(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }
    }
}
