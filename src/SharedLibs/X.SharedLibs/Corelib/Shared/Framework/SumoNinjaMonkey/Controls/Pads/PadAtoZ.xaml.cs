using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class PadAtoZ : UserControl
    {
        private static char[] base26Chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();


        public PadAtoZ()
        {
            this.InitializeComponent();

            //labels for tiles
            List<object> labels = new List<object>();
            foreach (var ch in base26Chars)
            {
                labels.Add(ch.ToString().ToUpper());
            }
            ttw.LabelsList = labels;

            //enable status of tiles
            List<bool> enabledList = new List<bool>();
            enabledList.Add(true); //A
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true);
            enabledList.Add(true); //Z
            enabledList.Add(false);
            enabledList.Add(false);
            enabledList.Add(false);
            enabledList.Add(false);
            ttw.EnabledList = enabledList;
        }


        private void Pad_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (ttw.IsTilesHidden) ttw.ShowTiles();
            else ttw.HideTiles();

        }
    }
}
