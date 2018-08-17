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
using Windows.UI.Xaml.Media.Imaging;
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

    #region TopFloor
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
            if (imgCart != null) {
              ((CompositeTransform)imgCart.RenderTransform).TranslateY = newSlot.Y * 40;
            }
            
          }));

    #endregion

    
    #region CurrentFloor
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

            gotoFloor(newFloor, item.Position.Y, grd, 0);

          }));

    #endregion


    #region NumberOfCarts
    public static int GetNumberOfCarts(DependencyObject obj)
    {
      return (int)obj.GetValue(NumberOfCartsProperty);
    }

    public static void SetNumberOfCarts(DependencyObject obj, int value)
    {
      obj.SetValue(NumberOfCartsProperty, value);
    }

    public static readonly DependencyProperty NumberOfCartsProperty =
        DependencyProperty.RegisterAttached("NumberOfCarts", typeof(int), typeof(ElevatorExtensions),
          new PropertyMetadata(0, (d, e) =>
          {
            Grid grd = (Grid)d;
            IPrototype item = (IPrototype)grd.DataContext;

            var preCount = (int)e.OldValue;
            var newCount = (int)e.NewValue;
            var delta = newCount - preCount;

            if (delta == 1) {
              addCarts(grd, 1);
            }
            
          }));

    #endregion


    public static void gotoFloor(int floor, int topFloor, Grid grd, int cartId) {
      Storyboard sbMoveElevator = (Storyboard)grd.Resources[$"sbMoveElevator{cartId}"];
      DoubleAnimationUsingKeyFrames daukf = (DoubleAnimationUsingKeyFrames)sbMoveElevator.Children[0];
      EasingDoubleKeyFrame edkf = (EasingDoubleKeyFrame)daukf.KeyFrames[0];

      edkf.Value = (topFloor - floor) * 40;
      sbMoveElevator.Begin();
    }

    public static void addCarts(Grid grd, int noOfCarts) {
      Grid grdCarts = (Grid)grd.FindName("grdCarts");
      var currentCartCount = grdCarts.Children.Count();
      
      for (var i = 0; i < noOfCarts; i++) {
        var id = currentCartCount + i;

        Image img = new Image();
        img.Name = $"elCart{id}";
        img.VerticalAlignment = VerticalAlignment.Top;
        img.Source = new BitmapImage(new Uri("ms-appx:///Assets/elevator-compartment-empty.png"));
        img.Margin = new Thickness(2, 0, 0, 3);
        img.RenderTransformOrigin = new Point(0.5, 0.5);
        img.RenderTransform = new CompositeTransform();
        img.Projection = new PlaneProjection();

        Storyboard sb = new Storyboard();
        DoubleAnimationUsingKeyFrames dauk = new DoubleAnimationUsingKeyFrames();
        dauk.SetValue(Storyboard.TargetNameProperty, $"elCart{id}");
        dauk.SetValue(Storyboard.TargetPropertyProperty, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
        dauk.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.Parse("00:00:01")), Value = 0 });
        sb.Children.Add(dauk);
        grd.Resources.Add($"sbMoveElevator{id}", sb);


        grdCarts.Children.Add(img);
      }

      
    }

  }
}
