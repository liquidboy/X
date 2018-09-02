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

namespace SampleAStar
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    public MainPage()
    {
      this.InitializeComponent();
    }
    
    private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
    {
      DrawMazeAndRoute();
    }

    private void DrawMazeAndRoute() {
      ClearRoute();
      DrawMaze();
      DrawRoute(new Point(2, 4));
    }

    private void DrawMaze() {
      //GENERATE A MAP
      var mapDimension = new Point(30, 30);
      var mazeGenerator = new MazeGenerator(mapDimension);
      var newMap = mazeGenerator.GetGeneratedMazeAsSingleDimensionArray(false);
      Map.Instance.SetMap(mapDimension, newMap);
    }

    private void ClearRoute() {
      foreach (VisualNode vn in cvLayout.Children) {
        vn.NodeClicked -= Vn_NodeClicked;
      }
      cvLayout.Children.Clear();
    }

    private void DrawRoute(Point end) {
      //DRAW MAP
      for (int y = 0; y < Map.Instance.MAP_HEIGHT; y++)
      {
        for (int x = 0; x < Map.Instance.MAP_WIDTH; x++)
        {
          var wh = 40d;
          var pt = new Point(x * wh, y * wh);

          VisualNode vn = new VisualNode();
          vn.Name = $"vnx{x}y{y}";
          vn.SetValue(Canvas.LeftProperty, pt.X);
          vn.SetValue(Canvas.TopProperty, pt.Y);
          vn.Width = wh;
          vn.Height = wh;
          vn.DrawPosition(x, y);
          vn.SetDot(false);
          vn.SetWall(Map.Instance.GetMap(x, y));
          vn.SetPosition(x, y);
          vn.NodeClicked += Vn_NodeClicked;
          cvLayout.Children.Add(vn);
        }
      }

      //START STAR SEARCH THROUGH MAP
      AStarExample.Start(cvLayout, end);
    }

    private void Vn_NodeClicked(object sender, EventArgs e)
    {
      string positionAsString = (string)sender;
      var positionAsParts = positionAsString.Split(",".ToCharArray());
      ClearRoute();
      DrawRoute(new Point(int.Parse(positionAsParts[0]), int.Parse(positionAsParts[1])));
    }

    private void butDrawNew_Click(object sender, RoutedEventArgs e)
    {
      DrawMazeAndRoute();
    }
  }
}
