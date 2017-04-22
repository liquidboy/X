using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class GlowingButton : UserControl
    {
        public string ClickCode { get; set; }
        public string ClickIdentifier { get; set; }
        public string ClickAggregateId { get; set; }

        public GlowingButton()
        {
            this.ClickIdentifier = "GBC";
            this.InitializeComponent();

        }

        private void layoutRoot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            sbPressed.Stop();
            sbPressed.Begin();

            if(!string.IsNullOrEmpty(ClickCode))
                Messenger.Default.Send(new GeneralSystemWideMessage(ClickCode) { Identifier = ClickIdentifier, Action = ClickCode, AggregateId = ClickAggregateId });
        }


        public void LoadMetroIcon(string key, Brush iconColor = null, double rotation = 0, double scale = 1 )
        {
            if (iconColor == null) iconColor = new SolidColorBrush(Windows.UI.Colors.White);

            string temp = (string)Application.Current.Resources[key];
            string pthString = @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                Data=""" + temp + @""" />";
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
            pth.Stretch = Stretch.Uniform;
            pth.Fill = iconColor;
            pth.Width = 25;
            pth.Height = 25;


            grdIcon.Children.Add(pth);

            ((CompositeTransform)grdIcon.RenderTransform).Rotation = rotation;
            ((CompositeTransform)grdIcon.RenderTransform).ScaleX = scale;
            ((CompositeTransform)grdIcon.RenderTransform).ScaleY = scale;
        }

        public void UpdateBackgroundColor(Brush bkgColor)
        {
            recBackground.Fill = bkgColor;
        }
    }
}
