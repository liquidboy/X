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

namespace X.UI.RichInput
{
    


    public sealed class Input : ItemsControl
    {
        Grid _grdRoot;
        int RenderTargetIndexFor_grdRoot = 0;
        ContentControl _ccInput;
        Style _GeneralTextBoxStyle;
        Style _GeneralPasswordBoxStyle;
        Style _GeneralCheckBoxStyle;
        Style _GeneralRadioButtonStyle;
        Style _GeneralComboBoxStyle;
        TextBox _udfTB1;
        TextBlock _udfTBL1;
        ComboBox _udfCB1;
        RadioButton _udfRB1;
        CheckBox _udfChkB1;
        Grid _udfg1;
        PasswordBox _udfPB1;
        EffectLayer.EffectLayer _bkgLayer;//x

        InputModel _model;


        public event Windows.UI.Xaml.RoutedEventHandler ValueChanged;

        public string Value { get; set; }

        public Input()
        {
            this.DefaultStyleKey = typeof(Input);

            Loaded += Input_Loaded;
            Unloaded += Input_Unloaded;
        }

        public void Invalidate() { _bkgLayer?.DrawUIElements(_grdRoot, RenderTargetIndexFor_grdRoot); }


        private void Input_Unloaded(object sender, RoutedEventArgs e)
        {
            UnloadControl(Type);
        }

        private void Input_Loaded(object sender, RoutedEventArgs e)
        {
            if (_bkgLayer != null)
            {
                _bkgLayer.DrawUIElements(_grdRoot);  //will draw at index 0 (RenderTargetIndexFor_icTabList)
                _bkgLayer.InitLayer(_grdRoot.ActualWidth, _grdRoot.ActualHeight);
            }
        }

        protected override void OnApplyTemplate()
        {
            if (_model == null)
            {
                _model = new InputModel();
            }

            if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as EffectLayer.EffectLayer;

            if (_grdRoot == null) {
                _grdRoot = GetTemplateChild("grdRoot") as Grid;
                _grdRoot.DataContext = _model;
            }

            if (_GeneralTextBoxStyle == null)
            {
                _GeneralTextBoxStyle = (Style)_grdRoot.Resources["GeneralTextBoxStyle"];
            }

            if (_GeneralPasswordBoxStyle == null)
            {
                _GeneralPasswordBoxStyle = (Style)_grdRoot.Resources["GeneralPasswordBoxStyle"];
            }

            if (_GeneralCheckBoxStyle == null)
            {
                _GeneralCheckBoxStyle = (Style)_grdRoot.Resources["GeneralCheckBoxStyle"];
            }

            if (_GeneralRadioButtonStyle == null)
            {
                _GeneralRadioButtonStyle = (Style)_grdRoot.Resources["GeneralRadioButtonStyle"];
            }

            if (_GeneralComboBoxStyle == null)
            {
                _GeneralComboBoxStyle = (Style)_grdRoot.Resources["GeneralComboBoxStyle"];
            }
            

            if (_ccInput == null) {
                _ccInput = GetTemplateChild("ccInput") as ContentControl;
                
                BuildControl(Type, Label, PlaceholderText, LabelFontSize, LabelTranslateY, GroupName,  _ccInput);
                SetColors(FocusColor, FocusHoverColor, FocusForegroundColor, _model);
                //SetColors();
            }

            if (_bkgLayer != null && _grdRoot != null && _grdRoot.ActualWidth != 0) _bkgLayer.InitLayer(_grdRoot.ActualWidth, _grdRoot.ActualHeight);

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


            _udfPB1 = null;
            _udfTB1 = null;
            _udfTBL1 = null;
            _udfCB1 = null;
            _udfChkB1 = null;
            _udfRB1 = null;

            _ccInput.Content = null;

            _model = null;

        }








        //==============================
        // EVENTS
        //==============================

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
                if (((PasswordBox)sender).Password.Length > 0)
                {
                    _udfg1.Visibility = Visibility.Visible;
                }
                else {
                    _udfg1.Visibility = Visibility.Collapsed;
                }

                Value = ((PasswordBox)sender).Password;
                ValueChanged?.Invoke(sender, e);

                Invalidate();
            }
        }

        private void ittext_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(Type == InputType.text)
            {
                if (((TextBox)sender).Text.Length > 0)
                {
                    _udfg1.Visibility = Visibility.Visible;
                }
                else {
                    _udfg1.Visibility = Visibility.Collapsed;
                }

                Value = ((TextBox)sender).Text;
                ValueChanged?.Invoke(sender, e);

                Invalidate();
            }
            
        }

        private void itcombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Type == InputType.combobox)
            {
                if (((ComboBox)sender).SelectedItem != null)
                {
                    _udfg1.Visibility = Visibility.Visible;
                }
                else {
                    _udfg1.Visibility = Visibility.Collapsed;
                }

                ValueChanged?.Invoke(sender, e);
                try {
                    Invalidate();
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









        public static readonly DependencyProperty GlowAmountProperty = DependencyProperty.Register("GlowAmount", typeof(double), typeof(Input), new PropertyMetadata(2));

        public static readonly DependencyProperty GlowColorProperty = DependencyProperty.Register("GlowColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.Black));

        public static readonly DependencyProperty FocusForegroundColorProperty = DependencyProperty.Register("FocusForegroundColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.White, OnPropertyChanged));

        public static readonly DependencyProperty FocusHoverColorProperty = DependencyProperty.Register("FocusHoverColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.DarkGray, OnPropertyChanged));

        public static readonly DependencyProperty FocusColorProperty = DependencyProperty.Register("FocusColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.DarkGray, OnPropertyChanged));

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(Input), new PropertyMetadata(string.Empty, OnPropertyChanged));
        
        public static readonly DependencyProperty LabelTranslateYProperty = DependencyProperty.Register("LabelTranslateY", typeof(double), typeof(Input), new PropertyMetadata(0, OnPropertyChanged));
        
        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register("LabelFontSize", typeof(double), typeof(Input), new PropertyMetadata(9, OnPropertyChanged));

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
