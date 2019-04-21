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
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.Viewer.NodeGraph.NodeTypeComponents
{
    public sealed partial class CloudNodeTypeComponent : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public CloudNodeTypeComponent()
        {
            this.InitializeComponent();
        }


        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var nnlm = (NodeNodeLinkModel)args.NewValue;
            try {
                if (!string.IsNullOrEmpty(nnlm.Node.Udfs2)) {
                    grdMain.Children.Clear();
                    var xaml = $@"<Grid xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" Margin=""10"">{(string)nnlm.Node.Udfs2}</Grid>";
                    var newUIElement = XamlReader.Load(xaml);
                    grdMain.Children.Add((UIElement)newUIElement);
                }
            }
            catch (Exception ex) {

            }
        }

    }
}
