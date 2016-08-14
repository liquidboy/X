using FlickrNet;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using X.CoreLib.GenericMessages;
using X.Services.Data;


namespace X.Extension.ThirdParty.FlickrHistory.VM
{
    public class SplashVM :ViewModelBase
    {
        public string Title { get; set; } = "Splash";
        public string Version { get; set; } = "1.0.0.1";
        public string GroupingType { get; set; } = "FlickrHistory";
        public string HostPackageFamilyName { get; set; } = "e10f99d4-4b05-4716-8c6a-4588ec9bef21_1v77q6cebkz10";


        private ObservableCollection<object> _history;
        public ObservableCollection<object> History { get { return _history; } set { _history = value; RaisePropertyChanged(); } }


        public SplashVM() {
            LoadMessangerRegistrations();
            History = new ObservableCollection<object>();

            //Messenger.Default.Send(new RequestPhotoComments());
        }

        private void LoadMessangerRegistrations()
        {
            Messenger.Default.Register<LoadPhoto>(this, DoLoadPhoto);
            Messenger.Default.Register<LoadPhotoUser>(this, DoLoadPhotoUser);
        }

        private void DoLoadPhoto(LoadPhoto msg)
        {
            //Comments = msg.Comments;
        }

        private void DoLoadPhotoUser(LoadPhotoUser msg)
        {
            History.Add(msg.User);
        }

        public void UnloadMessangerRegistrations()
        {
            Messenger.Default.Unregister<LoadPhoto>(this, DoLoadPhoto);
            Messenger.Default.Unregister<LoadPhotoUser>(this, DoLoadPhotoUser);
        }
    }
}
