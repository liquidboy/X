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
    public sealed partial class PathScene : UserControl, INodeTypeComponent
    {
        public event EventHandler NodeTypeValueChanged;
        public PathScene()
        {
            this.InitializeComponent();
        }

        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var nnlm = (NodeNodeLinkModel)args.NewValue;
            try {
                grdMain.Children.Clear();
                var xaml = (string)nnlm.InputNodeLinks[1].Value1;
                xaml = xaml.Replace("<Path ", @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" ");
                var newUIElement = XamlReader.Load(xaml);
                grdMain.Children.Add((UIElement)newUIElement);
            }
            catch (Exception ex) {

            }

            //PointAnimationUsingPath
            //PointAnimationBase
            //TimelineMarker
        }


    }

}

// https://www.google.com/search?ei=JhJpXKnFA8r_rQHLkoPQAg&q=uwp+animate+along+a+path&oq=uwp+animate+along+a+path&gs_l=psy-ab.3..33i22i29i30.740827.743534..743675...0.0..1.339.4313.0j6j11j2......0....1..gws-wiz.......0i71j0i67j0j0i22i30j33i160.N-C8fbSoAhY
// https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/how-to-animate-an-object-along-a-path-double-animation

