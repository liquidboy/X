using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extension.ThirdParty.Flickr.VM
{
    public class SplashVM :ViewModelBase
    {
        public string Title { get; set; } = "Splash";
        public string Version { get; set; } = "1.0.0.1";
        public ObservableCollection<string> FavouritePhotos { get; set; } = new ObservableCollection<string>();


        public SplashVM() {
            
        }

    }
}
