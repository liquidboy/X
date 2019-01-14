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

namespace X.Viewer.NodeGraph.NodeTypeComponents
{
    public sealed partial class ColorSliderValue : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public ColorSliderValue()
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
                    case "r": indexToUse = 0; break;
                    case "g": indexToUse = 1; break;
                    case "b": indexToUse = 2; break;
                    case "a": indexToUse = 3; break;
                }
                NodeTypeValueChanged.Invoke(vm.OutputNodeLinks[indexToUse].InputNodeKey, EventArgs.Empty);
            }
        }
    }
}
