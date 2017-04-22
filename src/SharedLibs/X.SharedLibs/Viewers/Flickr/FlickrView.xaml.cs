using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.Viewer.Flickr
{
    public sealed partial class FlickrView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private object _photo;
        public object Photo { get { return _photo; } set { _photo = value; RaisePropertyChanged("Photo");  } }

        private object _user;
        public object User { get { return _user; } set { _user = value; RaisePropertyChanged("User"); } }

        public FlickrView()
        {
            this.InitializeComponent();
        }

        private void RaisePropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            var lt = UI.Composition.LightPanel.LightingTypes.DistantDiffuse;
            switch (lpPlaque.SelectedLight) {
                case UI.Composition.LightPanel.LightingTypes.DistantDiffuse:
                    lt = UI.Composition.LightPanel.LightingTypes.DistantSpecular;
                    break;
                case UI.Composition.LightPanel.LightingTypes.DistantSpecular:
                    lt = UI.Composition.LightPanel.LightingTypes.PointDiffuse;
                    break;
                case UI.Composition.LightPanel.LightingTypes.PointDiffuse:
                    lt = UI.Composition.LightPanel.LightingTypes.PointSpecular;
                    break;
                case UI.Composition.LightPanel.LightingTypes.PointSpecular:
                    lt = UI.Composition.LightPanel.LightingTypes.SpotLightDiffuse;
                    break;
                case UI.Composition.LightPanel.LightingTypes.SpotLightDiffuse:
                    lt = UI.Composition.LightPanel.LightingTypes.SpotLightSpecular;
                    break;
                case UI.Composition.LightPanel.LightingTypes.SpotLightSpecular:
                    lt = UI.Composition.LightPanel.LightingTypes.DistantDiffuse;
                    break;
            }

            lpPlaque.SelectedLight = lt;
            lpImage.SelectedLight = lt;

            lpImage.Redraw();
        }
    }
}
