using FavouriteMX.Shared.Services;
using SumoNinjaMonkey.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CoreLib.Shared.F12
{

    public enum eDockPosition
    {
        bottom,
        right,
        left,
        top
    }

    public static class F12Service
    {



        public static F12Tools F12toolsInstance;

        public static bool IsStarted = false;
        public static eDockPosition CurrentDockPosition;

        public static void Start(ContentControl f12panel) {
            if (F12toolsInstance == null) F12toolsInstance = new F12Tools(f12panel);


            F12toolsInstance.F12Panel.Content = new ConsoleUI();
            F12toolsInstance.F12Panel.Visibility = Windows.UI.Xaml.Visibility.Visible;

            IsStarted = true;
        }


        public static void Stop() {
            F12toolsInstance.F12Panel.Content = null;
            IsStarted = false;
        }

        public static void DockPanelTo(eDockPosition dockPosition,   double panelSize) {

            if (dockPosition == eDockPosition.right)  
            {
                F12toolsInstance.F12Panel.Width = panelSize;
                F12toolsInstance.F12Panel.Height =  double.NaN;
                F12toolsInstance.F12Panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                F12toolsInstance.F12Panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            }
            else if (dockPosition == eDockPosition.bottom)
            {
                F12toolsInstance.F12Panel.Width = double.NaN;
                F12toolsInstance.F12Panel.Height = panelSize;
                F12toolsInstance.F12Panel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                F12toolsInstance.F12Panel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;

            }


            CurrentDockPosition = dockPosition;
        }
    }


    public class F12Tools
    {
        public ContentControl F12Panel { get; set; }


        public F12Tools(ContentControl f12Panel) {
            F12Panel = f12Panel;
        }
    }
}
