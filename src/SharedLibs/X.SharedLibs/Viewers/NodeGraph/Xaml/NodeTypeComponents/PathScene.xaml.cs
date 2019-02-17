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



        //public string PathData
        //{
        //    get { return (string)GetValue(PathDataProperty); }
        //    set { SetValue(PathDataProperty, value); }
        //}

        //public static readonly DependencyProperty PathDataProperty =
        //    DependencyProperty.Register("PathData", typeof(string), typeof(PathScene), new PropertyMetadata(string.Empty, (o, a) => {

        //        if (a.NewValue != null)
        //        {

        //        }

        //    }));


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


    public class PathService : DependencyObject
    {

        public static string GetPathData(DependencyObject obj)
        {
            return (string)obj.GetValue(PathDataProperty);
        }

        public static void SetPathData(DependencyObject obj, string value)
        {
            obj.SetValue(PathDataProperty, value);
        }

        public static readonly DependencyProperty PathDataProperty =
            DependencyProperty.RegisterAttached("PathData", typeof(string), typeof(PathService), new PropertyMetadata(string.Empty, (o, a) => {
                if (a.NewValue != null)
                {

                }
            }));
    }
}
