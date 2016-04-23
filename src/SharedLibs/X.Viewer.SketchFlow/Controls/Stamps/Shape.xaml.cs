using CoreLib.Converters;
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


    public sealed partial class Shape : UserControl, IStamp
    {
        public event EventHandler PerformAction;

        public Shape()
        {
            this.InitializeComponent();

            this.Unloaded += Shape_Unloaded;
            cpMain.ColorTypes = new List<string>() { "Stroke", "Fill" };
        }


        public object StampContent
        {
            get { return (object)GetValue(StampContentProperty); }
            set { SetValue(StampContentProperty, value); }
        }

        public Type StampType
        {
            get { return (Type)GetValue(StampTypeProperty); }
            set { SetValue(StampTypeProperty, value); }
        }

        public string StampData
        {
            get { return (string)GetValue(StampDataProperty); }
            set { SetValue(StampDataProperty, value); }
        }

        public static readonly DependencyProperty StampDataProperty = DependencyProperty.Register("StampData", typeof(string), typeof(Shape), new PropertyMetadata(null));
        public static readonly DependencyProperty StampContentProperty = DependencyProperty.Register("StampContent", typeof(object), typeof(Shape), new PropertyMetadata(null));
        public static readonly DependencyProperty StampTypeProperty = DependencyProperty.Register("StampType", typeof(Type), typeof(Shape), new PropertyMetadata(null));
        

        private void Shape_Unloaded(object sender, RoutedEventArgs e)
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
                if (colorPicker.Visibility == Visibility.Visible) colorPicker.Visibility = Visibility.Collapsed;
                else colorPicker.Visibility = Visibility.Visible;
            }

            PerformAction?.Invoke(this, e);
        }

        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ShapeEventArgs() { ActionType = eActionTypes.CloseStamp } );
        }

        private void butStamp_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ShapeEventArgs() { ActionType = eActionTypes.CreateFromStamp });
        }

     

        public void UpdateRotation(double angle)
        {
            ((CompositeTransform)elContent.RenderTransform).Rotation = angle;
            //((CompositeTransform)grdGridRotationMarkers.RenderTransform).Rotation = angle;
            grdGridRotationMarkers.RotationAngle = angle;
            
        }

        private void cpMain_ColorChanged(object sender, EventArgs e)
        {
            var cpea = e as ColorPickerEventArgs;
            var el = (Windows.UI.Xaml.Shapes.Shape)elContent.Content;
            if (cpea.ColorType == "Stroke") el.Stroke = (Brush)sender;
            else if (cpea.ColorType == "Fill") el.Fill = (Brush)sender;
        }

        private void butGridMarker_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new ShapeEventArgs() { ActionType = eActionTypes.ToggleGridMarkers });

            if (grdGridMarkers.Visibility == Visibility.Visible) grdGridMarkers.Visibility = Visibility.Collapsed;
            else  grdGridMarkers.Visibility = Visibility.Visible;

            grdGridRotationMarkers.Visibility = grdGridMarkers.Visibility;
        }


        public string GenerateXAML(string uid, double scaleX, double scaleY, double left, double top)
        {
            var el = (Windows.UI.Xaml.Shapes.Shape)elContent.Content;
            var rotationAngle = ((CompositeTransform)elContent.RenderTransform).Rotation;
            var leftToUse = left;
            var topToUse = top;
            var rotationXaml = $"<Path.RenderTransform><CompositeTransform Rotation=\"{ rotationAngle }\" /></Path.RenderTransform>";
            if (rotationAngle == 0) rotationXaml = "";

            var fillColor = (el.Fill!=null)? ((SolidColorBrush)el.Fill).Color.ToString(): "";
            var fillXaml = fillColor.Length >0 ? $"Fill=\"{fillColor}\"": "";

            var newStroke = el.StrokeThickness * (1 / scaleX);
            var dataXaml = $" Data=\"{ StampData }\""; 
            //string pthString = $"<Path Data=\"{ data }\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Stretch\" Fill=\"DarkOrange\" Stretch=\"Uniform\" />";
            return $"<Path x:Name=\"{uid}\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Stretch\" Height=\"{ (this.Height * (1 / scaleY)) }\" Width=\"{ (this.Width * (1 / scaleX)) }\"  StrokeThickness=\"{ newStroke }\" Stretch=\"Uniform\" Stroke=\"{ ((SolidColorBrush)el.Stroke).Color.ToString() }\" Canvas.Left=\"{ leftToUse }\" Canvas.Top=\"{ topToUse }\" RenderTransformOrigin=\"0.5,0.5\" { fillXaml } { dataXaml }>{ rotationXaml }</Path>";
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
                var elTemplate = template as Windows.UI.Xaml.Shapes.Shape;
                var el = (Windows.UI.Xaml.Shapes.Shape)elContent.Content;
                try { ((CompositeTransform)elContent.RenderTransform).Rotation = ((CompositeTransform)elTemplate.RenderTransform).Rotation; } catch { }
                el.Stroke = elTemplate.Stroke;
                el.StrokeThickness = elTemplate.StrokeThickness;
                el.Fill = elTemplate.Fill;
            } else if (template is Windows.UI.Xaml.Shapes.Path) {
                var elTemplate = template as Windows.UI.Xaml.Shapes.Path;

                string pthString = $"<Path xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Data=\"{ StampData }\" HorizontalAlignment=\"Stretch\" VerticalAlignment=\"Stretch\" Fill=\"DarkOrange\" Stroke=\"DarkOrange\" Stretch=\"Uniform\" />";
                var el = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
                elContent.Content = el;
                
                var eeeee = (((Windows.UI.Xaml.Shapes.Path)template).Data).ToString(); ;
                try { ((CompositeTransform)elContent.RenderTransform).Rotation = ((CompositeTransform)elTemplate.RenderTransform).Rotation; } catch { }
                el.Stroke = elTemplate.Stroke;
                el.StrokeThickness = elTemplate.StrokeThickness;
                el.Fill = elTemplate.Fill;
            }
        }

        public void UpdateStrokeThickness(double thickness) { ((Windows.UI.Xaml.Shapes.Shape)elContent.Content).StrokeThickness = thickness; }

        public string GetData() { return StampData; }
    }

    public class ShapeEventArgs : EventArgs, IStampEventArgs
    {
        public eActionTypes ActionType { get; set; }
        
    }
}
