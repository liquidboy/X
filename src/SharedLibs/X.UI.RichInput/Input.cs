using System;
using System.Collections.Generic;
using System.Linq;
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
    public enum InputType
    {
        button,
        checkbox,
        combobox,
        color,
        date,
        datetime,
        datetimeLocal,
        email,
        month,
        number,
        password,
        radio,
        range,
        search,
        submit,
        tel,
        text,
        time,
        url,
        week

    }


    public sealed class Input : Control
    {
        Grid _grdRoot;
        ContentControl _ccInput;
        Style _GeneralTextBoxStyle;
        Style _GeneralPasswordBoxStyle;
        Style _GeneralCheckBoxStyle;
        Style _GeneralRadioButtonStyle;
        Style _GeneralComboBoxStyle;
        TextBox _udfTB1;
        TextBlock _udfTBL1;
        Grid _udfg1;

        InputModel _model;

        public event Windows.UI.Xaml.RoutedEventHandler ValueChanged;


        public string Value { get; set; }

        public Input()
        {
            this.DefaultStyleKey = typeof(Input);

            Loaded += Input_Loaded;
            Unloaded += Input_Unloaded;
        }

        private void Input_Unloaded(object sender, RoutedEventArgs e)
        {
            UnloadControl(Type);
        }

        private void Input_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void OnApplyTemplate()
        {
            if (_model == null)
            {
                _model = new InputModel();
            }

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
                
                BuildControl(Type, Label, PlaceholderText, LabelFontSize, LabelTranslateY, GroupName, Items, _ccInput);
                SetColors(FocusColor, FocusHoverColor, FocusForegroundColor, _model);
            }
            
            base.OnApplyTemplate();
        }
        
        private void BuildControl(InputType type, string label, string placeholderText, double labelFontSize, double labelTranslateY, string groupName, ItemCollection items, ContentControl ccInput) {

            FrameworkElement fe = null;

            if (type == InputType.text)
            {
                var tb = new TextBox();
                tb.PlaceholderText = placeholderText;
                tb.Style = _GeneralTextBoxStyle;
                tb.KeyUp += ittext_KeyUp;

                _udfg1 = new Grid() { Padding = new Thickness(2, 2, 2, 2), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment= VerticalAlignment.Top};
                _udfTBL1 = new TextBlock();
                _udfTBL1.Text = label;
                _udfTBL1.FontSize = labelFontSize;
                _udfg1.Margin = new Thickness(0, labelTranslateY, 0, 0);
                _udfg1.Visibility = Visibility.Collapsed;
                _udfg1.Children.Add(_udfTBL1);

                var gd = new Grid();
                gd.Children.Add(_udfg1);
                gd.Children.Add(tb);

                fe = gd;
            }
            else if (type == InputType.password)
            {
                

                var tb = new PasswordBox();
                tb.PlaceholderText = placeholderText;
                tb.Style = _GeneralPasswordBoxStyle;
                tb.PasswordChanged += itpassword_PasswordChanged;

                _udfg1 = new Grid() { Padding = new Thickness(2, 2, 2, 2), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top };
                _udfTBL1 = new TextBlock();
                _udfTBL1.Text = label;
                _udfTBL1.FontSize = labelFontSize;
                _udfg1.Margin = new Thickness(0, labelTranslateY, 0, 0);
                _udfg1.Visibility = Visibility.Collapsed;
                _udfg1.Children.Add(_udfTBL1);

                var gd = new Grid();
                gd.Children.Add(_udfg1);
                gd.Children.Add(tb);

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

                var cb = new CheckBox();
                cb.Checked += itcheckbox_Checked;
                cb.Content = lb;
                cb.Style = _GeneralCheckBoxStyle;
                sp.Children.Add(cb);

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

                var rb = new RadioButton();
                rb.GroupName = groupName;
                rb.Checked += itradio_Checked;
                rb.Content = lb;
                rb.Style = _GeneralRadioButtonStyle;
                sp.Children.Add(rb);

                fe = sp;
            }
            else if (type == InputType.combobox)
            {
                var sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;

                var lb = new TextBlock();
                lb.Text = label;
                lb.FontSize = LabelFontSize;
                lb.Margin = new Thickness(0, LabelTranslateY, 0, 0);

                var cb = new ComboBox();
                cb.Style = _GeneralComboBoxStyle;
                if (Items != null && Items.Count > 0) {
                    foreach(var item in Items)
                    {
                        cb.Items.Add(item);
                    }
                }
                sp.Children.Add(cb);

                fe = sp;
            }


            fe.HorizontalAlignment = HorizontalAlignment.Stretch;
            fe.VerticalAlignment = VerticalAlignment.Stretch;
            ccInput.Content = fe;

        }


        private void SetColors(Color focusColor, Color focusHoverColor, Color focusForegroundColor, InputModel model) {

            model.FocusColor = focusColor;
            model.FocusHoverColor = focusHoverColor;
            model.FocusForegroundColor = focusForegroundColor;

            if (_udfg1 != null) {
                _udfg1.Background = new SolidColorBrush(focusColor);
                if (_udfTBL1 != null) _udfTBL1.Foreground = new SolidColorBrush(focusForegroundColor);
            }
        }

        private void UpdateControl(InputType type, string label, string placeholderText, double labelFontSize, double labelTranslateY, string groupName, ContentControl ccInput)
        {
            throw new NotImplementedException();
        }

        private void UnloadControl(InputType type)
        {

            if (type == InputType.text)
            {
                var gd = (Grid)_ccInput.Content;
                var tb = (TextBox)gd.Children[1];
                tb.KeyUp -= ittext_KeyUp;
            }
            else if (type == InputType.password)
            {
                var gd = (Grid)_ccInput.Content;
                var tb = (PasswordBox)gd.Children[1];
                tb.PasswordChanged -= itpassword_PasswordChanged;
            }
            else if (type == InputType.checkbox)
            {
                var sp = (StackPanel)_ccInput.Content;
                var cb = (CheckBox)sp.Children[0];
                cb.Checked -= itcheckbox_Checked;
            }
            else if (type == InputType.radio)
            {
                var sp = (StackPanel)_ccInput.Content;
                var cb = (RadioButton)sp.Children[0];
                cb.Checked -= itradio_Checked;
            }
            else if (type == InputType.combobox)
            {
                var sp = (StackPanel)_ccInput.Content;
                var cb = (ComboBox)sp.Children[0];
                //cb.Checked -= itcheckbox_Checked;
                if (cb.Items != null && cb.Items.Count > 0) cb.Items.Clear();
            }

            if (_udfg1 != null)
            {
                _udfg1 = null;
                _udfg1.Children.Clear();
            }
            

            _udfTB1 = null;
            _udfTBL1 = null;
            

            _ccInput.Content = null;

            _model = null;

        }








        //==============================
        // EVENTS
        //==============================

        private void itradio_Checked(object sender, RoutedEventArgs e)
        {
            if (Type == InputType.radio)
            {
                Value = ((RadioButton)sender).IsChecked.ToString();
                ValueChanged?.Invoke(sender, e);
            }
        }

        private void itcheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (Type == InputType.checkbox)
            {
                Value = ((CheckBox)sender).IsChecked.ToString();
                ValueChanged?.Invoke(sender, e);
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
        
        public ItemCollection Items
        {
            get { return (ItemCollection)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }












        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ItemCollection), typeof(Input), new PropertyMetadata(null));

        public static readonly DependencyProperty FocusForegroundColorProperty = DependencyProperty.Register("FocusForegroundColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.White));

        public static readonly DependencyProperty FocusHoverColorProperty = DependencyProperty.Register("FocusHoverColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.DarkGray));

        public static readonly DependencyProperty FocusColorProperty = DependencyProperty.Register("FocusColor", typeof(Color), typeof(Input), new PropertyMetadata(Colors.DarkGray));

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
                instance.UpdateControl(instance.Type, instance.Label, instance.PlaceholderText, instance.LabelFontSize, instance.LabelTranslateY, instance.GroupName, instance._ccInput);
            }
        }


     

    }

 



    class InputModel : DependencyObject {

        public object Data { get; set; }

        public Color FocusColor { get; set; }

        public Color FocusHoverColor { get; set; }

        public Color FocusForegroundColor { get; set; }
        

    }
}
