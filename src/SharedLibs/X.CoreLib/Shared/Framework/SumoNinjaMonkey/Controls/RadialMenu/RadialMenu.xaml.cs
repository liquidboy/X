using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
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


namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class RadialMenu : UserControl
    {

        static int _defaultOuterDiameter = 240; // + 54 FOR THE GROWTH
        static int defaultOuterBorderWidth = 25;



        #region DefaultOuterDiameter
        public int DefaultOuterDiameter
        {
            get { return (int)GetValue(DefaultOuterDiameterProperty); }
            set { SetValue(DefaultOuterDiameterProperty, value); }
        }


        public static readonly DependencyProperty DefaultOuterDiameterProperty =
            DependencyProperty.Register("DefaultOuterDiameter", typeof(int), typeof(RadialMenu), new PropertyMetadata(_defaultOuterDiameter, DefaultOuterDiameterPropertyChanged));

        private static void DefaultOuterDiameterPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RadialMenu rm = (RadialMenu)obj;

            ((DoubleAnimationUsingKeyFrames)rm.sbShowMainMenu.Children[0]).KeyFrames[0].Value = (int)args.NewValue + (defaultOuterBorderWidth * 2);
            ((DoubleAnimationUsingKeyFrames)rm.sbShowMainMenu.Children[1]).KeyFrames[0].Value = defaultOuterBorderWidth;

            ((DoubleAnimationUsingKeyFrames)rm.sbHideMainMenu.Children[0]).KeyFrames[0].Value = (int)args.NewValue;

            rm.MainOuterCircleBorderBackgroundDiameterAnim = (int)args.NewValue;

            rm.elcb1.Width = (int)args.NewValue;
            rm.elcb1.Height = (int)args.NewValue;
        }       
        #endregion

        List<Arc> _arcs = new List<Arc>();

        public RadialMenu()
        {


            this.InitializeComponent();

            sbHideMainMenu.Completed += (o, a) => { };
            sbShowMainMenu.Completed += (o, a) => {

                Arc arc = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                arc.ParentMenuId = 0;
                arc.MenuId = 1;
                arc.BuildArcAwayFromCenter(DefaultOuterDiameter, defaultOuterBorderWidth, Windows.UI.Colors.Purple, -22, 22);
                grdButtons.Children.Add(arc);
                _arcs.Add(arc);


                arc = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                arc.ParentMenuId = 0;
                arc.MenuId = 2;
                arc.BuildArcAwayFromCenter(DefaultOuterDiameter, defaultOuterBorderWidth, Windows.UI.Colors.Purple, 23, 67);
                grdButtons.Children.Add(arc);
                _arcs.Add(arc);


                arc = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                arc.ParentMenuId = 0;
                arc.MenuId = 3;
                arc.BuildArcAwayFromCenter(DefaultOuterDiameter, defaultOuterBorderWidth, Windows.UI.Colors.Gray, 158, 202);
                grdButtons.Children.Add(arc);
                _arcs.Add(arc);

            };


            Messenger.Default.Register<ArcMenuItemSelectedMessage>(
            this,
            DoArcMenuItemSelectedMessageCallback);



        }

        bool isShowingSubMenu = false;
        private void DoArcMenuItemSelectedMessageCallback(ArcMenuItemSelectedMessage msg)
        {
            if (msg.Identifier != "AMS") return;




            if (isShowingSubMenu)
            {
                //Remove all Submenu items
                foreach (var arc in _arcs)
                {
                    if (arc.ParentMenuId != 0) arc.Unload();
                }

                //Show all main menu items
                foreach (var arc in _arcs)
                {
                    if (arc.ParentMenuId == 0) arc.Visibility = Visibility.Visible;

                }
            }
            else
            {

                //Hide main Menu items not related to the main menu item selected
                foreach (var arc in _arcs)
                {
                    if (arc.ParentMenuId == 0 && arc.MenuId != int.Parse(msg.Content)) arc.Visibility = Visibility.Collapsed;
                }

                //Remove all Submenu items if there are any
                _arcs.RemoveAll(x=>x.ParentMenuId!=0);

                //Get ParentArc
                var parentArc = _arcs.Where(x => x.MenuId == int.Parse((string)msg.Content)).First();


                //show submenu
                if (parentArc.MenuId == 1)
                {
                    double startAngle = parentArc.AngleStart;
                    Arc arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = parentArc.MenuId;
                    arcChild.MenuId = 10;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Purple, 2, startAngle, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);


                    startAngle += 44;
                    arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = int.Parse((string)msg.Content);
                    arcChild.MenuId = 11;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Yellow, 2, startAngle + 1, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);

                    startAngle += 44;
                    arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = int.Parse((string)msg.Content);
                    arcChild.MenuId = 12;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Orange, 2, startAngle + 1, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);

                }
                else if (parentArc.MenuId == 2)
                {
                    double startAngle = parentArc.AngleStart;
                    Arc arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = parentArc.MenuId;
                    arcChild.MenuId = 10;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Purple, 2, startAngle, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);


                    startAngle += 44;
                    arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = int.Parse((string)msg.Content);
                    arcChild.MenuId = 11;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Purple, 2, startAngle + 1, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);


                }
                else if (parentArc.MenuId == 3)
                {
                    double startAngle = parentArc.AngleStart;
                    Arc arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = parentArc.MenuId;
                    arcChild.MenuId = 10;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Gray, 2, startAngle, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);


                    startAngle += 44;
                    arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = int.Parse((string)msg.Content);
                    arcChild.MenuId = 11;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Gray, 2, startAngle + 1, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);

                    startAngle += 44;
                    arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = int.Parse((string)msg.Content);
                    arcChild.MenuId = 12;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Gray, 2, startAngle + 1, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);

                    startAngle += 44;
                    arcChild = new Arc(layoutRoot.ActualWidth, layoutRoot.ActualHeight);
                    arcChild.ParentMenuId = int.Parse((string)msg.Content);
                    arcChild.MenuId = 13;
                    arcChild.BuildArcTowardsCenter(DefaultOuterDiameter, 60, Windows.UI.Colors.Gray, 2, startAngle + 1, startAngle + 44, false);
                    grdButtons.Children.Add(arcChild);
                    _arcs.Add(arcChild);
                }
            }

            isShowingSubMenu = !isShowingSubMenu;
        }




        #region MainOuterCircleBorderBackgroundDiameterAnim
        public double MainOuterCircleBorderBackgroundDiameterAnim
        {
            get { return (double)GetValue(MainOuterCircleBorderBackgroundDiameterAnimProperty); }
            set { SetValue(MainOuterCircleBorderBackgroundDiameterAnimProperty, value); }
        }

        public static readonly DependencyProperty MainOuterCircleBorderBackgroundDiameterAnimProperty =
            DependencyProperty.Register("MainOuterCircleBorderBackgroundDiameterAnim", typeof(double), typeof(RadialMenu), new PropertyMetadata(_defaultOuterDiameter, MainOuterCircleBorderBackgroundDiameterAnimPropertyChanged));

        private static void MainOuterCircleBorderBackgroundDiameterAnimPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RadialMenu rm = (RadialMenu)obj;
            rm.elcb1.Width = (double)args.NewValue;
            rm.elcb1.Height = (double)args.NewValue;
        }
        #endregion


        #region MainOuterCircleBorderWidthAnim
        public double MainOuterCircleBorderWidthAnim
        {
            get { return (double)GetValue(MainOuterCircleBorderWidthAnimProperty); }
            set { SetValue(MainOuterCircleBorderWidthAnimProperty, value); }
        }

        public static readonly DependencyProperty MainOuterCircleBorderWidthAnimProperty =
            DependencyProperty.Register("MainOuterCircleBorderWidthAnim", typeof(double), typeof(RadialMenu), new PropertyMetadata(0, MainOuterCircleBorderWidthAnimPropertyChanged));

        private static void MainOuterCircleBorderWidthAnimPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            RadialMenu rm = (RadialMenu)obj;
            rm.elcb1.StrokeThickness = (double)args.NewValue;
        }
        #endregion



        bool isShowingOuterBorder = false;
        private void grdMainActivationButton_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            sbToggleActivationButton.Begin();
            isShowingSubMenu = false;

            if (isShowingOuterBorder)
            {
                foreach (var arc in _arcs)
                {
                    arc.Unload();
                }

                sbHideMainMenu.Begin();

            }
            else
            {
                foreach (var arc in _arcs)
                {
                    grdButtons.Children.Remove(arc);
                }

                //grdButtons.Children.Clear();

                sbShowMainMenu.Begin();
            }

            isShowingOuterBorder = !isShowingOuterBorder;
        }




    }
}
