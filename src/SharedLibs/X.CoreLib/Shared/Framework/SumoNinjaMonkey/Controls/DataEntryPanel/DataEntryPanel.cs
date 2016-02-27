using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed class DataEntryPanel : Control
    {
        private static Grid mainGrid;
        private static StackPanel spChildren;

        private static List<DPMandatory> _mandatoryControls;
        private static List<UIElement> _controls;

        

        public DataEntryPanel()
        {
            this.DefaultStyleKey = typeof(DataEntryPanel);
            _mandatoryControls = new List<DPMandatory>();
            _controls = new List<UIElement>();
        }

        protected override void OnApplyTemplate()
        {

            

            base.OnApplyTemplate();

            
        }

        public void Show()
        {
            InitControls();
        }

        public void Hide()
        {
            UnloadAll();
        }


        private void InitControls()
        {
            if (mainGrid == null)
            {

                mainGrid = (Grid)GetTemplateChild("mainGrid");
                spChildren = (StackPanel)GetTemplateChild("spChildren");

                Render();

                DispatcherTimer dtOnce = new DispatcherTimer();
                dtOnce.Interval = TimeSpan.FromSeconds(1.5);
                dtOnce.Tick += (o, e) => { ShowMandatory(); dtOnce.Stop(); };
                dtOnce.Start();
            }
            
        }


        private void UnloadAll()
        {
            foreach (var m in _mandatoryControls)
            {
                m.Hide();
            }
            
        }

        public void LoadList(int index, Type ListSource)
        {
            if (ListSource.GetTypeInfo().IsEnum)
            {
                string[] names = Enum.GetNames(ListSource);

                var control = _controls[index];
                if (control is ComboBox)
                {
                    ((ComboBox)control).ItemsSource = names;
                }
                else if (control is ListBox)
                {
                    ((ListBox)control).ItemsSource = names;
                }
            }
            else if (ListSource.GetTypeInfo().IsClass || ListSource.GetTypeInfo().IsAnsiClass)
            {
                var props = ListSource.GetTypeInfo().DeclaredProperties;
            
                var control = _controls[index];
                if (control is ComboBox)
                {
                    ((ComboBox)control).ItemsSource = props.Select(x => x.Name).AsEnumerable();
                    //((ComboBox)control)
                }
                else if (control is ListBox)
                {
                    ((ListBox)control).ItemsSource = props.Select(x => x.Name).AsEnumerable();
                }
            }
            
        }




        public string MetaData
        {
            get { return (string)GetValue(MetaDataProperty); }
            set { SetValue(MetaDataProperty, value); }
        }

        public static readonly DependencyProperty MetaDataProperty =
            DependencyProperty.Register("MetaData", typeof(string), typeof(DataEntryPanel), new PropertyMetadata(null, MetaDataChanged));

        private static void MetaDataChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            
        }



        private void Render()
        {
            if (mainGrid == null) return;
            string[] lines = this.MetaData.Split("^".ToCharArray());

            StackPanel sp = spChildren; // new StackPanel();            
            sp.Orientation = Orientation.Vertical;
            //sp.Transitions = new TransitionCollection();
            //sp.Transitions.Add(new EntranceThemeTransition());
            //mainGrid.Children.Add(sp);

            foreach (var line in lines)
            {
                string[] parts = line.Split("|".ToCharArray());

                UIElement uie = null;
                TextBlock label = null;
                Border labelBorder = null;
                DPMandatory mandatory = null;
                switch (parts[0].Trim())
                {
                    case "slider":
                        uie = new Slider();
                        Slider te1 = (Slider)uie;
                        #region Slider
                        te1.Minimum = double.Parse(parts[1]);
                        te1.Maximum = double.Parse(parts[2]);
                        te1.Value = double.Parse(parts[3]);
                        te1.Width = double.Parse(parts[4]);
                        te1.Height = double.Parse(parts[5]);
                        te1.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        te1.ValueChanged += (o,e) => {
                            Messenger.Default.Send(new DataEntryResponseMessage(e.NewValue.ToString()) { Identifier = parts[6] });
                        };
                        //te1.Foreground = new SolidColorBrush( Colors.Red);
                        //te1.Background = new SolidColorBrush(Colors.Gray);

                        label = new TextBlock();
                        label.Text = parts[6];
                        label.HorizontalAlignment = StringToHorizontalAlignment(parts[8]);
                        label.VerticalAlignment = StringToVerticalAlignment(parts[12]);
                        label.Foreground = new SolidColorBrush(ColorConverter(parts[10]));
                        label.Margin = new Thickness(10);
                        label.FontSize = double.Parse(parts[11]);

                        labelBorder = new Border();
                        labelBorder.Width = double.Parse(parts[7]);
                        labelBorder.Background = new SolidColorBrush(ColorConverter(parts[9]));
                        labelBorder.Child = label;
                        labelBorder.Margin = new Thickness(0, 0, 10, 0);

                        if (parts[13] == "1") mandatory = new DPMandatory();
                        #endregion
                        break;
                    case "textbox": 
                        uie = new TextBox();
                        TextBox te2 = (TextBox)uie;
                        #region TextBox
                        te2.Width = double.Parse(parts[1]);
                        te2.Height = double.Parse(parts[2]);
                        te2.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        te2.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
                        te2.BorderThickness = new Thickness(1);
                        te2.TextWrapping = TextWrapping.Wrap;

                        label = new TextBlock();
                        label.Text = parts[4];
                        label.HorizontalAlignment = StringToHorizontalAlignment(parts[6]);
                        label.VerticalAlignment = StringToVerticalAlignment(parts[10]);
                        label.Foreground = new SolidColorBrush(ColorConverter(parts[8]));
                        label.Margin = new Thickness(10);
                        label.FontSize = double.Parse(parts[9]);

                        labelBorder = new Border();
                        labelBorder.Width = double.Parse(parts[5]);
                        labelBorder.Background = new SolidColorBrush(ColorConverter(parts[7]));
                        labelBorder.Child = label;
                        labelBorder.Margin = new Thickness(0, 0, 10, 0);

                        if (parts[11] == "1") mandatory = new DPMandatory();
                        #endregion
                        break;
                    case "listbox": 
                        uie = new ListBox();
                        ListBox te3 = (ListBox)uie;
                        #region ListBox
                        te3.Width = double.Parse(parts[1]);
                        te3.Height = double.Parse(parts[2]);
                        te3.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        te3.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
                        te3.BorderThickness = new Thickness(1);

                        label = new TextBlock();
                        label.Text = parts[3];
                        label.HorizontalAlignment = StringToHorizontalAlignment(parts[5]);
                        label.VerticalAlignment = StringToVerticalAlignment(parts[9]);
                        label.Foreground = new SolidColorBrush(ColorConverter(parts[7]));
                        label.Margin = new Thickness(10);
                        label.FontSize = double.Parse(parts[8]);

                        labelBorder = new Border();
                        labelBorder.Width = double.Parse(parts[4]);
                        labelBorder.Background = new SolidColorBrush(ColorConverter(parts[6]));
                        labelBorder.Child = label;
                        labelBorder.Margin = new Thickness(0, 0, 10, 0);

                        if (parts[10] == "1") mandatory = new DPMandatory();
                        #endregion
                        break;
                    case "combobox": 
                        uie = new ComboBox();
                        ComboBox te4 = (ComboBox)uie;
                        #region ComboBox
                        te4.Width = double.Parse(parts[1]);
                        te4.Height = double.Parse(parts[2]);
                        te4.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        te4.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
                        te4.BorderThickness = new Thickness(1);

                        label = new TextBlock();
                        label.Text = parts[3];
                        label.HorizontalAlignment = StringToHorizontalAlignment(parts[5]);
                        label.VerticalAlignment = StringToVerticalAlignment(parts[9]);
                        label.Foreground = new SolidColorBrush(ColorConverter(parts[7]));
                        label.Margin = new Thickness(10);
                        label.FontSize = double.Parse(parts[8]);


                        labelBorder = new Border();
                        labelBorder.Width = double.Parse(parts[4]);
                        labelBorder.Background = new SolidColorBrush(ColorConverter(parts[6]));
                        labelBorder.Child = label;
                        labelBorder.Margin = new Thickness(0, 0, 10, 0);

                        if (parts[10] == "1") mandatory = new DPMandatory();
                        #endregion
                        break;

                    case "checkbox":
                        uie = new CheckBox();
                        CheckBox te5 = (CheckBox)uie;
                        #region checkbox
                        te5.Width = double.Parse(parts[1]);
                        te5.Height = double.Parse(parts[2]);
                        te5.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        te5.IsChecked = bool.Parse(parts[3]);
                        te5.Checked += (o, e) =>
                        {
                            Messenger.Default.Send(new DataEntryResponseMessage("True") { Identifier = parts[4] });
                        };
                        te5.Unchecked += (o, e) =>
                        {
                            Messenger.Default.Send(new DataEntryResponseMessage("False") { Identifier = parts[4] });
                        };
                        //te1.Foreground = new SolidColorBrush( Colors.Red);
                        //te1.Background = new SolidColorBrush(Colors.Gray);

                        label = new TextBlock();
                        label.Text = parts[4];
                        label.HorizontalAlignment = StringToHorizontalAlignment(parts[6]);
                        label.VerticalAlignment = StringToVerticalAlignment(parts[11]);
                        label.Foreground = new SolidColorBrush(ColorConverter(parts[8]));
                        label.Margin = new Thickness(10);
                        label.FontSize = double.Parse(parts[9]);

                        labelBorder = new Border();
                        labelBorder.Width = double.Parse(parts[5]);
                        labelBorder.Background = new SolidColorBrush(ColorConverter(parts[7]));
                        labelBorder.Child = label;
                        labelBorder.Margin = new Thickness(0, 0, 10, 0);

                        if (parts[11] == "1") mandatory = new DPMandatory();
                        #endregion
                        break;
                    case "loadlist":
                        int indexOfControl = int.Parse(parts[1]);
                        UIElement listControl = _controls[indexOfControl];
                        #region LoadList
                        if (parts[2] == "reflection")
                        {
                            
                            //SharpDX.Direct3D11.Comparison c2 = SharpDX.Direct3D11.Comparison.Always;
                            
                            var t = Type.GetType(parts[3]);
                            //var lang = Activator.CreateInstance(t, new[] { "en-US" });
                            //var name = t.GetTypeInfo().GetDeclaredProperty("DisplayName").GetValue(lang); 
                            LoadList(indexOfControl, t);
                        }
                        #endregion
                        break;
                    default: //unhandelled control
                        break;

                }


                if (uie != null)
                {
                    if (labelBorder != null) 
                    {
                        //has a label
                        StackPanel spWithLabel = new StackPanel();
                        spWithLabel.Orientation = Orientation.Horizontal;
                        spWithLabel.Children.Add(labelBorder);
                        spWithLabel.Children.Add(uie);
                        spWithLabel.Margin = new Thickness(0, 10, 0, 0);

                        if (mandatory != null) spWithLabel.Children.Add(mandatory);

                        sp.Children.Add(spWithLabel);
                    }
                    else
                    {
                        //no label
                        
                        StackPanel spWithoutLabel = new StackPanel();
                        spWithoutLabel.Orientation = Orientation.Horizontal;
                        spWithoutLabel.Children.Add(uie);
                        spWithoutLabel.Margin = new Thickness(0, 10, 0, 0);
                        if (mandatory != null) spWithoutLabel.Children.Add(mandatory);
                        sp.Children.Add(spWithoutLabel);
                    }

                    _controls.Add(uie);
                }

                if (mandatory != null)
                {
                    mandatory.Margin = new Thickness(10, 0, 0, 0);

                    _mandatoryControls.Add(mandatory);
                }
                

            }
            //sp.UpdateLayout();
            
        }

        private Color ColorConverter(string colorHashCode)
        {
            int argb = Int32.Parse(colorHashCode.Replace("#", ""), NumberStyles.HexNumber);

            return Color.FromArgb((byte)((argb & -16777216) >> 0x18),
                                         (byte)((argb & 0xff0000) >> 0x10),
                                         (byte)((argb & 0xff00) >> 8),
                                         (byte)(argb & 0xff));

        }

        private VerticalAlignment StringToVerticalAlignment(string value)
        {
            switch (value)
            {
                case "top": return Windows.UI.Xaml.VerticalAlignment.Top;
                case "bottom": return Windows.UI.Xaml.VerticalAlignment.Bottom;
                case "center": return Windows.UI.Xaml.VerticalAlignment.Center;
                case "stretch": return Windows.UI.Xaml.VerticalAlignment.Stretch;
            }

            return Windows.UI.Xaml.VerticalAlignment.Top;
        }

        private HorizontalAlignment StringToHorizontalAlignment(string value)
        {
            switch (value)
            {
                case "left": return Windows.UI.Xaml.HorizontalAlignment.Left;
                case "right": return Windows.UI.Xaml.HorizontalAlignment.Right;
                case "center": return Windows.UI.Xaml.HorizontalAlignment.Center;
                case "stretch": return Windows.UI.Xaml.HorizontalAlignment.Stretch;
            }

            return Windows.UI.Xaml.HorizontalAlignment.Left;
        }

        public void SetValue(int index, object value)
        {
            if (value == null) return;
            if (mainGrid == null || mainGrid.Children.Count == 0) return ;

            StackPanel sp = (StackPanel)mainGrid.Children[3];

            if (index >= sp.Children.Count) return;

            UIElement uie = sp.Children[index];
            uie = ((StackPanel)uie).Children[1];

            if (uie is Slider)
            {
                ((Slider)uie).Value = (double)value;
            }
            else if (uie is TextBox)
            {
                ((TextBox)uie).Text = (string)value;
            }
            else if (uie is ListBox)
            {
                ((ListBox)uie).SelectedValue = value;
            }
            else if (uie is ComboBox)
            {
                ((ComboBox)uie).SelectedValue = value;
            }
        }

        public object GetValue(int index)
        {
            if (mainGrid == null || mainGrid.Children.Count == 0) return null;
            
            StackPanel sp = (StackPanel)mainGrid.Children[3];

            if (index >= sp.Children.Count) return null;

            UIElement uie = sp.Children[index];
            uie = ((StackPanel)uie).Children[1];

            if (uie is Slider)
            {
                return ((Slider)uie).Value;
            }
            else if (uie is TextBox)
            {

                return ((TextBox)uie).Text;
            }
            else if (uie is ListBox)
            {
                return ((ListBox)uie).SelectedValue;
            }
            else if (uie is ComboBox)
            {
                return ((ComboBox)uie).SelectedValue;
            }

            return null;
        }

        private void ShowMandatory()
        {
            foreach (var m in _mandatoryControls)
            {
                m.Show();                
            }
        }



        public string HeaderLabel
        {
            get { return (string)GetValue(HeaderLabelProperty); }
            set { SetValue(HeaderLabelProperty, value); }
        }

        public static readonly DependencyProperty HeaderLabelProperty =
            DependencyProperty.Register("HeaderLabel", typeof(string), typeof(DataEntryPanel), new PropertyMetadata(string.Empty));



        public Brush HeaderBackgroundColor
        {
            get { return (Brush)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty HeaderBackgroundColorProperty =
            DependencyProperty.Register("HeaderBackgroundColor", typeof(Brush), typeof(DataEntryPanel), new PropertyMetadata(Colors.Black));

        
        public Brush HeaderForegroundColor
        {
            get { return (Brush)GetValue(HeaderForegroundColorProperty); }
            set { SetValue(HeaderForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty HeaderForegroundColorProperty =
            DependencyProperty.Register("HeaderForegroundColor", typeof(Brush), typeof(DataEntryPanel), new PropertyMetadata(Colors.Black));



        public Brush GeneralBackgroundColor
        {
            get { return (Brush)GetValue(GeneralBackgroundColorProperty); }
            set { SetValue(GeneralBackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty GeneralBackgroundColorProperty =
            DependencyProperty.Register("GeneralBackgroundColor", typeof(Brush), typeof(DataEntryPanel), new PropertyMetadata(Colors.WhiteSmoke));

        
    }
}
