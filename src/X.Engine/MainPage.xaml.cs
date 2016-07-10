using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.Engine
{

    public sealed partial class MainPage : Page
    {
        bool running = true;
        D3D12Pipeline _pipeline;

        public MainPage()
        {
            this.InitializeComponent();

            layoutRoot.Unloaded += LayoutRoot_Unloaded;
        }

        private void LayoutRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            _pipeline = null;
        }

        private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            _pipeline = new D3D12Pipeline();

            _pipeline.InitPipeline(Window.Current.CoreWindow, (int)Window.Current.Bounds.Width, (int)Window.Current.Bounds.Height);

            DoWorkAsyncInfiniteLoop(_pipeline);
            
        }



        private async Task DoWorkAsyncInfiniteLoop(D3D12Pipeline pipeline)
        {
            while (true)
            {
                // do the work in the loop
                pipeline.Update();

                // update the UI
                pipeline.Render();
                
                // don't run again for at least 200 milliseconds
                await Task.Delay(30);
            }
        }

    }
}
