using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace X.UI.EffectLayer
{
    public sealed class EffectLayer : Control
    {
        ContentControl _bkgLayer;
        private GlowEffectGraph glowEffectGraph = new GlowEffectGraph();
        private CanvasControl canvas;

        double ParentWidth;
        double ParentHeight;
        private double MaxGlowAmount = 2;
        private double ExpandAmount { get { return Math.Max(GlowAmount, MaxGlowAmount) * 2; } }

        double offsetY = 0;
        double offsetX = 0;

        bool doDlayedInitBkgLayerLaterAfterOnApplyTemplate = false;


        public EffectLayer()
        {
            this.DefaultStyleKey = typeof(EffectLayer);

            Loaded += EffectLayer_Loaded;
            Unloaded += EffectLayer_Unloaded;
        }

        protected override void OnApplyTemplate()
        {
            if (_bkgLayer == null) _bkgLayer = GetTemplateChild("bkgLayer") as ContentControl;

            if (doDlayedInitBkgLayerLaterAfterOnApplyTemplate) InitBkgLayer();


            base.OnApplyTemplate();
        }
        

        private void EffectLayer_Unloaded(object sender, RoutedEventArgs e)
        {
            // Explicitly remove references to allow the Win2D controls to get garbage collected
            if (canvas != null)
            {
                canvas.Draw -= OnDraw;
                canvas.RemoveFromVisualTree();
                canvas = null;
            }
        }
        

        private void EffectLayer_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        public void InitLayer(double canvasWidth, double canvasHeight) {
            
            ParentWidth = canvasWidth - offsetX;
            ParentHeight = canvasHeight - offsetY;

            //x    
            canvas = new CanvasControl();
            if (ShowGlowArea) canvas.ClearColor = Windows.UI.Colors.CornflowerBlue;
            canvas.Width = ParentWidth + ExpandAmount;
            canvas.Height = ParentHeight + ExpandAmount;

            //if the control begins invisible or hidden then it will not have rendered its tree yet 
            //we can only do this init if bkglayer exists, which means only after OnApplyTemplate
            //.. this is such a hack BUT it's a chicken and egg problem, sometimes the InitLayer gets
            //called too early :(
            if (_bkgLayer == null) doDlayedInitBkgLayerLaterAfterOnApplyTemplate = true;
            else InitBkgLayer();
        }


        private void InitBkgLayer() {
            
            _bkgLayer.Width = canvas.Width;
            _bkgLayer.Height = canvas.Height;

            ((CompositeTransform)_bkgLayer.RenderTransform).TranslateX = -1 * (ExpandAmount / 2) + offsetX;
            ((CompositeTransform)_bkgLayer.RenderTransform).TranslateY = -1 * (ExpandAmount / 2) + offsetY;

            canvas.Draw += OnDraw;
            _bkgLayer.Content = canvas;

            //canvas.Visibility = Visibility.Collapsed;
        }

        List<string> _paths = new List<string>();
        public void DrawPath(string path)
        {
            _paths.Add(path);
        }

        //x
        private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var sz = new Size(ParentWidth, ParentHeight);

            if(_paths.Count>0)
                DoPathEffect(sender, args.DrawingSession);
            else 
                DoEffect(args.DrawingSession, sz, (float)GlowAmount, GlowColor, ((SolidColorBrush)GlowFill).Color, ExpandAmount);
            
        }
        

        private void DoEffect(CanvasDrawingSession ds, Size size, float amount, Windows.UI.Color glowColor, Windows.UI.Color fillColor, double expandAmount)
        {
            var offset = (float)expandAmount / 2;
            //using (var textLayout = CreateTextLayout(ds, size))
            using (var cl = new CanvasCommandList(ds))
            {
                using (var clds = cl.CreateDrawingSession())
                {
                    clds.FillRectangle(0, 0, (float)size.Width, (float)size.Height, glowColor);
                }

                glowEffectGraph.Setup(cl, amount);
                ds.DrawImage(glowEffectGraph.Output, offset, offset);
                ds.FillRectangle(offset, offset, (float)size.Width, (float)size.Height, fillColor);
            }
        }


        private void DoPathEffect(CanvasControl sender, CanvasDrawingSession ds ) {

            var sz = new Size(ParentWidth, ParentHeight);

            using (var thBuilder = new Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder(sender))
            {
                var pthConverter = new PathToD2DPathGeometryConverter();

                foreach(var path in _paths)
                {
                    var offset = (float)ExpandAmount / 2;
                    //using (var textLayout = CreateTextLayout(ds, size))
                    using (var cl = new CanvasCommandList(ds))
                    using (var pthGeo = pthConverter.parse(path, thBuilder))
                    {
                        using (var clds = cl.CreateDrawingSession())
                        {
                            clds.FillGeometry(pthGeo,0,0, GlowColor);
                        }

                        glowEffectGraph.Setup(cl, (float)GlowAmount);
                        ds.DrawImage(glowEffectGraph.Output, offset, offset);
                        ds.FillGeometry(pthGeo,offset, offset, ((SolidColorBrush)GlowFill).Color);
                    }
                    
                }

            }
        }



        public double GlowAmount
        {
            get { return (double)GetValue(GlowAmountProperty); }
            set { SetValue(GlowAmountProperty, value); }
        }
        
        public bool ShowGlowArea
        {
            get { return (bool)GetValue(ShowGlowAreaProperty); }
            set { SetValue(ShowGlowAreaProperty, value); }
        }
        
        public Color GlowColor
        {
            get { return (Windows.UI.Color)GetValue(GlowColorProperty); }
            set { SetValue(GlowColorProperty, value); }
        }

        public Brush GlowFill
        {
            get { return (Brush)GetValue(GlowFillProperty); }
            set { SetValue(GlowFillProperty, value); }
        }






        public static readonly DependencyProperty GlowAmountProperty =
    DependencyProperty.Register("GlowAmount", typeof(double), typeof(EffectLayer), new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty GlowColorProperty =
            DependencyProperty.Register("GlowColor", typeof(Color), typeof(EffectLayer), new PropertyMetadata(Colors.Black, OnPropertyChanged));
        
        public static readonly DependencyProperty ShowGlowAreaProperty =
            DependencyProperty.Register("ShowGlowArea", typeof(bool), typeof(EffectLayer), new PropertyMetadata(false, OnPropertyChanged));
        
        public static readonly DependencyProperty GlowFillProperty =
            DependencyProperty.Register("GlowFill", typeof(Brush), typeof(EffectLayer), new PropertyMetadata(Colors.Black, OnPropertyChanged));





        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as EffectLayer;
            if (d == null)
                return;

            if (instance.canvas != null)
            {
                instance.canvas.Invalidate();
                instance.InvalidateMeasure();
            }
        }

        



    }
}
