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


namespace X.UI.Skeuomorphism
{
    public sealed partial class SilverlightPanel : UserControl
    {
        public SilverlightPanel()
        {
            this.InitializeComponent();
        }



        public UIElement SourceObject
        {
            get { return (UIElement)GetValue(SourceObjectProperty); }
            set { SetValue(SourceObjectProperty, value); }
        }

        public static readonly DependencyProperty SourceObjectProperty =
            DependencyProperty.Register("SourceObject", typeof(UIElement), typeof(SilverlightPanel), new PropertyMetadata(null));


    }
}
