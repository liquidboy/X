using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using X.ModernDesktop.SimTower.Models;
using X.ModernDesktop.SimTower.Models.Item;

namespace X.ModernDesktop.SimTower.Views
{
  public sealed partial class Elevator : UserControl
  { 
    public Elevator()
    {
      this.InitializeComponent();
    }
    
  }

  public static class ElevatorExtensions {


    public static Slot GetTopFloor(DependencyObject obj)
    {
      return (Slot)obj.GetValue(TopFloorProperty);
    }

    public static void SetTopFloor(DependencyObject obj, Slot value)
    {
      obj.SetValue(TopFloorProperty, value);
    }

    public static readonly DependencyProperty TopFloorProperty =
        DependencyProperty.RegisterAttached("TopFloor", typeof(Slot), typeof(ElevatorExtensions),
          new PropertyMetadata(0, (d, e) =>
          {
            Grid grd = (Grid)d;
            Slot newSlot = (Slot)e.NewValue;

            //set elevator to floor 0
            Image imgCart = (Image)grd.FindName("elCart");
            ((CompositeTransform)imgCart.RenderTransform).TranslateY = newSlot.Y * 40;

          }));








    public static int GetCurrentFloor(DependencyObject obj)
    {
      return (int)obj.GetValue(CurrentFloorProperty);
    }

    public static void SetCurrentFloor(DependencyObject obj, int value)
    {
      obj.SetValue(CurrentFloorProperty, value);
    }

    public static readonly DependencyProperty CurrentFloorProperty =
        DependencyProperty.RegisterAttached("CurrentFloor", typeof(int), typeof(ElevatorExtensions),
          new PropertyMetadata(0, (d, e) =>
          {
            Grid grd = (Grid)d;
            IPrototype item = (IPrototype)grd.DataContext;
            var newFloor = (int)e.NewValue;

            gotoFloor(newFloor, item.Position.Y, grd);

          }));






    public static void gotoFloor(int floor, int topFloor, Grid grd) {
      Storyboard sbMoveElevator = (Storyboard)grd.Resources["sbMoveElevator"];
      DoubleAnimationUsingKeyFrames daukf = (DoubleAnimationUsingKeyFrames)sbMoveElevator.Children[0];
      EasingDoubleKeyFrame edkf = (EasingDoubleKeyFrame)daukf.KeyFrames[0];

      edkf.Value = (topFloor - floor) * 40;
      sbMoveElevator.Begin();
    }

  }
}
