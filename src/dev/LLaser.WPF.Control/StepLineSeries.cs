//*********************************************************
// LLaser.WPF project - StepLineSeries.cs
// Created at 2013-5-14
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;

namespace LLaser.WPF.Controls
{
    public class StepLineSeries : LineSeries
    {
        /// <summary>
        ///     Gets the collection of points that make up the line.
        /// </summary>
        public new PointCollection Points
        {
            get { return GetValue(PointsProperty) as PointCollection; }
            private set { SetValue(PointsProperty, value); }
        }

        #region dependency properties

        public static readonly DependencyProperty JumpFirstProperty =
            DependencyProperty.Register("JumpFirst", typeof(bool), typeof(StepLineSeries), new PropertyMetadata(true));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(StepLineSeries),
                                        new PropertyMetadata(Brushes.Yellow));

        public static readonly DependencyProperty AraeBackgroundProperty =
            DependencyProperty.Register("AraeBackground", typeof(Brush), typeof(StepLineSeries),
                                        new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(StepLineSeries),
                                        new PropertyMetadata(1.0));


        public static readonly DependencyProperty IsShortDelayProperty =
            DependencyProperty.Register("IsShortDelay", typeof(bool), typeof(StepLineSeries),
                                        new PropertyMetadata(true));

        public static readonly DependencyProperty IsFillXAxisProperty =
            DependencyProperty.Register("IsFillXAxis", typeof(bool), typeof(StepLineSeries),
                                        new PropertyMetadata(true));

        public bool IsFillXAxis
        {
            get { return (bool)GetValue(IsFillXAxisProperty); }
            set { SetValue(IsFillXAxisProperty, value); }
        }

        public bool IsShortDelay
        {
            get { return (bool)GetValue(IsShortDelayProperty); }
            set { SetValue(IsShortDelayProperty, value); }
        }


        public bool JumpFirst
        {
            get { return (bool)GetValue(JumpFirstProperty); }
            set { SetValue(JumpFirstProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public Brush AraeBackground
        {
            get { return (Brush)GetValue(AraeBackgroundProperty); }
            set { SetValue(AraeBackgroundProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        #endregion

        protected override void UpdateShapeFromPoints(IEnumerable<Point> points)
        {
            if (points.Any())
            {
                var pointCollection = new PointCollection();
                foreach (Point point in points)
                {
                    pointCollection.Add(point);
                }
                Points = CreateStepLineSeries(pointCollection);
            }
            else
            {
                Points = null;
            }
        }

        private PointCollection CreateStepLineSeries(PointCollection points)
        {
            var returnPoints = new PointCollection();
            for (int i = 0; i < points.Count; i++)
            {
                int shortDelay = IsShortDelay ? 2 : 0;
                Point currentPoint = points[i];
                returnPoints.Add(new Point(currentPoint.X + shortDelay, currentPoint.Y));
                if (i < points.Count - 1)
                {
                    Point nextPoint = points[i + 1];
                    returnPoints.Add(new Point(JumpFirst ? currentPoint.X - shortDelay : nextPoint.X - shortDelay,
                                               JumpFirst ? nextPoint.Y : currentPoint.Y));
                }
            }
            if (IsFillXAxis && points.Last().X < ActualWidth)
                returnPoints.Add(new Point(ActualWidth, points.Last().Y));
            return returnPoints;
        }
    }
}