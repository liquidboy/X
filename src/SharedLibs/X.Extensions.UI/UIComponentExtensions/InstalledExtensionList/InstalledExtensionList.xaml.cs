using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WeakEvent;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.Services.Data;

namespace X.Extensions.UIComponentExtensions
{
    public sealed partial class InstalledExtensionList : UserControl, IExtensionContent
    {
        public InstalledExtensionList()
        {
            this.InitializeComponent();

            tlMain.AddTab("Installed Extensions", true);
            tlMain.AddTab("Store");

            layoutRoot.DataContext = this;
        }

        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public async void RecieveMessage(object message)
        {
            if (message is ResponseListOfInstalledExtensionsEventArgs ) {
                var ea = (ResponseListOfInstalledExtensionsEventArgs)message;
                tbExtensionCount.Text = ea.ExtensionsMetadata.Count() + " extensions";

                var extensionsInStorage = X.Services.Data.StorageService.Instance.Storage.RetrieveList<ExtensionManifestDataModel>();
                foreach (dynamic emd in ea.ExtensionsMetadata) {
                    var uid = emd.TitleHashed;
                    var found = extensionsInStorage.Where(x => x.Uid == uid).ToList();
                    if (found != null && found.Count() > 0) {
                        emd.Id = found.First().Id;
                        emd.IsExtEnabled = found.First().IsExtEnabled;
                    }
                }


                icMain.ItemsSource = ea.ExtensionsMetadata;
            }
        }

        public void Unload()
        {
            
        }

        private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            _SendMessageSource?.Raise(this, new RequestListOfInstalledExtensionsEventArgs() { ReceiverType = ExtensionType.UIComponent });


        }

        private void butEnable_Click(object sender, RoutedEventArgs e)
        {
            dynamic item = ((Button)sender).DataContext;
            var uid = FlickrNet.UtilityMethods.MD5Hash(item.Title);
            var id = 0;
            try
            {
                id = item.Id;
            }
            catch (Exception ex)
            {
                //id doesnt exist so create a new one
            }
            if (id > 0)
                X.Services.Data.StorageService.Instance.Storage.UpdateFieldById<ExtensionManifestDataModel>(id, "IsExtEnabled", 1);
            else
                X.Services.Data.StorageService.Instance.Storage.Insert(new ExtensionManifestDataModel() { Uid = uid, IsExtEnabled = true });

            item.IsExtEnabled = true;
        }


        private void butDisable_Click(object sender, RoutedEventArgs e)
        {
            dynamic item = ((Button)sender).DataContext;
            var uid = FlickrNet.UtilityMethods.MD5Hash(item.Title);
            var id = 0;
            try
            {
                id = item.Id;
            }
            catch (Exception ex){
                //id doesnt exist so create a new one
            }

            
            if(id>0)
                X.Services.Data.StorageService.Instance.Storage.UpdateFieldById<ExtensionManifestDataModel>(id, "IsExtEnabled", 0);
            else
                X.Services.Data.StorageService.Instance.Storage.Insert(new ExtensionManifestDataModel() { Uid = uid, IsExtEnabled = false });

            item.IsExtEnabled = false;

        }

        //private void butClose_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    SendMessage?.Invoke(this, new CloseExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ExtensionManifest.UniqueID });
        //}
    }
}
