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

        //private void butClose_PointerReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    SendMessage?.Invoke(this, new CloseExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ExtensionManifest.UniqueID });
        //}
    }
}
