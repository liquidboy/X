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


namespace X.Extension.ThirdParty.FlickrComments.VM
{
    public class SplashVM :ViewModelBase
    {
        public string Title { get; set; } = "Splash";
        public string Version { get; set; } = "1.0.0.1";
        public string GroupingType { get; set; } = "FlickrComments";
        public string HostPackageFamilyName { get; set; } = "cdb82af3-5805-42f7-bb9e-9c2dcc2f45f9_1v77q6cebkz10";


        private object _comments;
        public object Comments { get { return _comments; } set { _comments = value; RaisePropertyChanged(); } }


        public SplashVM() {
            LoadMessangerRegistrations();

            Messenger.Default.Send(new RequestPhotoComments());
        }

        private void LoadMessangerRegistrations()
        {
            Messenger.Default.Register<LoadPhotoComments>(this, DoLoadPhotoComments);
        }

        private void DoLoadPhotoComments(LoadPhotoComments msg) {
            Comments = msg.Comments;
        }

        public void UnloadMessangerRegistrations()
        {
            Messenger.Default.Unregister<LoadPhotoComments>(this, DoLoadPhotoComments);
        }
    }
}
