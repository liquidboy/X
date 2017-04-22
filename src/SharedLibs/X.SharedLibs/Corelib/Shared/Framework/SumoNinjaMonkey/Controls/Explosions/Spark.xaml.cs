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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace SumoNinjaMonkey.Framework.Controls.Explosions
{
    public sealed partial class Spark : UserControl, IExplosion
    {
        public Spark()
        {
            this.InitializeComponent();
        }

        public void Explode(double durationInSeconds, double x, double y, int randomColorCode, Action callbackOnComplete){


            DoubleAnimation daukfSX = (DoubleAnimation)sbAnimate.Children[2];
            DoubleAnimation daukfSY = (DoubleAnimation)sbAnimate.Children[3];
            DoubleAnimationUsingKeyFrames daukfX = (DoubleAnimationUsingKeyFrames)sbAnimate.Children[1];
            DoubleAnimationUsingKeyFrames daukfY = (DoubleAnimationUsingKeyFrames)sbAnimate.Children[0];

            //scale
            daukfSX.To = 0.5d;
            daukfSY.To = 0.5d;

            //time
            //daukfX.KeyFrames[0].KeyTime = TimeSpan.FromSeconds(durationInSeconds) ;
            //daukfX.KeyFrames[1].KeyTime = TimeSpan.FromSeconds(durationInSeconds);
            //daukfX.KeyFrames[2].KeyTime = TimeSpan.FromSeconds(durationInSeconds);
            //daukfY.KeyFrames[0].KeyTime = TimeSpan.FromSeconds(durationInSeconds) ;
            //daukfY.KeyFrames[1].KeyTime = TimeSpan.FromSeconds(durationInSeconds);
            //daukfY.KeyFrames[2].KeyTime = TimeSpan.FromSeconds(durationInSeconds);

            //set translation

            daukfX.KeyFrames[0].Value = (x * 0.5d) * 0.5d;
            daukfX.KeyFrames[1].Value = x * 0.5d;
            daukfX.KeyFrames[2].Value = x;
            //daukfY.KeyFrames[0].Value = TimeSpan.FromSeconds(durationInSeconds);
            //daukfY.KeyFrames[1].Value = TimeSpan.FromSeconds(durationInSeconds);
            //daukfY.KeyFrames[2].Value = TimeSpan.FromSeconds(durationInSeconds);


            //set color
            if (randomColorCode == 1) _0.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            else if (randomColorCode == 2) _0.Fill = new SolidColorBrush(Windows.UI.Colors.Yellow);
            else if (randomColorCode == 3) _0.Fill = new SolidColorBrush(Windows.UI.Colors.Pink);
            else if (randomColorCode == 4) _0.Fill = new SolidColorBrush(Windows.UI.Colors.Green);
            else if (randomColorCode == 5) _0.Fill = new SolidColorBrush(Windows.UI.Colors.Purple);
            else if (randomColorCode == 6) _0.Fill = new SolidColorBrush(Windows.UI.Colors.Orange);
            else if (randomColorCode == 7) _0.Fill = new SolidColorBrush(Windows.UI.Colors.Blue);


            //execute explosion of sprite
            sbAnimate.Completed += (o, a) => { callbackOnComplete(); callbackOnComplete = null; };
            sbAnimate.Begin();
        }

        public void SetMargin(Thickness margin)
        {
            Margin = margin;
        }

        public void SetHorizontalAlignment(HorizontalAlignment align)
        {
            HorizontalAlignment = align;
        }

        public void SetVerticalAlignment(VerticalAlignment align)
        {
            VerticalAlignment = align;
        }
    }
}
