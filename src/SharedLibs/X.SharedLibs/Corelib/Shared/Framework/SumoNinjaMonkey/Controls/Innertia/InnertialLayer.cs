


using System;
using SumoNinjaMonkey.Framework.Controls.Innertia;
//using JF.Studio.Views;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed class InnertialLayer : ContentControl
    {
        Canvas rootParent; //the roots parent needs to be a canvas
        Canvas root;
        Storyboard layerStoryboard;


        LayerState layerState;
        DoubleAnimation layerAnimationX;
        DoubleAnimation layerAnimationY;


        public double SensitivityX { get; set; }
        public double SensitivityY { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public InnertialLayer()
        {
            this.DefaultStyleKey = typeof(InnertialLayer);


        }



        protected override void OnDisconnectVisualChildren()
        {
            base.OnDisconnectVisualChildren();
        }
        protected override void OnApplyTemplate()
        {
            if (!(this.Parent is Canvas)) throw new Exception("parent of an InnertialLayer needs to be a canvas");

            base.OnApplyTemplate();

            InitializeControl();
        }

        private void InitializeControl()
        {
            try
            {

                if (root == null)
                {
                    root = (Canvas)GetTemplateChild("root");
                    layerStoryboard = (Storyboard)root.Resources["layerStoryboard"];

                    layerAnimationX = (DoubleAnimation)layerStoryboard.Children[0];
                    layerAnimationY = (DoubleAnimation)layerStoryboard.Children[1];

                    layerState = new LayerState(SensitivityX, SensitivityY);

                    Conductor.Beat += Conductor_Beat;

                    rootParent = (Canvas)this.Parent;
                    //rootParent.PointerPressed += rootParent_PointerPressed;
                    rootParent.ManipulationDelta += rootParent_ManipulationDelta;
                    rootParent.ManipulationMode = this.ManipulationMode; // &ManipulationModes.TranslateY;

                }


            }
            catch { }
        }

        public void UnInitializeControl()
        {
            if (root != null && rootParent != null)
            {
                Conductor.Beat -= Conductor_Beat;
                //rootParent.PointerPressed -= rootParent_PointerPressed;
                rootParent.ManipulationDelta -= rootParent_ManipulationDelta;

                layerState = null;
                layerStoryboard.Stop();
            }
        }


        private void Conductor_Beat(object sender, System.EventArgs e)
        {
            layerState.FrameUpdate();

            if (layerState.Velocity.X == 0 && layerState.Velocity.Y == 0) Conductor.Stop();

            AnimateLayer(layerState.Velocity.X * 50, layerState.Velocity.Y * 50);
            //AnimateLayer(layerState.Position.X, layerState.Position.Y);
            //ForcePositionLayer(layerState.Velocity.X*100, layerState.Velocity.Y*100);
        }



        private void AnimateLayer(double newTranslateX, double newTranslateY)
        {

            X += newTranslateX + this.Margin.Left;
            Y += newTranslateY + this.Margin.Top;

            layerAnimationX.To = X; // newTranslateX;
            layerAnimationY.To = Y; // newTranslateY;

            layerStoryboard.Begin();
        }

        public void ForcePositionLayer(double x, double y)
        {
            X += x + this.Margin.Left;
            Y += y + this.Margin.Top;

            layerAnimationX.To = X;
            layerAnimationY.To = Y;

            layerStoryboard.Begin();
        }

        public void ForceExactPositionLayer(double x, double y)
        {
            X = x + this.Margin.Left;
            Y = y + this.Margin.Top;

            layerAnimationX.To = X;
            layerAnimationY.To = Y;

            layerStoryboard.Begin();
        }

        private void rootParent_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            
            layerState.Velocity.X = layerState.Sensitivity.X == 0 ? 0 : e.Velocities.Linear.X;
            layerState.Velocity.Y = layerState.Sensitivity.Y == 0 ? 0 : e.Velocities.Linear.Y;

            layerState.Position.X = e.Position.X;
            layerState.Position.Y = e.Position.Y;
        }



        //private void rootParent_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    if(!Conductor.IsRunning)Conductor.Start();
        //}




    }



    public class LayerState
    {
        public Point Velocity; //this is initially passed in and updated by the algorithm during each tick 
        public Point Position; //this is updated by the algorithm during each tick
        public Point Sensitivity; //how sensitive should the easing be for the re-positioning



        public LayerState(double sensitivityX, double sensitivityY)
        {
            Sensitivity.X = sensitivityX;
            Sensitivity.Y = sensitivityY;

        }

        public void FrameUpdate()
        {
            //VELOCITY
            //this.Velocity.X = this.Velocity.X / 2;
            this.Velocity.X = Math.Abs(this.Velocity.X) < 0.05 ? 0 : this.Velocity.X;

            //this.Velocity.Y = this.Velocity.Y / 2;
            this.Velocity.Y = Math.Abs(this.Velocity.Y) < 0.05 ? 0 : this.Velocity.Y;

            //POSITION
            this.Position.X += this.Velocity.X;
            this.Position.Y += this.Velocity.Y;
        }
    }
}
