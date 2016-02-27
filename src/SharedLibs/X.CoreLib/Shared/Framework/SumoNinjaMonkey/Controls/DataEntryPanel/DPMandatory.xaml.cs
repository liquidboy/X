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
    public sealed partial class DPMandatory : UserControl
    {
        public DPMandatory()
        {
            this.InitializeComponent();
        }

        public void Show()
        {
            label.Text = "Mandatory";
            sbShow.Stop();
            sbShow.Completed += sbShow_Completed;
            sbShow.Begin();
        }

        public void Hide()
        {
            label.Text = "Mandatory";
            sbShow.Seek(TimeSpan.FromSeconds(0));
            sbShow.Stop();
        }

        void sbShow_Completed(object sender, object e)
        {
            sbShow.Completed -= sbShow_Completed;
            label.Text = "M";
        }
    }
}
