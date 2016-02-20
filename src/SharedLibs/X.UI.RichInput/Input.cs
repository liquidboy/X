using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace X.UI.RichInput
{
    


    public sealed class Input : ItemsControl
    {
        Grid _grdContainer;
        Grid _grdRoot;
        int RenderTargetIndexFor_grdRoot = 0;
        ContentControl _ccInput;
        Style _GeneralTextBoxStyle;
        Style _GeneralPasswordBoxStyle;
        Style _GeneralCheckBoxStyle;
        Style _GeneralRadioButtonStyle;
        Style _GeneralComboBoxStyle;
        Style _GeneralToggleSwitchStyle;
        Style _GeneralProgressBarStyle;
        Style _GeneralProgressRingStyle;
        Style _GeneralSliderStyle;
        TextBox _udfTB1;
        TextBlock _udfTBL1;
        ComboBox _udfCB1;
        RadioButton _udfRB1;
        CheckBox _udfChkB1;
        ToggleSwitch _udfTS1;
        ProgressBar _udfProgBr1;
        ProgressRing _udfProgRn1;
        Slider _udfSl1;
        Grid _udfg1;
        PasswordBox _udfPB1;
        EffectLayer.EffectLayer _bkgLayer;//x

        Storyboard _sbHideBgLayer;
        Storyboard _sbShowBgLayer;

        InputModel _model;

        double bkgOffsetX = 0;
        double bkgOffsetY = 0;

        public event Windows.UI.Xaml.RoutedEventHandler ValueChanged;

        DispatcherTimer dtInvalidate;

        public string Value { get; set; }

        public Input()
        {
            this.DefaultStyleKey = typeof(Input);

            Loaded += Input_Loaded;
            Unloaded += Input_Unloaded;
        }

        public void Invalidate(double offsetX = 0, double offsetY = 0) { _bkgLayer?.DrawUIElements(_grdRoot, RenderTargetIndexFor_grdRoot, offsetX, offsetY); }


        private void Input_Unloaded(object sender, RoutedEventArgs e)
        {
            UnloadControl(Type);
        }

        private void Input_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bkgLayer != null)
            {
                if (Type == InputType.radio || Type == InputType.checkbox)
                {
                    bkgOffsetY = 2;
                    bkgOffsetX = 0;
                }

                _bkgLayer.DrawUIElements(_grdRoot);  //will draw at index 0 (RenderTargetIndexFor_icTabList)
                _bkgLayer.InitLayer(_grdRoot.ActualWidth, _grdRoot.ActualHeight, bkgOffsetX, bkgOffsetY);
            }


            //if(Type== InputType.progress)
            //{

            //}
        }

        protected override void OnApplyTemplate()
        {
            if (_model == null) _model = new InputModel(); 

            if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;
            if (_grdContainer == null) _grdContainer = GetTemplateChild("grdContainer") as Grid;
            
            if (_grdRoot == null) {
                _grdRoot = GetTemplateChild("grdRoot") as Grid;
                _grdRoot.DataContext = _model;
            }

            if (_GeneralTextBoxStyle == null) _GeneralTextBoxStyle = (Style)_grdRoot.Resources["GeneralTextBoxStyle"]; 
            if (_GeneralPasswordBoxStyle == null) _GeneralPasswordBoxStyle = (Style)_grdRoot.Resources["GeneralPasswordBoxStyle"]; 
            if (_GeneralCheckBoxStyle == null) _GeneralCheckBoxStyle = (Style)_grdRoot.Resources["GeneralCheckBoxStyle"];
            if (_GeneralRadioButtonStyle == null) _GeneralRadioButtonStyle = (Style)_grdRoot.Resources["GeneralRadioButtonStyle"];
            if (_GeneralComboBoxStyle == null) _GeneralComboBoxStyle = (Style)_grdRoot.Resources["GeneralComboBoxStyle"];
            if (_GeneralToggleSwitchStyle == null) _GeneralToggleSwitchStyle = (Style)_grdRoot.Resources["GeneralToggleSwitchStyle"];
            if (_GeneralProgressBarStyle == null) _GeneralProgressBarStyle = (Style)_grdRoot.Resources["GeneralProgressBarStyle"];
            if (_GeneralProgressRingStyle == null) _GeneralProgressRingStyle = (Style)_grdRoot.Resources["GeneralProgressRingStyle"];
            if (_GeneralSliderStyle == null) _GeneralSliderStyle = (Style)_grdRoot.Resources["GeneralSliderStyle"];
            
            if (_sbHideBgLayer == null)
            {
                _sbHideBgLayer = (Storyboard)_grdContainer.Resources["sbHideBgLayer"];
                _sbShowBgLayer = (Storyboard)_grdContainer.Resources["sbShowBgLayer"];
            }
            



            if (_ccInput == null) {
                _ccInput = GetTemplateChild("ccInput") as ContentControl;
                
                BuildControl(Type, Label, PlaceholderText, LabelFontSize, LabelTranslateY, GroupName,  _ccInput);
                SetColors(FocusColor, FocusHoverColor, FocusForegroundColor, _model);
                //SetColors();
            }


            if (Type == InputType.radio || Type == InputType.checkbox)
            {
                bkgOffsetY = 2;
                bkgOffsetX = 0;
            }
            else if (Type == InputType.toggleSwitch || Type == InputType.progress || Type== InputType.progressRing || Type == InputType.slider) {

                dtInvalidate = new DispatcherTimer();
                dtInvalidate.Interval = TimeSpan.FromMilliseconds(500);
                dtInvalidate.Tick += DtInvalidate_Tick;
            }

            if (_bkgLayer != null && _grdRoot != null && _grdRoot.ActualWidth != 0) _bkgLayer.InitLayer(_grdRoot.ActualWidth, _grdRoot.ActualHeight, bkgOffsetX, bkgOffsetY);

            base.OnApplyTemplate();
        }

 
        private void BuildControl(InputType type, string label, string placeholderText, double labelFontSize, double labelTranslateY, string groupName, ContentControl ccInput) {

            FrameworkElement fe = null;

            if (type == InputType.text)
            {
                _udfTB1 = new TextBox();
                _udfTB1.PlaceholderText = placeholderText;
                _udfTB1.Style = _GeneralTextBoxStyle;
                _udfTB1.KeyUp += ittext_KeyUp;

                _udfg1 = new Grid() { Padding = new Thickness(2, 2, 2, 2), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment= VerticalAlignment.Top};
                _udfTBL1 = new TextBlock();
                _udfTBL1.Text = label;
                _udfTBL1.FontSize = labelFontSize;
                _udfg1.Margin = new Thickness(0, labelTranslateY, 0, 0);
                _udfg1.Visibility = Visibility.Collapsed;
                _udfg1.Children.Add(_udfTBL1);
                
                var gd = new Grid();
                gd.Children.Add(_udfg1);
                gd.Children.Add(_udfTB1);

                fe = gd;
            }
            else if (type == InputType.password)
            {
                _udfPB1 = new PasswordBox();
                _udfPB1.PlaceholderText = placeholderText;
                _udfPB1.Style = _GeneralPasswordBoxStyle;
                _udfPB1.PasswordChanged += itpassword_PasswordChanged;

                _udfg1 = new Grid() { Padding = new Thickness(2, 2, 2, 2), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
                _udfTBL1 = new TextBlock();
                _udfTBL1.Text = label;
                _udfTBL1.FontSize = labelFontSize;
                _udfg1.Margin = new Thickness(0, labelTranslateY, 0, 0);
                _udfg1.Visibility = Visibility.Collapsed;
                _udfg1.Children.Add(_udfTBL1);

                var gd = new Grid();
                gd.Children.Add(_udfg1);
                gd.Children.Add(_udfPB1);

                fe = gd;
            }
            else if (type == InputType.combobox)
            {
                _udfCB1 = new ComboBox();
                _udfCB1.Style = _GeneralComboBoxStyle;
                _udfCB1.PlaceholderText = placeholderText;
                _udfCB1.SetBinding(ComboBox.ItemsSourceProperty, new Binding() { Source = Items });
                _udfCB1.Width = this.Width;
                _udfCB1.SelectionChanged += itcombobox_SelectionChanged;

                _udfg1 = new Grid() { Padding = new Thickness(2, 2, 2, 2), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
                _udfTBL1 = new TextBlock();
                _udfTBL1.Text = label;
                _udfTBL1.FontSize = labelFontSize;
                _udfg1.Margin = new Thickness(0, labelTranslateY, 0, 0);
                _udfg1.Visibility = Visibility.Collapsed;
                _udfg1.Children.Add(_udfTBL1);

                var gd = new Grid();
                gd.Children.Add(_udfg1);
                gd.Children.Add(_udfCB1);

                fe = gd;
            }
            else if (type == InputType.checkbox)
            {
                var sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                
                var lb = new TextBlock();
                lb.Text = label;
                lb.FontSize = LabelFontSize;
                lb.Margin = new Thickness(0, LabelTranslateY, 0, 0);

                _udfChkB1 = new CheckBox();
                _udfChkB1.Checked += itcheckbox_Changed;
                _udfChkB1.Unchecked += itcheckbox_Changed;
                _udfChkB1.Content = lb;
                _udfChkB1.Style = _GeneralCheckBoxStyle;
                sp.Children.Add(_udfChkB1);

                fe = sp;
            }
            else if (type == InputType.toggleSwitch)
            {
                _udfTS1 = new ToggleSwitch();
                _udfTS1.Style = _GeneralToggleSwitchStyle;
                _udfTS1.Toggled += ittoggleswitch_Toggled;
                _udfTS1.FontSize = FontSize;
                _udfTS1.OnContent = Content1;
                _udfTS1.OffContent = Content2;
                fe = _udfTS1;
            }
            else if (type == InputType.radio)
            {
                var sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                
                var lb = new TextBlock();
                lb.Text = label;
                lb.FontSize = LabelFontSize;
                lb.Margin = new Thickness(0, LabelTranslateY, 0, 0);

                _udfRB1 = new RadioButton();
                _udfRB1.GroupName = groupName;
                _udfRB1.Checked += itradio_Changed;
                _udfRB1.Unchecked += itradio_Changed;
                _udfRB1.Content = lb;
                _udfRB1.Style = _GeneralRadioButtonStyle;
                sp.Children.Add(_udfRB1);

                fe = sp;
            }
            else if (type == InputType.progress)
            {
                _udfProgBr1 = new ProgressBar();
                _udfProgBr1.Style = _GeneralProgressBarStyle;
                _udfProgBr1.ValueChanged += itProgBr_ValueChanged;
                _udfProgBr1.FontSize = FontSize;
                _udfProgBr1.DataContext = this;
                _udfProgBr1.SetBinding(ProgressBar.MaximumProperty, new Binding() { Path= new PropertyPath("Maximum1") });
                _udfProgBr1.SetBinding(ProgressBar.MinimumProperty, new Binding() { Path = new PropertyPath("Minimum1") });
                _udfProgBr1.SetBinding(ProgressBar.ValueProperty, new Binding() { Path = new PropertyPath("Value1")  });
                _udfProgBr1.SetBinding(ProgressBar.SmallChangeProperty, new Binding() { Path = new PropertyPath("SmallChange1") });
                _udfProgBr1.SetBinding(ProgressBar.LargeChangeProperty, new Binding() { Path = new PropertyPath("LargeChange1") });
                
                fe = _udfProgBr1;
            }
            else if (type == InputType.progressRing)
            {
                _udfProgRn1 = new ProgressRing();
                _udfProgRn1.Style = _GeneralProgressRingStyle;
                //_udfProgRn1.val += itProgBr_ValueChanged;
                _udfProgRn1.FontSize = FontSize;
                _udfProgRn1.DataContext = this;
                

                fe = _udfProgRn1;
            }
            else if (type == InputType.slider)
            {
                _udfSl1 = new Slider();
                _udfSl1.Style = _GeneralSliderStyle;
                _udfSl1.ValueChanged += itSl_ValueChanged;
                _udfSl1.FontSize = FontSize;
                _udfSl1.DataContext = this;
                _udfSl1.SetBinding(Slider.MaximumProperty, new Binding() { Path = new PropertyPath("Maximum1") });
                _udfSl1.SetBinding(Slider.MinimumProperty, new Binding() { Path = new PropertyPath("Minimum1") });
                _udfSl1.SetBinding(Slider.ValueProperty, new Binding() { Path = new PropertyPath("Value1") });
                _udfSl1.SetBinding(Slider.SmallChangeProperty, new Binding() { Path = new PropertyPath("SmallChange1") });
                _udfSl1.SetBinding(Slider.StepFrequencyProperty, new Binding() { Path = new PropertyPath("SmallChange1") });
                _udfSl1.SetBinding(Slider.LargeChangeProperty, new Binding() { Path = new PropertyPath("LargeChange1") });

                fe = _udfSl1;
            }



            fe.HorizontalAlignment = HorizontalAlignment.Stretch;
            fe.VerticalAlignment = VerticalAlignment.Stretch;
            ccInput.Content = fe;

        }

 

        private async void SetColors(Color focusColor, Color focusHoverColor, Color focusForegroundColor, InputModel model) 
        {

            model.FocusColor = focusColor;
            model.FocusHoverColor = focusHoverColor;
            model.FocusForegroundColor = focusForegroundColor;

            if (_udfg1 != null) _udfg1.Background = new SolidColorBrush(focusColor);
            if (_udfCB1 != null)
            {
                _udfCB1.BorderBrush = new SolidColorBrush(focusColor);
                _udfCB1.Foreground = new SolidColorBrush(focusColor);
                //var border = _udfCB1.FindName("PopupBorder") as Border;
                //if (border != null) {
                //    border.Background = new SolidColorBrush(focusColor);
                //    border.BorderBrush = new SolidColorBrush(focusColor);
                //}                
            }

            if (_udfTB1 != null)
            {
                _udfTB1.Foreground = new SolidColorBrush(focusColor);
                _udfTB1.BorderBrush = new SolidColorBrush(focusColor);
            }

            if (_udfPB1!= null)
            {
                _udfPB1.Foreground = new SolidColorBrush(focusColor);
                _udfPB1.BorderBrush = new SolidColorBrush(focusColor);
            }

            if (_udfChkB1 != null)
            {
                _udfChkB1.Foreground = new SolidColorBrush(focusColor);
            }

            if (_udfRB1 != null)
            {
                _udfRB1.Foreground = new SolidColorBrush(focusColor);
            }

            if (_udfProgBr1 != null)
            {
                _udfProgBr1.Foreground = new SolidColorBrush(focusColor);
                _udfProgBr1.Background = new SolidColorBrush(focusHoverColor);
            }

            if (_udfSl1 != null)
            {
                _udfSl1.Foreground = new SolidColorBrush(focusColor);
                _udfSl1.Background = new SolidColorBrush(focusHoverColor);
            }


            if (Type == InputType.text || Type == InputType.password || Type == InputType.combobox)
            {
                if (_udfTBL1 != null) _udfTBL1.Foreground = new SolidColorBrush(focusForegroundColor);
            }

            //if (_udfg1 != null) _udfg1.SetBinding(Grid.BackgroundProperty, new Binding() { Path = new PropertyPath("FocusColor") });
            //if (_udfCB1 != null) _udfCB1.SetBinding(ComboBox.ForegroundProperty , new Binding() { Path = new PropertyPath("FocusColor") });

            //if (Type == InputType.text || Type == InputType.password || Type == InputType.combobox)
            //{
            //    if (_udfTBL1 != null) _udfTBL1.SetBinding(TextBlock.ForegroundProperty, new Binding() { Path = new PropertyPath("FocusForegroundColor") });
            //}
        }

        private void UpdateControl(InputType type, string label, string placeholderText, double labelFontSize, double labelTranslateY, string groupName, ContentControl ccInput)
        {
            throw new NotImplementedException();
        }

        private void UnloadControl(InputType type)
        {

            if (type == InputType.text)
            {
                _udfTB1.KeyUp -= ittext_KeyUp;
            }
            else if (type == InputType.password)
            {
                _udfPB1.PasswordChanged -= itpassword_PasswordChanged;
            }
            else if (type == InputType.checkbox)
            {

            }
            else if (type == InputType.radio)
            {

            }
            else if (type == InputType.combobox)
            {
                var sp = (StackPanel)_ccInput.Content;
                var cb = (ComboBox)sp.Children[0];
                cb.SelectionChanged -= itcombobox_SelectionChanged;
                if (cb.Items != null && cb.Items.Count > 0) cb.Items.Clear();
                if (cb.ItemsSource != null) cb.ItemsSource = null;
            }
            if (_udfTS1 != null) {
                _udfTS1.Toggled -= ittoggleswitch_Toggled;
            }
            if (_udfProgBr1 != null)
            {
                _udfProgBr1.ValueChanged -= itProgBr_ValueChanged;
            }
            if (_udfSl1 != null)
            {
                _udfSl1.ValueChanged -= itSl_ValueChanged;
            }
            if (_udfRB1 != null)
            {
                _udfRB1.Checked -= itradio_Changed;
                _udfRB1.Unchecked -= itradio_Changed;
            }
            if (_udfChkB1 != null)
            {
                _udfChkB1.Checked -= itcheckbox_Changed;
                _udfChkB1.Unchecked -= itcheckbox_Changed;
            }

            if (_udfg1 != null)
            {
                _udfg1 = null;
                _udfg1.Children.Clear();
            }

            if (dtInvalidate != null) {
                dtInvalidate.Stop();
                dtInvalidate.Tick += DtInvalidate_Tick;
            }

            _sbHideBgLayer?.Stop();
            _sbHideBgLayer = null;
            _sbShowBgLayer?.Stop();
            _sbShowBgLayer = null;

            _udfProgBr1 = null;
            _udfPB1 = null;
            _udfTB1 = null;
            _udfTBL1 = null;
            _udfCB1 = null;
            _udfChkB1 = null;
            _udfRB1 = null;
            _udfTS1 = null;
            dtInvalidate = null;
            _ccInput.Content = null;
            _grdContainer = null;
            _model = null;

        }










        //==============================
        // EVENTS
        //==============================
        private void itSl_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (dtInvalidate != null)
            {
                _sbHideBgLayer?.Begin();
                dtInvalidate.Start();
                //Invalidate();
            }
        }

        private void itProgBr_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (dtInvalidate!= null)
            {
                //_sbHideBgLayer?.Begin();
                //dtInvalidate.Start();
                Invalidate();
            }
        }

        private void DtInvalidate_Tick(object sender, object e)
        {
            dtInvalidate.Stop();
            _sbShowBgLayer?.Begin();
            Invalidate();
        }


        private void ittoggleswitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (Type == InputType.toggleSwitch)
            {
                Value = ((ToggleSwitch)sender).IsOn.ToString();
                ValueChanged?.Invoke(sender, e);
                _sbHideBgLayer?.Begin();
                dtInvalidate.Start();
                Invalidate();
            }
        }


        private void itradio_Changed(object sender, RoutedEventArgs e)
        {
            if (Type == InputType.radio)
            {
                Value = ((RadioButton)sender).IsChecked.ToString();
                ValueChanged?.Invoke(sender, e);
                
                Invalidate();
            }
        }


        private void itcheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (Type == InputType.checkbox)
            {
                Value = ((CheckBox)sender).IsChecked.ToString();
                ValueChanged?.Invoke(sender, e);

                Invalidate();
            }
        }


        private void itpassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Type == InputType.password)
            {
                var offsetX = 0;
                var offsetY = 0;

                if (((PasswordBox)sender).Password.Length > 0)
                {
                    _udfg1.Visibility = Visibility.Visible;
                    offsetY = -15;
                }
                else {
                    _udfg1.Visibility = Visibility.Collapsed;
                }

                Value = ((PasswordBox)sender).Password;
                ValueChanged?.Invoke(sender, e);
                
                Invalidate(offsetX, offsetY);
            }
        }

        private void ittext_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(Type == InputType.text)
            {
                var offsetX = 0;
                var offsetY = 0;

                if (((TextBox)sender).Text.Length > 0)
                {
                    _udfg1.Visibility = Visibility.Visible;
                    offsetY = -15;
                }
                else {
                    _udfg1.Visibility = Visibility.Collapsed;
                }

                Value = ((TextBox)sender).Text;
                ValueChanged?.Invoke(sender, e);

                Invalidate(offsetX, offsetY);
            }
            
        }

        private void itcombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Type == InputType.combobox)
            {
                var offsetX = 0;
                var offsetY = 0;

                if (((ComboBox)sender).SelectedItem != null)
                {
                    _udfg1.Visibility = Visibility.Visible;
                    offsetY = -15;
                }
                else {
                    _udfg1.Visibility = Visibility.Collapsed;
                }

                ValueChanged?.Invoke(sender, e);

                try {
                    Invalidate(offsetX, offsetY);
                } catch (Exception ex){
                    //todo: handle this error
                }
                
            }
        }



















        public InputType Type
        {
            get { return (InputType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        
        public double LabelFontSize
        {
            get { return (double)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }
        
        public double LabelTranslateY
        {
            get { return (double)GetValue(LabelTranslateYProperty); }
            set { SetValue(LabelTranslateYProperty, value); }
        }
        
        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        public Color FocusColor
        {
            get { return (Color)GetValue(FocusColorProperty); }
            set { SetValue(FocusColorProperty, value); }
        }

        public Color FocusHoverColor
        {
            get { return (Color)GetValue(FocusHoverColorProperty); }
            set { SetValue(FocusHoverColorProperty, value); }
        }
        
        public Color FocusForegroundColor
        {
            get { return (Color)GetValue(FocusForegroundColorProperty); }
            set { SetValue(FocusForegroundColorProperty, value); }
        }
        
        public Color GlowColor
        {
            get { return (Color)GetValue(GlowColorProperty); }
            set { SetValue(GlowColorProperty, value); }
        }
        
        public double GlowAmount
        {
            get { return (double)GetValue(GlowAmountProperty); }
            set { SetValue(GlowAmountProperty, value); }
        }
        
        public Object Content1
        {
            get { return (Object)GetValue(Content1Property); }
            set { SetValue(Content1Property, value); }
        }
        
        public Object Content2
        {
            get { return (Object)GetValue(Content2Property); }
            set { SetValue(Content2Property, value); }
        }
        
        public double Minimum1
        {
            get { return (double)GetValue(Minimum1Property); }
            set { SetValue(Minimum1Property, value); }
        }
        
        public double Maximum1
        {
            get { return (double)GetValue(Maximum1Property); }
            set { SetValue(Maximum1Property, value); }
        }
        
        public double SmallChange1
        {
            get { return (double)GetValue(SmallChange1Property); }
            set { SetValue(SmallChange1Property, value); }
        }
        
        public double LargeChange1
        {
            get { return (double)GetValue(LargeChange1Property); }
            set { SetValue(LargeChange1Property, value); }
        }
        
        public double Value1
        {
            get { return (double)GetValue(Value1Property); }
            set { SetValue(Value1Property, value); }
        }























        public static readonly DependencyProperty Value1Property = DependencyProperty.Register("Value1", typeof(double), typeof(Input), new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty LargeChange1Property = DependencyProperty.Register("LargeChange1", typeof(double), typeof(Input), new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty SmallChange1Property = DependencyProperty.Register("SmallChange1", typeof(double), typeof(Input), new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty Maximum1Property = DependencyProperty.Register("Maximum1", typeof(double), typeof(Input), new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty Minimum1Property = DependencyProperty.Register("Minimum1", typeof(double), typeof(Input), new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty Content2Property = DependencyProperty.Register("Content2", typeof(Object), typeof(Input), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty Content1Property = DependencyProperty.Register("Content1", typeof(Object), typeof(Input), new PropertyMetadata(null, OnPropertyChanged));

        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(Input), new PropertyMetadata(3));

        public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register("GlowColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.Black));

        public static readonly DependencyProperty FocusForegroundColorProperty = DependencyProperty.Register("FocusForegroundColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.White, OnPropertyChanged));

        public static readonly DependencyProperty FocusHoverColorProperty = DependencyProperty.Register("FocusHoverColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.DarkGray, OnPropertyChanged));

        public static readonly DependencyProperty FocusColorProperty = DependencyProperty.Register("FocusColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.DarkGray, OnPropertyChanged));

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(Input), new PropertyMetadata(string.Empty, OnPropertyChanged));
        
        public static readonly DependencyProperty LabelTranslateYProperty = DependencyProperty.Register("LabelTranslateY", typeof(double), typeof(Input), new PropertyMetadata(0d, OnPropertyChanged));
        
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register("LabelFontSize", typeof(double), typeof(Input), new PropertyMetadata(9d, OnPropertyChanged));

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register("PlaceholderText", typeof(string), typeof(Input), new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(Input), new PropertyMetadata(string.Empty, OnPropertyChanged));
        
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(InputType), typeof(Input), new PropertyMetadata(0, OnPropertyChanged));







        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as Input;
            if (d == null)
                return;

            if (instance._ccInput != null)
            {
                //instance.UpdateControl(instance.Type, instance.Label, instance.PlaceholderText, instance.LabelFontSize, instance.LabelTranslateY, instance.GroupName, instance._ccInput);

                //instance.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                    instance.SetColors(instance.FocusColor, instance.FocusHoverColor, instance.FocusForegroundColor, instance._model);
                //});
                

                //((UIElement)d).UpdateLayout();
            }
        }


     

    }

 




}
