﻿using System;
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
using X.UI.NodeGraph;

namespace X.Viewer.NodeGraph.NodeTypeComponents
{
    public sealed partial class XamlFragment : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public XamlFragment()
        {
            this.InitializeComponent();
        }


        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var nnlm = (NodeNodeLinkModel)args.NewValue;
            try {
                grdMain.Children.Clear();
                var xaml = (string)nnlm.InputNodeLinks[0].Value1;
                xaml = xaml.Replace("<Path ", @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" ");
                var newUIElement = XamlReader.Load(xaml);
                grdMain.Children.Add((UIElement)newUIElement);
            }
            catch (Exception ex) {

            }


        }

    }
}
