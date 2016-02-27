using System;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
namespace SumoNinjaMonkey.Framework.Controls
{
    public class Animation
    {
        private static int pixelsMove = 70;
        public static void Appear(FrameworkElement el, int millisecondPostpone = 0)
        {
            if (el.GetType().Name == "SplashScreenView")
            {
                el.Opacity =1.0;
                return;
            }


            DispatchedHandler invokedHandler = new DispatchedHandler(() =>
            {
                TranslateTransform translateTransform = new TranslateTransform();
                el.RenderTransform = translateTransform;
                translateTransform.X = (double)Animation.pixelsMove;
                if (translateTransform != null)
                {
                    SplineDoubleKeyFrame splineDoubleKeyFrame = new SplineDoubleKeyFrame();
                    splineDoubleKeyFrame.KeyTime = TimeSpan.FromMilliseconds((double)(10 + millisecondPostpone));
                    splineDoubleKeyFrame.Value = (double)Animation.pixelsMove;
                    SplineDoubleKeyFrame splineDoubleKeyFrame2 = new SplineDoubleKeyFrame();
                    splineDoubleKeyFrame2.KeyTime = TimeSpan.FromMilliseconds((double)(350 + millisecondPostpone));
                    splineDoubleKeyFrame2.Value = 0.0;
                    splineDoubleKeyFrame2.KeySpline = new KeySpline();
                    splineDoubleKeyFrame2.KeySpline.ControlPoint1 = new Point(0.0, 0.0);
                    splineDoubleKeyFrame2.KeySpline.ControlPoint2 = new Point(0.0, 1.0);
                    DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTarget(doubleAnimationUsingKeyFrames, translateTransform);
                    Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames, "(TranslateTransform.X)");
                    doubleAnimationUsingKeyFrames.KeyFrames.Add(splineDoubleKeyFrame);
                    doubleAnimationUsingKeyFrames.KeyFrames.Add(splineDoubleKeyFrame2);
                    SplineDoubleKeyFrame splineDoubleKeyFrame3 = new SplineDoubleKeyFrame();
                    splineDoubleKeyFrame3.KeyTime = TimeSpan.FromMilliseconds((double)millisecondPostpone);
                    splineDoubleKeyFrame3.Value = 0.0;
                    SplineDoubleKeyFrame splineDoubleKeyFrame4 = new SplineDoubleKeyFrame();
                    splineDoubleKeyFrame4.KeyTime = TimeSpan.FromMilliseconds((double)(300 + millisecondPostpone));
                    splineDoubleKeyFrame4.Value = 1.0;
                    DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
                    Storyboard.SetTarget(doubleAnimationUsingKeyFrames2, el);
                    Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames2, "(UIElement.Opacity)");
                    doubleAnimationUsingKeyFrames2.KeyFrames.Add(splineDoubleKeyFrame3);
                    doubleAnimationUsingKeyFrames2.KeyFrames.Add(splineDoubleKeyFrame4);
                    Storyboard storyboard = new Storyboard();
                    storyboard.Children.Add(doubleAnimationUsingKeyFrames);
                    storyboard.Children.Add(doubleAnimationUsingKeyFrames2);
                    storyboard.Begin();
                }

            });


            el.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);
        }
    }
}
