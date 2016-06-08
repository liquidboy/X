using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace X.UI.Chrome
{
    public sealed class Shell : Control
    {
        bool hasInitialized = false;
        Grid _root;
        ContentControl _mainContent;
        ContentControl _ctlBarBottom;
        ContentControl _ctlBarTop;
        ContentControl _ctlBarLeft;
        ContentControl _ctlBarRight;
        ContentControl _ctlOneBox;


        public StackPanel DockedExtensionBottomFull;
        public StackPanel DockedExtensionLeft;
        public StackPanel DockedExtensionRight;
        public StackPanel DockedExtensionTopFull;
        public StackPanel DockedExtensionBottom;
        public StackPanel DockedExtensionTop;

        public Shell()
        {
            this.DefaultStyleKey = typeof(Shell);
            this.Loaded += Shell_Loaded; ;
            this.Unloaded += Shell_Unloaded; ;
        }

        private void Shell_Unloaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            oneTimeInit();
        }


        private bool _IsOneBoxHidden = false;
        public bool IsOneBoxHidden {
            get { return _IsOneBoxHidden; }
            set {
                _IsOneBoxHidden = value;
                if (_IsOneBoxHidden) {
                    DockedExtensionTopFull.Visibility = Visibility.Collapsed;
                    _ctlOneBox.Visibility = Visibility.Collapsed;
                }
                else {
                    DockedExtensionTopFull.Visibility = Visibility.Visible;
                    _ctlOneBox.Visibility = Visibility.Visible;
                }
            }
        }




        protected override void OnApplyTemplate()
        {
            oneTimeInit();
            base.OnApplyTemplate();
        }

        private void oneTimeInit() {
            if (hasInitialized) return;

            if (_root == null) _root = GetTemplateChild("root") as Grid;
            
            if (_root != null) {
                if (_mainContent == null) _mainContent = GetTemplateChild("grdMainContent") as ContentControl;
                if (_ctlBarBottom == null) _ctlBarBottom = GetTemplateChild("ctlExtensionsBarBottom") as ContentControl;
                if (_ctlBarTop == null) _ctlBarTop = GetTemplateChild("ctlExtensionsBarTop") as ContentControl;
                if (_ctlBarLeft == null) _ctlBarLeft = GetTemplateChild("ctlExtensionsBarLeft") as ContentControl;
                if (_ctlBarRight == null) _ctlBarRight = GetTemplateChild("ctlExtensionsBarRight") as ContentControl;
                if (_ctlOneBox == null) _ctlOneBox = GetTemplateChild("ctlOneBox") as ContentControl;
                

                if (DockedExtensionBottomFull == null) DockedExtensionBottomFull = GetTemplateChild("grdDockedExtensionBottomFull") as StackPanel;
                if (DockedExtensionLeft == null) DockedExtensionLeft = GetTemplateChild("grdDockedExtensionLeft") as StackPanel;
                if (DockedExtensionRight == null) DockedExtensionRight = GetTemplateChild("grdDockedExtensionRight") as StackPanel;
                if (DockedExtensionTopFull == null) DockedExtensionTopFull = GetTemplateChild("grdDockedExtensionTopFull") as StackPanel;
                if (DockedExtensionBottom == null) DockedExtensionBottom = GetTemplateChild("grdDockedExtensionBottom") as StackPanel;
                if (DockedExtensionTop == null) DockedExtensionTop = GetTemplateChild("grdDockedExtensionTop") as StackPanel;
            }
            
            if (_root != null) hasInitialized = true;
        }


        public object ContentMain
        {
            get { return (object)GetValue(ContentMainProperty); }
            set { SetValue(ContentMainProperty, value); }
        }
        
        public object BarTop
        {
            get { return (object)GetValue(BarTopProperty); }
            set { SetValue(BarTopProperty, value); }
        }

        public object BarBottom
        {
            get { return (object)GetValue(BarBottomProperty); }
            set { SetValue(BarBottomProperty, value); }
        }

        public object BarLeft
        {
            get { return (object)GetValue(BarLeftProperty); }
            set { SetValue(BarLeftProperty, value); }
        }

        public object BarRight
        {
            get { return (object)GetValue(BarRightProperty); }
            set { SetValue(BarRightProperty, value); }
        }

        public object ContentOneBox
        {
            get { return (object)GetValue(ContentOneBoxProperty); }
            set { SetValue(ContentOneBoxProperty, value); }
        }

        public object ContentExtraTabs
        {
            get { return (object)GetValue(ContentExtraTabsProperty); }
            set { SetValue(ContentExtraTabsProperty, value); }
        }




        public static readonly DependencyProperty ContentExtraTabsProperty = DependencyProperty.Register("ContentExtraTabs", typeof(object), typeof(Shell), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentOneBoxProperty = DependencyProperty.Register("ContentOneBox", typeof(object), typeof(Shell), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentMainProperty = DependencyProperty.Register("ContentMain", typeof(object), typeof(Shell), new PropertyMetadata(null));
        public static readonly DependencyProperty BarTopProperty = DependencyProperty.Register("BarTop", typeof(object), typeof(Shell), new PropertyMetadata(null));
        public static readonly DependencyProperty BarBottomProperty = DependencyProperty.Register("BarBottom", typeof(object), typeof(Shell), new PropertyMetadata(null));
        public static readonly DependencyProperty BarLeftProperty = DependencyProperty.Register("BarLeft", typeof(object), typeof(Shell), new PropertyMetadata(null));
        public static readonly DependencyProperty BarRightProperty = DependencyProperty.Register("BarRight", typeof(object), typeof(Shell), new PropertyMetadata(null));



    }
}
