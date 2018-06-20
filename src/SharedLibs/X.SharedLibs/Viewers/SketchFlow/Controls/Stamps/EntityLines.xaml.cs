using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using X.Viewer.SketchFlow.Controls.Pickers;

namespace X.Viewer.SketchFlow.Controls.Stamps
{


  public sealed partial class EntityLines : UserControl, IStamp
  {
    public event EventHandler PerformAction;

    public EntityLines()
    {
      this.InitializeComponent();

      tlLeftCenterToolbar.AddTab("Colors", isSelected: true);
      tlLeftCenterToolbar.AddTab("Text");
      tlLeftCenterToolbar.AddTab("Lines");
      tlLeftCenterToolbar.OnTabChanged += TlLeftCenterToolbar_OnTabChanged;

      this.Unloaded += EntityLines_Unloaded;
      cpMain.ColorTypes = new List<string>() { "Text Color", "Circle Color" };
      tpMain.FontFamilies = new List<string>() { "Neue Haas Grotesk Text Pro", "FangSong", "Kokila", "Cambria", "Courier New", "Gadugi", "Georgia", "Leelawadee UI", "Lucida Console", "Segoe MDL2 Assets", "Segoe UI", "Segoe UI Emoji", "Verdana" };
    }

    private void TlLeftCenterToolbar_OnTabChanged(object sender, EventArgs e)
    {
      var tab = (X.UI.LiteTab.Tab)sender;

      textPicker.Visibility = Visibility.Collapsed;
      colorPicker.Visibility = Visibility.Collapsed;

      switch (tab.Name)
      {
        case "Colors": colorPicker.Visibility = Visibility.Visible; break;
        case "Text": textPicker.Visibility = Visibility.Visible; break;
        case "Lines": break;
      }
    }

    private void EntityLines_Unloaded(object sender, RoutedEventArgs e)
    {

    }

    private void edges_PerformAction(object sender, EventArgs e)
    {
      var ea = e as ResizeMoveEdgesEventArgs;

      if (ea.ActionType == eActionTypes.ToolbarTopRight)
      {
        if (spToolbar.Visibility == Visibility.Visible) spToolbar.Visibility = Visibility.Collapsed;
        else spToolbar.Visibility = Visibility.Visible;
      }
      else if (ea.ActionType == eActionTypes.CenterLeft)
      {
        if (leftCenterToolBar.Visibility == Visibility.Visible) leftCenterToolBar.Visibility = Visibility.Collapsed;
        else leftCenterToolBar.Visibility = Visibility.Visible;
      }

      PerformAction?.Invoke(this, e);
    }

    private void butClose_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke(this, new EntityLinesEventArgs() { ActionType = eActionTypes.CloseStamp });
    }

    private void butStamp_Click(object sender, RoutedEventArgs e)
    {
      PerformAction?.Invoke(this, new EntityLinesEventArgs() { ActionType = eActionTypes.CreateFromStamp });
    }



    public void UpdateRotation(double angle)
    {
      //((CompositeTransform)elParent.RenderTransform).Rotation = angle;
      //grdGridRotationMarkers.RotationAngle = angle;
    }

    private void cpMain_ColorChanged(object sender, EventArgs e)
    {
      //var cpea = e as ColorPickerEventArgs;
      //if (cpea.ColorType == "Text Color") {
      //  elTxt.Foreground = (Brush)sender;
      //}
      //else if (cpea.ColorType == "Circle Color")
      //{
      //  el.Stroke = (Brush)sender;
      //}
    }

    //private void butGridMarker_Click(object sender, RoutedEventArgs e)
    //{
    //  PerformAction?.Invoke(this, new EntityLinesEventArgs() { ActionType = eActionTypes.ToggleGridMarkers });

    //  if (grdGridMarkers.Visibility == Visibility.Visible) grdGridMarkers.Visibility = Visibility.Collapsed;
    //  else grdGridMarkers.Visibility = Visibility.Visible;

    //  grdGridRotationMarkers.Visibility = grdGridMarkers.Visibility;
    //}


