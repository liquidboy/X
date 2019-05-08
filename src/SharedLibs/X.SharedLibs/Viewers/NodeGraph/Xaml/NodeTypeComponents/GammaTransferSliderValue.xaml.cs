using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.UI.NodeGraph;

namespace X.Viewer.NodeGraph.NodeTypeComponents
{
    public sealed partial class GammaTransferSliderValue : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public GammaTransferSliderValue()
        {
            this.InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (NodeTypeValueChanged != null) {
                NodeNodeLinkModel vm = (NodeNodeLinkModel)((FrameworkElement)sender).DataContext;
                if (vm == null) return;
                FrameworkElement el = (FrameworkElement)sender;
                var tag = el.Tag as string;
                var indexToUse = 0;
                switch (tag) {
                    case "ramp": indexToUse = 0; break;
                    case "rexp": indexToUse = 1; break;
                    case "rofs": indexToUse = 2; break;
                    case "gamp": indexToUse = 3; break;
                    case "gexp": indexToUse = 4; break;
                    case "gofs": indexToUse = 5; break;
                    case "bamp": indexToUse = 6; break;
                    case "bexp": indexToUse = 7; break;
                    case "bofs": indexToUse = 8; break;
                    case "aamp": indexToUse = 9; break;
                    case "aexp": indexToUse = 10; break;
                    case "aofs": indexToUse = 11; break;
                }
                try {
                    NodeTypeValueChanged.Invoke(vm.OutputNodeLinks[indexToUse].InputNodeKey, EventArgs.Empty);
                } catch { }
                
            }
        }
    }
}
