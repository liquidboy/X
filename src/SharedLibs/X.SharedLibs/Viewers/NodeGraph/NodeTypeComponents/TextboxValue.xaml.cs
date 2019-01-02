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
    public sealed partial class TextboxValue : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public TextboxValue()
        {
            this.InitializeComponent();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (NodeTypeValueChanged != null)
            {
                NodeNodeLinkModel vm = (NodeNodeLinkModel)((FrameworkElement)sender).DataContext;
                NodeTypeValueChanged.Invoke(vm.OutputNodeLinks[0].InputNodeKey, EventArgs.Empty);
            }
        }
    }
}
