using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WeakEvent;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.Extensions;

namespace X.Player
{
    public sealed partial class MainPage : Page
    {
       

        public MainPage()
        {
            this.InitializeComponent();
        }
        
        public async void ProcessArguments(string arguments, string tileId)
        {
            //await ctlShellExt.InitExtensions();
        }
        
        
    }
}