    public string GenerateXAML(string uid, double scaleX, double scaleY, double left, double top)
    {
      //var rotationAngle = ((CompositeTransform)elParent.RenderTransform).Rotation;
      //var leftToUse = left;
      //var topToUse = top;
      //var rotationXaml = $"<StackPanel.RenderTransform><CompositeTransform Rotation=\"{ rotationAngle }\" /></StackPanel.RenderTransform>";
      //if (rotationAngle == 0) rotationXaml = "";
      //var newFontSize = elTxt.FontSize * (1 / scaleX);

      //var xamlTemplate = @"
      //  <StackPanel x:Name=""{0}"" Orientation=""Horizontal"" RenderTransformOrigin=""0.5,0.5"" VerticalAlignment=""Top"" HorizontalAlignment=""Stretch"" Canvas.Left=""{1}"" Canvas.Top=""{2}"">
      //    <StackPanel.RenderTransform>
      //      <CompositeTransform Rotation=""{3}""></CompositeTransform>
      //    </StackPanel.RenderTransform>
      //    <Rectangle Stroke=""{4}"" RenderTransformOrigin=""0.5,0.5"" Height=""{5}"" Width=""{6}""></Rectangle>
      //    <TextBlock Foreground=""{7}"" FontSize=""{8}"" Text=""{9}"" TextWrapping=""WrapWholeWords"" IsColorFontEnabled=""True"" VerticalAlignment=""Center"" Margin=""10,0,0,0"">
      //    </TextBlock>
      //  </StackPanel>
      //";

      //return string.Format(
      //    xamlTemplate,
      //    uid,
      //    leftToUse,
      //    topToUse,
      //    rotationAngle,
      //    ((SolidColorBrush)el.Stroke).Color.ToString(),
      //    (el.Height * (1 / scaleY)),
      //    (el.Width * (1 / scaleX)),
      //    ((SolidColorBrush)elTxt.Foreground).Color.ToString(),
      //    newFontSize,
      //    elTxt.Text
      //  );
      return string.Empty;
    }

    public void PopulateFromUIElement(UIElement element)
    {
      throw new NotImplementedException();
    }

    private void butLock_Click(object sender, RoutedEventArgs e)
    {
      butLock.Visibility = Visibility.Collapsed;
      butUnlock.Visibility = Visibility.Visible;
      edges.IsLocked = true;
    }

    private void butUnlock_Click(object sender, RoutedEventArgs e)
    {
      butLock.Visibility = Visibility.Visible;
      butUnlock.Visibility = Visibility.Collapsed;
      edges.IsLocked = false;
    }

    public void GenerateFromXAML(UIElement template)
    {
      //if (template is StackPanel)
      //{
      //  var elTemplate = template as StackPanel;
      //  var elChild1Rectangle = elTemplate.Children[0] as Windows.UI.Xaml.Shapes.Rectangle ;
      //  var elChild2TextBlock = elTemplate.Children[1] as TextBlock;
      //  try { ((CompositeTransform)elParent.RenderTransform).Rotation = ((CompositeTransform)elTemplate.RenderTransform).Rotation; } catch { }
      //  el.Stroke = elChild1Rectangle.Stroke;
      //  el.Height = elChild1Rectangle.Height;
      //  el.Width = elChild1Rectangle.Width;
      //  elTxt.Text = elChild2TextBlock.Text;
      //  elTxt.Foreground = elChild2TextBlock.Foreground;
      //  elTxt.FontSize = elChild2TextBlock.FontSize;
      //}
    }

    public void UpdateStrokeThickness(double thickness) {
      //if (thickness > 0) elTxt.FontSize = thickness;
    }
    public string GetData() { return string.Empty; }

    private void tpMain_TextChanged(object sender, EventArgs e)
    {
      //var tea = e as TextPickerEventArgs;
      //elTxt.Text = tea.Text;
      //if (!string.IsNullOrEmpty(tea.FontFamily)) elTxt.FontFamily = new FontFamily(tea.FontFamily);
    }
  }

  public class EntityLinesEventArgs : EventArgs, IStampEventArgs
  {
    public eActionTypes ActionType { get; set; }

  }
}
