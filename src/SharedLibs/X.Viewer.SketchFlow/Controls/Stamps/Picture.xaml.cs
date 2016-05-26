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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using X.Viewer.SketchFlow.Controls.Pickers;

namespace X.Viewer.SketchFlow.Controls.Stamps
{


    public sealed partial class Picture : UserControl, IStamp
    {
        public event EventHandler PerformAction;

        public Picture()
        {
            this.InitializeComponent();

            this.Loaded += Picture_Loaded;
            this.Unloaded += Picture_Unloaded;
            cpMain.ColorTypes = new List<string>() { "Stroke", "Fill" };

            tlLeftCenterToolbar.AddTab("Colors", isSelected: true);
            tlLeftCenterToolbar.AddTab("Image");
            tlLeftCenterToolbar.OnTabChanged += TlLeftCenterToolbar_OnTabChanged;
        }

        private void Picture_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPictureLibrary();
        }

        private void TlLeftCenterToolbar_OnTabChanged(object sender, EventArgs e)
        {
            var tab = (X.UI.LiteTab.Tab)sender;

            cpMain.Visibility = Visibility.Collapsed;
            ipMain.Visibility = Visibility.Collapsed;

            switch (tab.Name)
            {
                case "Colors": cpMain.Visibility = Visibility.Visible; break;
                case "Image": ipMain.Visibility = Visibility.Visible; break;
            }
        }

        private void Picture_Unloaded(object sender, RoutedEventArgs e)
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
            PerformAction?.Invoke(this, new PictureEventArgs() { ActionType = eActionTypes.CloseStamp } );
        }

        private void butStamp_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new PictureEventArgs() { ActionType = eActionTypes.CreateFromStamp });
        }

     

        public void UpdateRotation(double angle)
        {
            ((CompositeTransform)el.RenderTransform).Rotation = angle;
            grdGridRotationMarkers.RotationAngle = angle;
            
        }

        private void cpMain_ColorChanged(object sender, EventArgs e)
        {
            var cpea = e as ColorPickerEventArgs;
            //if(cpea.ColorType == "Stroke") el.Stroke = (Brush)sender;
            //else if (cpea.ColorType == "Fill") el.Fill = (Brush)sender;
        }

        private void ipMain_TextChanged(object sender, EventArgs e)
        {
            var ipea = e as ImagePickerEventArgs;
            //el.Source = new BitmapImage(new Uri(ipea.Text));
        }

        private void butGridMarker_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new PictureEventArgs() { ActionType = eActionTypes.ToggleGridMarkers });

            if (grdGridMarkers.Visibility == Visibility.Visible) grdGridMarkers.Visibility = Visibility.Collapsed;
            else  grdGridMarkers.Visibility = Visibility.Visible;

            grdGridRotationMarkers.Visibility = grdGridMarkers.Visibility;
        }


        public string GenerateXAML(string uid, double scaleX, double scaleY, double left, double top)
        {
            var rotationAngle = ((CompositeTransform)el.RenderTransform).Rotation;
            var leftToUse = left;
            var topToUse = top;
            var rotationXaml = $"<Image.RenderTransform><CompositeTransform Rotation=\"{ rotationAngle }\" /></Image.RenderTransform>";
            if (rotationAngle == 0) rotationXaml = "";
            
            return $"<Image x:Name=\"{uid}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" Height=\"{ (this.Height * (1 / scaleY)) }\" Width=\"{ (this.Width * (1 / scaleX)) }\"  Canvas.Left=\"{ leftToUse }\" Canvas.Top=\"{ topToUse }\" RenderTransformOrigin=\"0.5,0.5\" Source=\"{ ((Windows.UI.Xaml.Media.Imaging.BitmapImage)el.Source).UriSource }\" Stretch=\"{ el.Stretch }\" >{ rotationXaml }</Image>";
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
            if (template is Image) {
                var elTemplate = template as Image;
                try { ((CompositeTransform)el.RenderTransform).Rotation = ((CompositeTransform)elTemplate.RenderTransform).Rotation; } catch { }
                el.Source = elTemplate.Source;
                el.Stretch = elTemplate.Stretch;
                //el.Stroke = elTemplate.Stroke;
                //el.StrokeThickness = elTemplate.StrokeThickness;
                //el.Fill = elTemplate.Fill;
            }
        }

        public void UpdateStrokeThickness(double thickness) { }
        public string GetData() { return string.Empty; }

        public void LoadPictureLibrary()
        {
            
        }
    }

    public class PictureEventArgs : EventArgs, IStampEventArgs
    {
        public eActionTypes ActionType { get; set; }
        
    }
}
