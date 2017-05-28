using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace X.NeonShell.Controls
{
    public sealed partial class ContextMenuView : UserControl
    {
        //public event EventHandler OnClosing;
        //private DispatcherTimer dtClose;

        private Brush _normalIconColor = new SolidColorBrush(Colors.Purple);

        private GeneralSystemWideMessage _msgToPassAlong;
        
  







        public void  Show()
        {
            sbShow.Begin();
            //sbCountdown.Begin();

        }


        public void Hide()
        {
            sbHide.Begin();
            sbHide.Completed += (o, args) => { 
                //if (OnClosing != null) OnClosing(this, EventArgs.Empty);
                //sbCountdown.Stop();
                sbHide.Stop();
                sbShow.Stop();
                //dtClose = null; 
            };
        }


        public string YesMessengerContent { get; set; }
        public string YesMessengerIdentifier { get; set; }
        public string NoMessengerContent { get; set; }
        public string NoMessengerIdentifier { get; set; }

        public ContextMenuView(
            string msg, 
            string title,           
            GeneralSystemWideMessage msgToPassAlong = null
            )
        {
            this.InitializeComponent();


            _msgToPassAlong = msgToPassAlong;

            
        }


        public void LoadMainContent(UserControl content) {
            mainContent.Content = content;
        }
      
    }
}
