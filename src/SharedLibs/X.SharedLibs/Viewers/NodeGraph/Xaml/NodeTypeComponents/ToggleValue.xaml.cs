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
    public sealed partial class ToggleValue : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public ToggleValue()
        {
            this.InitializeComponent();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ProcessChange(sender, true);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ProcessChange(sender, false);
        }

        private void ProcessChange(object sender, bool isChecked) {
            if (NodeTypeValueChanged != null)
            {
                NodeNodeLinkModel vm = (NodeNodeLinkModel)((FrameworkElement)sender).DataContext;
                if (vm == null) return;
                vm.OutputNodeLinks[0].Value1 = isChecked ? "1" : "0";
                NodeTypeValueChanged.Invoke(vm.OutputNodeLinks[0].InputNodeKey, EventArgs.Empty);
            }
        }
    }
}
