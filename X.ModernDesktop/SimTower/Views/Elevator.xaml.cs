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

    public static Slot GetFloor(DependencyObject obj)
    {
      return (Slot)obj.GetValue(FloorProperty);
    }

    public static void SetFloor(DependencyObject obj, Slot value)
    {
      obj.SetValue(FloorProperty, value);
    }

    public static readonly DependencyProperty FloorProperty =
        DependencyProperty.RegisterAttached("Floor", typeof(Slot), typeof(ElevatorExtensions), new PropertyMetadata(0, (d, e) => {


        }));
  }
}
