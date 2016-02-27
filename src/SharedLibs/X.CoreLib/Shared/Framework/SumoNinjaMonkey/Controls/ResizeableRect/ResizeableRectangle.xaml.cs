using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class ResizeableRectangle : UserControl
    {

        public enum eResizeableRectangleState
        {
            None,
            MoveAllDirections,
            ResizeWidthHeightEqually,
            ResizeWidthHeightIndependently
        }

        public eResizeableRectangleState State { get; set; }
        public bool CanResizeWidthHeightIndependently { get; set; }

        public ResizeableRectangle()
        {
            this.InitializeComponent();

            elResizeDot.PointerPressed += elResizeDot_PointerPressed;
            elMoveDot.PointerPressed += elMoveDot_PointerPressed;
        }

        void elMoveDot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.State = eResizeableRectangleState.MoveAllDirections;
        }

        private void elResizeDot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (CanResizeWidthHeightIndependently) this.State = eResizeableRectangleState.ResizeWidthHeightIndependently;
            else this.State = eResizeableRectangleState.ResizeWidthHeightEqually;
        }

        


    }
}
