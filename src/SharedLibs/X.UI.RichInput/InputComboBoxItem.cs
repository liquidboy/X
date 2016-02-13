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


namespace X.UI.RichInput
{
    public sealed class InputComboBoxItem : ComboBoxItem
    {
        InputModel _model;
        Grid _grdRoot;
        Style _GeneralComboBoxItemStyle;

        public InputComboBoxItem()
        {
            this.DefaultStyleKey = typeof(InputComboBoxItem);

            this.Loaded += InputComboBoxItem_Loaded;
            this.Unloaded += InputComboBoxItem_Unloaded;
        }

        private void InputComboBoxItem_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void InputComboBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override void OnApplyTemplate()
        {
            if (_model == null)
            {
                _model = new InputModel();
            }

            if (_grdRoot == null)
            {
                _grdRoot = GetTemplateChild("grdRoot") as Grid;
                _grdRoot.DataContext = _model;
            }

            if (_GeneralComboBoxItemStyle == null)
            {
                _GeneralComboBoxItemStyle = (Style)_grdRoot.Resources["GeneralComboBoxItemStyle"];
            }

            this.Style = _GeneralComboBoxItemStyle;


            base.OnApplyTemplate();
        }
    }
}
