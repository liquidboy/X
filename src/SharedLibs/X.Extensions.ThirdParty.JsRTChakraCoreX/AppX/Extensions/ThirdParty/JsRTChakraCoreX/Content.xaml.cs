using CoreLib.Extensions;
using System;
using WeakEvent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace X.Extensions.ThirdParty.JsRTChakraCoreX
{
    public sealed partial class Content : UserControl, IExtensionContent
    {
       
        public Content()
        {
            this.InitializeComponent();
        }

        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public async void RecieveMessage(object message)
        {
           
        }

        public void Unload()
        {

        }

        private async void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

    
    }
}
