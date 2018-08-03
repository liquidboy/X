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
  public sealed partial class Floor : UserControl
  { 
    public Floor()
    {
      this.InitializeComponent();
    }

    public Brush SetBackgroundColor() {
      if (!(DataContext is IPrototype)) return new SolidColorBrush(Colors.SlateGray);
      
      IPrototype p = (IPrototype)this.DataContext;
      if(p.Position.Y >= 0) return new SolidColorBrush(Colors.SlateGray);
      return new SolidColorBrush(Colors.DarkSlateGray);
    }
  }
}
