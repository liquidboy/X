using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class Arc : UserControl
    {

        public int ParentMenuId { get; set; }
        public int MenuId { get; set; }


        public double AngleStart { get; set; }

        public Arc(double width, double height )
        {
            this.InitializeComponent();

            this.Width = width;
            this.Height = height;
        }



        public void BuildArcTowardsCenter(int diameter, int arcThickness, Windows.UI.Color color, int gap, double angleStart, double angleEnd, bool showArrow = true)
        {
            AngleStart = angleStart;

            BuildArc(
                    (diameter / 2) - (arcThickness / 2) - gap,
                    arcThickness,
                    color,
                    angleStart,
                    angleEnd
                    );

            sbShowArc.Begin();

            if (!showArrow) grdTriangle.Visibility = Visibility.Collapsed;
            else grdTriangle.Visibility = Visibility.Visible;
        }

        public void BuildArcAwayFromCenter(int diameter, int arcThickness, Windows.UI.Color color, double angleStart, double angleEnd, bool showArrow = true)
        {
            AngleStart = angleStart;

            BuildArc(
                    (diameter / 2) + (arcThickness / 2),
                    arcThickness,
                    color,
                    angleStart,
                    angleEnd
                    );
            sbShowArc.Begin();

            if (!showArrow) grdTriangle.Visibility = Visibility.Collapsed;
            else grdTriangle.Visibility = Visibility.Visible;
        }


        private void BuildArc(int radius, int arcThickness, Windows.UI.Color color, double angleStart, double angleEnd)
        {
            double angleChange = 44;
            

            Point startPoint = CalculateCartesianPoint(angleStart, radius);
            Point endPoint = CalculateCartesianPoint(angleEnd, radius);


            // Create an ArcSegment to define the geometry of the path.
            // The Size property of this segment is animated.
            ArcSegment myArcSegment = new ArcSegment();
            myArcSegment.Size = new Size(radius, radius);
            myArcSegment.SweepDirection = SweepDirection.Clockwise;
            myArcSegment.Point = endPoint;
            myArcSegment.IsLargeArc = false;


            PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
            myPathSegmentCollection.Add(myArcSegment);

            // Create a PathFigure to be used for the PathGeometry of myPath.
            PathFigure myPathFigure = new PathFigure();
            myPathFigure.StartPoint = startPoint;
            myPathFigure.Segments = myPathSegmentCollection;

            PathFigureCollection myPathFigureCollection = new PathFigureCollection();
            myPathFigureCollection.Add(myPathFigure);

            PathGeometry myPathGeometry = new PathGeometry();
            myPathGeometry.Figures = myPathFigureCollection;

            // Create a path to draw a geometry with.
            Windows.UI.Xaml.Shapes.Path myPath = new Windows.UI.Xaml.Shapes.Path();
            myPath.Stroke = new SolidColorBrush(color);
            myPath.StrokeThickness = arcThickness;

            // specify the shape of the path using the path geometry.
            myPath.Data = myPathGeometry;

            Canvas containerCanvas = new Canvas();
            containerCanvas.Children.Add(myPath);
            containerCanvas.PointerPressed += (o,a) => {

                sbPressButton.Begin();


                Messenger.Default.Send(new ArcMenuItemSelectedMessage(this.MenuId.ToString()) { Identifier = "AMS" });

            };

            layoutRoot.Children.Add(containerCanvas);

            ((CompositeTransform)pthTriangle.RenderTransform).TranslateX = radius;
            //((CompositeTransform)pthTriangle.RenderTransform).TranslateY = radius;
            ((CompositeTransform)grdTriangle.RenderTransform).Rotation = angleStart +  (angleChange/2);
        }

        private Point CalculateCartesianPoint(double degreesFromX, double radius)
        {
            //l = rθ where θ is the angle subtended by the arc at the centre in radians 
            //and r is the measure of the radius of circle
            //calculation : angle_radians = angle_degrees * PI / 180 
            double angleRadians = degreesFromX * Math.PI / 180;
            double lengthOfArc = radius * angleRadians;


            //The parametric form of any point on a circle of radius r and centre (h,k) is 
            //given by (h + rcosθ, k + rsinθ), where θ is the angle of the radius line 
            //through the point whose slope is tanθ.
            double h = this.Width / 2, k = this.Height / 2;
            Point ret = new Point(
                h + (radius * Math.Cos(angleRadians)),
                k + (radius * Math.Sin(angleRadians))
                );

            return ret;
        }


        public void Unload()
        {
            sbHideArc.Begin();
        }

    }
}
