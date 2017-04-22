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

namespace SumoNinjaMonkey.Framework.Controls.Clippy
{
    public sealed partial class Blackboard : UserControl
    {

        public Blackboard()
        {
            this.InitializeComponent();
        }

        public void Init(double width, double height)
        {
            rootWidth = width;
            rootHeight = height;
        }

        //CalloutQuadLocation:
        //  1   |  2
        //  ---------
        //  3   |  4

        private void ShowCallout(double x, double y, double width, double height, int CalloutQuadLocation)
        {
            
            //sbMoveCallout.Stop();
            //sbMoveDot.Stop();

            //var CalloutQuadLocation = 0;

            //if(x < (layoutRoot.ActualWidth/2)){
            //    //1 or 3
            //    if(y < (layoutRoot.ActualHeight/2)) CalloutQuadLocation = 1;
            //    else CalloutQuadLocation = 3;
            //}else{
            //    //2 or 4
            //    if(y < (layoutRoot.ActualHeight/2)) CalloutQuadLocation = 2;
            //    else CalloutQuadLocation = 4;
            //}

            //RECTANGLE 
            DoubleAnimation daTranslateX = sbMoveCallout.Children[0] as DoubleAnimation;
            DoubleAnimation daTranslateY = sbMoveCallout.Children[1] as DoubleAnimation;
            //DoubleAnimation daWidth = sbMoveCallout.Children[2] as DoubleAnimation;
            //DoubleAnimation daHeight = sbMoveCallout.Children[3] as DoubleAnimation;

            //POINTER
            DoubleAnimation daTranslateDotX = sbMoveDot.Children[0] as DoubleAnimation;
            DoubleAnimation daTranslateDotY = sbMoveDot.Children[2] as DoubleAnimation;
            DoubleAnimation daRotateDot = sbMoveDot.Children[1] as DoubleAnimation;


            
            //grdMain.Width = width;
            grdMain.MaxWidth = width;
            grdMain.Height = height;

            switch (CalloutQuadLocation)
            {
                case 1:
                    daTranslateX.To = x - (rootWidth / 2);
                    daTranslateY.To = y - (rootHeight / 2) + (height / 2) + 20;

                    daRotateDot.To = 0d;
                    daTranslateDotY.To = -(height / 2) - 10;
                    break;
                case 2:
                    daTranslateX.To = x - (rootWidth / 2);
                    daTranslateY.To = y - (rootHeight / 2) - (height / 2) - 20;;

                    daRotateDot.To = -180d;
                    daTranslateDotY.To = (height / 2) + 10;
                    break;
                case 3:
                    daTranslateX.To = x - (rootWidth / 2) - (width / 2) - 20;
                    daTranslateY.To = y - (rootHeight / 2);

                    daRotateDot.To = 90d;
                    daTranslateDotX.To = (width / 2) + 10;
                    break;
                case 4:
                    daTranslateX.To = x - (rootWidth / 2) + (width / 2) + 20;
                    daTranslateY.To = y - (rootHeight / 2);

                    daRotateDot.To = -90d;
                    daTranslateDotX.To = - (width / 2) - 10;
                    break;
            }



            sbMoveCallout.Begin();
            sbMoveDot.Begin();
        }

        public enum dotPosition
        {
            above = 1,
            below = 2,
            left = 3,
            right = 4
        }

        List<Step> _steps = new List<Step>();
        int _currentStepIndex = 0;

        public void WriteOnBoard(string text, double x, double y, double width, double height, dotPosition dotPosition)
        {
            _steps.Add(new Step() { 
                Text = text,
                X = x,
                Y = y,
                Width = width,
                Height = height,
                DotPosition = dotPosition
            });

            
        }

        private void NextStep()
        {
            var step = _steps[_currentStepIndex];
            tbData.Text = step.Text;
            ShowCallout(step.X, step.Y, step.Width, step.Height, (int)step.DotPosition);

            _currentStepIndex++;
            if (_currentStepIndex > _steps.Count() - 1) _currentStepIndex = _steps.Count() - 1;
        }

        private void PreviousStep()
        {
            var step = _steps[_currentStepIndex];
            tbData.Text = step.Text;
            ShowCallout(step.X, step.Y, step.Width, step.Height, (int)step.DotPosition);

            _currentStepIndex--;
            if (_currentStepIndex < 0) _currentStepIndex = 0;
        }

        public void StartHelp()
        {
            NextStep();
        }

        //private void layoutRoot_PointerMoved(object sender, PointerRoutedEventArgs e)
        //{
        //    var pt = e.GetCurrentPoint(null);

        //    ShowCallout(pt.Position.X, pt.Position.Y, 300d, 180d, 4);
        //}

        private class Step{
            public string Text { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public dotPosition DotPosition{ get; set; }

        }

        private void butPreviousStep_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PreviousStep();
        }

        private void butNextStep_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NextStep();
        }
        double rootWidth;
        double rootHeight;


    }
}
