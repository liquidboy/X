using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Services.Data;

namespace X.Viewer.SketchFlow
{
    public class Sketch :ViewModelBase
    {
        public ObservableCollection<SketchPage> Pages { get; set; }

        private SketchPage _SelectedPage;
        public SketchPage SelectedPage {
            get { return _SelectedPage; }
            set {
                if(_SelectedPage != null)_SelectedPage.IsSelected = false;
                _SelectedPage = value;
                RaisePropertyChanged();
                _SelectedPage.IsSelected = true;
            }
        }
        
        public Sketch() {
            Pages = new ObservableCollection<SketchPage>();
        }

        public void ExternalPC(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }
    }

    public class SketchPage : ViewModelBase
    {
        public string Title { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private bool _IsSelected;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; RaisePropertyChanged(); } }

        public ObservableCollection<PageLayer> Layers { get; set; }

        public List<PageLayer> ExpandedLayers { get { return Layers.Where(x => x.IsExpanded).ToList(); } private set { } }

        public int Id { get; set; }
        
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
        private bool _isEnabled = true;
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged(); } } 
        public bool HasChildContainerCanvas { get; set; } = false;

        private bool _isExpanded = false;
        public bool IsExpanded { get { return _isExpanded; } set { _isExpanded = value; RaisePropertyChanged(); } }


        private string _layerNumber;
        public string LayerNumber { get { return _layerNumber; } set { _layerNumber = value; RaisePropertyChanged(); } }

        public ObservableCollection<XamlFragment> XamlFragments { get; set; }
        
        public PageLayer() {
            XamlFragments = new ObservableCollection<XamlFragment>();
        }

        public void ExternalPC(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }
    }


    public class XamlFragment: ViewModelBase
    {
        public string Xaml { get; set; }
        public string Uid { get; set; } = new Guid().ToString();

        public Type Type { get; set; }
        public string Data { get; set; }

        private bool _isEnabled = true;
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged(); } }
    }

}
