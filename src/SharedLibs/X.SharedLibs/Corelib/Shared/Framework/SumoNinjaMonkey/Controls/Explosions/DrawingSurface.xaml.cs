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

namespace SumoNinjaMonkey.Framework.Controls.Explosions
{
    public sealed partial class DrawingSurface : UserControl
    {
        Random rnd = new Random();

        public DrawingSurface()
        {
            this.InitializeComponent();
        }



        public void DoExplosion(double x, double y)
        {
            int colorCode = rnd.Next(0, 7); //0 <== flickr pink
            for (int i = 0; i < 100; i++)
            {
                int explosionType = 4; //rnd.Next(0, 10);
                IExplosion explosion;
                if (explosionType == 4 )
                {
                    explosion = new Explosions.Explosion();
                }
                else if (explosionType == 3)
                {
                    explosion = new Explosions.Spark();
                }
                else
                {
                    explosion = new Explosions.ExplosionRectangle();
                }
                
                explosion.SetMargin( new Thickness(x, y, 0, 0));
                explosion.SetHorizontalAlignment(Windows.UI.Xaml.HorizontalAlignment.Left);
                explosion.SetVerticalAlignment(Windows.UI.Xaml.VerticalAlignment.Top);
                layoutRoot.Children.Add(explosion as UserControl);
                explosion.Explode((rnd.NextDouble() * 3), rnd.Next(-150, 150), rnd.Next(-150, 150), colorCode, () =>
                {
                    layoutRoot.Children.Remove(explosion as UserControl);
                });
            }

        }
    }
}
