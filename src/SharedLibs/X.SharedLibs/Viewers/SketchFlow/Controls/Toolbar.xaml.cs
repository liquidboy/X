using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using X.Services.Data;

namespace X.Viewer.SketchFlow.Controls
{
  public sealed partial class Toolbar : UserControl
  {
    public event EventHandler PerformAction;

    public Toolbar()
    {
      this.InitializeComponent();

      //lbSavedSketches.ItemsSource = new string[] { "sketch1", "sketch2" };
    }

    private void butOne_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("SnapViewer", EventArgs.Empty);
    }

    private void butToolbar_Click(object sender, RoutedEventArgs e)
    {
      if (spToolbar.Visibility == Visibility.Visible)
      {
        spToolbar.Visibility = Visibility.Collapsed;
        spPageSizes.Visibility = Visibility.Collapsed;
        spShapes.Visibility = Visibility.Collapsed;
        butToolbar.Background = new SolidColorBrush(Colors.LightGray);
        butAddPage.Background = new SolidColorBrush(Colors.LightGray);
        butShape.Background = new SolidColorBrush(Colors.LightGray);
      }
      else
      {
        spToolbar.Visibility = Visibility.Visible;
        butToolbar.Background = new SolidColorBrush(Colors.Gray);


      }
    }

    private void butAddPage_Click(object sender, RoutedEventArgs e)
    {
      //other buts disabled
      spShapes.Visibility = Visibility.Collapsed;
      spEntities.Visibility = Visibility.Collapsed;
      butShape.Background = new SolidColorBrush(Colors.LightGray);
      butEntity.Background = new SolidColorBrush(Colors.LightGray);

      if (spPageSizes.Visibility == Visibility.Visible)
      {
        spPageSizes.Visibility = Visibility.Collapsed;
        butAddPage.Background = new SolidColorBrush(Colors.LightGray);
      }
      else
      {
        spPageSizes.Visibility = Visibility.Visible;
        butAddPage.Background = new SolidColorBrush(Colors.Gray);
      }
    }



    private void butShape_Click(object sender, RoutedEventArgs e)
    {
      //other buts disabled
      spPageSizes.Visibility = Visibility.Collapsed;
      spEntities.Visibility = Visibility.Collapsed;
      butAddPage.Background = new SolidColorBrush(Colors.LightGray);
      butEntity.Background = new SolidColorBrush(Colors.LightGray);

      if (spShapes.Visibility == Visibility.Visible)
      {
        spShapes.Visibility = Visibility.Collapsed;
        butShape.Background = new SolidColorBrush(Colors.LightGray);
      }
      else
      {
        spShapes.Visibility = Visibility.Visible;
        butShape.Background = new SolidColorBrush(Colors.Gray);
      }
    }
    



    private void but640360_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("AddPage640360", EventArgs.Empty);
    }

    private void but200200_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("AddPageTiles", EventArgs.Empty);
    }

    private void but1200600_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("AddPage1200600", EventArgs.Empty);
    }

    private void but1600900_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("AddPage1600900", EventArgs.Empty);
    }

    private void but1400768_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("AddPage1400768", EventArgs.Empty);
    }

    private void but18001200_Click(object sender, RoutedEventArgs e)
    {
      if (PerformAction != null) PerformAction("AddPage18001200", EventArgs.Empty);
    }

    private void butCircle_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddStamp", StartPoint = pt, StampType = typeof(Stamps.Circle) });
    }

    private void butRectangle_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddStamp", StartPoint = pt, StampType = typeof(Stamps.Rectangle) });
    }

    private Windows.Foundation.Point getPoint(UIElement el)
    {
      var sv = (FrameworkElement)((FrameworkElement)this.Parent).Parent; // get canvas in the parent , tight relation between this control and the parent now
                                                                         //var cv = (UIElement)sv.FindName("cvMain"); // get canvas in parent
      var cv = (UIElement)sv.FindName("cvMainAdorner"); // get canvas in parent
      var tp = cv.TransformToVisual(el); // get transform of this button in relation to the canvas(cv)
      var pt = tp.TransformPoint(new Windows.Foundation.Point(0, 0)); // get position from the transform(tp)

      //just tweak the position so its not under toolbar
      pt.X = pt.X + 20;
      pt.Y = pt.Y - 90;

      return pt;
    }

    private void butGenericStamp_Click(object sender, RoutedEventArgs e)
    {
      var pathData = ((X.UI.Path.Path)((Button)sender).Content).Data;
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddStamp", StartPoint = pt, StampType = typeof(Windows.UI.Xaml.Shapes.Path), Data = pathData });
    }

    private void butText_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddText", StartPoint = pt, StampType = typeof(Stamps.Text) });
    }

    private void butImage_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddImage", StartPoint = pt, StampType = typeof(Stamps.Picture) });
    }

    //Picker : LoadSketch
    public void LoadSketchs(IList<SketchDataModel> sketches)
    {
      pikLoadSketch.LoadSketchs(sketches);
    }
    public void ClearSketchs()
    {
      pikLoadSketch.ClearSketchs();
    }

    private void butLoad_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("GetAllSketchs", new ToolbarEventArgs() { ActionType = "GetAllSketchs" });
    }

    private void pikLoadSketch_PerformAction(object sender, EventArgs e)
    {
      PerformAction?.Invoke(sender, e);
    }

    //Picker : SaveSketch
    private void pikSaveSketch_PerformAction(object sender, EventArgs e)
    {
      PerformAction?.Invoke(sender, e);
    }










    // ===================
    // ENTITY
    // ===================

    private void butEntity_Click(object sender, RoutedEventArgs e)
    {
      //other buts disabled
      spShapes.Visibility = Visibility.Collapsed;
      spPageSizes.Visibility = Visibility.Collapsed;
      butAddPage.Background = new SolidColorBrush(Colors.LightGray);
      butShape.Background = new SolidColorBrush(Colors.LightGray);

      if (spEntities.Visibility == Visibility.Visible)
      {
        spEntities.Visibility = Visibility.Collapsed;
        butEntity.Background = new SolidColorBrush(Colors.LightGray);
      }
      else
      {
        spEntities.Visibility = Visibility.Visible;
        butEntity.Background = new SolidColorBrush(Colors.Gray);
      }
    }

    private void butEntityContainer_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke("AddEntityContainer", EventArgs.Empty);
    }

    private void butEntityDataModel_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddEntityDM", StartPoint = pt, StampType = typeof(Stamps.Entity) });
    }

    private void butEntityProperty_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddEntityProperty", StartPoint = pt, StampType = typeof(Stamps.EntityProperty) });
    }

    private void butEntityMethod_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddEntityMethod", StartPoint = pt, StampType = typeof(Stamps.EntityMethod) });
    }

    private void butEntityLines_Click(object sender, RoutedEventArgs e)
    {
      var pt = getPoint((UIElement)sender);
      PerformAction?.Invoke(null, new ToolbarEventArgs() { ActionType = "AddEntityLines", StartPoint = pt, StampType = typeof(Stamps.EntityLines) });
    }
  }


  public class ToolbarEventArgs : EventArgs
  {
    public string ActionType;
    public Type StampType;
    public Windows.Foundation.Point StartPoint;
    public string Data;
  }
}
