using CoreLib.Extensions;
using Octokit;
using System;
using WeakEvent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace X.Extensions.ThirdParty.GitX
{
    public sealed partial class Content : UserControl, IExtensionContent
    {
        public User UserData { get; set; }

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
            
            UserData = await InstanceLocator.Instance.GitClient.User.Get("liquidboy");
            
            profileView.DataContext = UserData;

        }

        private void loginView_OnLoggedIn(object sender, EventArgs e)
        {
            loginView.Visibility = Visibility.Collapsed;
            profileView.Visibility = Visibility.Visible;
            repositoryView.Visibility = Visibility.Visible;
            notificationsView.Visibility = Visibility.Visible;
        }
    }
}
