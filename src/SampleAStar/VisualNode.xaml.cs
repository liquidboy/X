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

namespace SampleAStar
{
  public sealed partial class VisualNode : UserControl
  {
    public event EventHandler NodeClicked;
    
    public string Position { get; set; }

    public VisualNode()
    {
      this.InitializeComponent();
    }

    public void DrawPosition(double x, double y) {
      labelPosition.Text = $"{x},{y}";
    }

    public void SetDot(bool isVisible) {
      elDot.Visibility = isVisible? Visibility.Visible: Visibility.Collapsed ;
    }

    public void SetWall(int status) {
      switch (status) {
        case 9:
          recWall.Opacity = 100;
          labelPosition.Foreground = new SolidColorBrush(Colors.White);
          break;
        default:
          recWall.Opacity = 0;
          break;
      }
    }

    public void SetPosition(int x, int y) { Position = $"{x},{y}"; }
    private void layoutRoot_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
      NodeClicked?.Invoke(Position, new EventArgs());
    }
  }
}
