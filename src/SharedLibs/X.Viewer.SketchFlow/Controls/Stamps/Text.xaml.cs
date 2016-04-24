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
using Windows.UI.Xaml.Shapes;
using X.Viewer.SketchFlow.Controls.Pickers;

namespace X.Viewer.SketchFlow.Controls.Stamps
{


    public sealed partial class Text : UserControl, IStamp
    {
        public event EventHandler PerformAction;

        public Text()
        {
            this.InitializeComponent();

            this.Unloaded += Text_Unloaded;
            cpMain.ColorTypes = new List<string>() { "Foreground", "SelectionHighlightColor" };
        }

        private void Text_Unloaded(object sender, RoutedEventArgs e)
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
            else if (ea.ActionType == eActionTypes.CenterLeft) {
                if (leftCenterToolBar.Visibility == Visibility.Visible) leftCenterToolBar.Visibility = Visibility.Collapsed;
                else leftCenterToolBar.Visibility = Visibility.Visible;
            }

            PerformAction?.Invoke(this, e);
        }

        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new TextEventArgs() { ActionType = eActionTypes.CloseStamp } );
        }

        private void butStamp_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new TextEventArgs() { ActionType = eActionTypes.CreateFromStamp });
        }

     

        public void UpdateRotation(double angle)
        {
            ((CompositeTransform)el.RenderTransform).Rotation = angle;
            //((CompositeTransform)grdGridRotationMarkers.RenderTransform).Rotation = angle;
            grdGridRotationMarkers.RotationAngle = angle;
            
        }

        private void cpMain_ColorChanged(object sender, EventArgs e)
        {
            var cpea = e as ColorPickerEventArgs;
            if(cpea.ColorType == "Foreground") el.Foreground = (Brush)sender;
            else if (cpea.ColorType == "SelectionHighlightColor") el.SelectionHighlightColor = (SolidColorBrush)sender;
        }

        private void tpMain_TextChanged(object sender, EventArgs e)
        {
            var tea = e as TextPickerEventArgs;
            el.Text = tea.Text;
        }

        private void butGridMarker_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new TextEventArgs() { ActionType = eActionTypes.ToggleGridMarkers });

            if (grdGridMarkers.Visibility == Visibility.Visible) grdGridMarkers.Visibility = Visibility.Collapsed;
            else  grdGridMarkers.Visibility = Visibility.Visible;

            grdGridRotationMarkers.Visibility = grdGridMarkers.Visibility;
        }


        public string GenerateXAML(string uid, double scaleX, double scaleY, double left, double top)
        {
            var rotationAngle = ((CompositeTransform)el.RenderTransform).Rotation;
            var leftToUse = left;
            var topToUse = top;
            var rotationXaml = $"<TextBlock.RenderTransform><CompositeTransform Rotation=\"{ rotationAngle }\" /></TextBlock.RenderTransform>";
            if (rotationAngle == 0) rotationXaml = "";

            var fillColor = (el.SelectionHighlightColor!=null)? ((SolidColorBrush)el.SelectionHighlightColor).Color.ToString(): "";
            var fillXaml = fillColor.Length >0 ? $"SelectionHighlightColor=\"{fillColor}\"": "";

            var newStroke = el.FontSize * (1 / scaleX);
            
            return $"<TextBlock x:Name=\"{uid}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" Height=\"{ (this.Height * (1 / scaleY)) }\" Width=\"{ (this.Width * (1 / scaleX)) }\"  StrokeThickness=\"{ newStroke }\" Foreground=\"{ ((SolidColorBrush)el.Foreground).Color.ToString() }\" Canvas.Left=\"{ leftToUse }\" Canvas.Top=\"{ topToUse }\" RenderTransformOrigin=\"0.5,0.5\" { fillXaml } >{ rotationXaml }</TextBlock>";
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
            if (template is Ellipse) {
                var elTemplate = template as Ellipse;
                try { ((CompositeTransform)el.RenderTransform).Rotation = ((CompositeTransform)elTemplate.RenderTransform).Rotation; } catch { }
                el.Foreground = elTemplate.Stroke;
                el.FontSize = elTemplate.StrokeThickness;
                el.SelectionHighlightColor = (SolidColorBrush)elTemplate.Fill;
            }
        }

        public void UpdateStrokeThickness(double thickness) { if (thickness >= 0) el.FontSize = thickness; }
        public string GetData() { return string.Empty; }


    }

    public class TextEventArgs : EventArgs, IStampEventArgs
    {
        public eActionTypes ActionType { get; set; }
        public string Text { get; set; }

    }
}
