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
    public sealed partial class SliderValue : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public SliderValue()
        {
            this.InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (NodeTypeValueChanged != null) {
                NodeNodeLinkModel vm = (NodeNodeLinkModel)((FrameworkElement)sender).DataContext;
                if (vm == null) return;
                if (vm.OutputNodeLinks.Count > 0) {
                    NodeTypeValueChanged.Invoke(vm.OutputNodeLinks[0].InputNodeKey, EventArgs.Empty);
                }
            }
        }
    }
}
