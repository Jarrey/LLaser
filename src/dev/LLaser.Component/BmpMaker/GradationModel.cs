//********************************************************* 
// LLaser.Component project - GradationModel.cs
// Created at 2013-6-17
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace LLaser.Component
{
    using System.Windows.Input;

    using LLaser.Common.Command;

    [Serializable]
    public class GradationModel : BmpMakerBase
    {
        public enum Directions
        {
            H,      // horizontal
            V,      // vertical
            LT2RB,  // left top to right bottom
            RB2LT,  // right bottom to left top
            CH,     // center in horizontal
            CV,     // center in vertical
            C2E,    // center to edge
            R       // radiation
        }

        #region fields and properties

        private int _start;
        private int _end = 0xff;
        private int _step = 256;
        private bool _isGradationEnabled;
        private Directions _direction;
        private bool _isGrayStep = true;

        public int Start
        {
            get { return _start; }
            set { SetBmpProperty(ref _start, value); }
        }

        public int End
        {
            get { return _end; }
            set { SetBmpProperty(ref _end, value); }
        }

        public int Step
        {
            get { return _step; }
            set { SetBmpProperty(ref _step, value); }
        }

        public Directions Direction
        {
            get { return _direction; }
            set { SetBmpProperty(ref _direction, value); }
        }

        public bool IsGradationEnabled
        {
            get { return _isGradationEnabled; }
            set { SetBmpProperty(ref _isGradationEnabled, value); }
        }

        public bool IsGrayStep
        {
            get { return _isGrayStep; }
            set { SetBmpProperty(ref _isGrayStep, value); }
        }

        public bool IsSGrayStep
        {
            get { return !_isGrayStep; }
            set { SetBmpProperty(ref _isGrayStep, !value); }
        }

        #endregion

        #region methods

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            if (IsGradationEnabled)
            {
                double width = bmp.PixelWidth;
                double height = IsSGrayStep ? bmp.PixelHeight / 3.0 : bmp.PixelHeight;
                bmp.ForEach((x, y, color) =>
                    {
                        #region for S gray step
                        if (IsSGrayStep)
                        {
                            if (y <= height)
                            {
                                double r = (double)Start / 0xff; // S gray top ratio
                                return Color.FromArgb(0xff, (byte)(color.R * r), (byte)(color.G * r), (byte)(color.B * r));
                            }
                            if (y > height * 2 && y <= height * 3)
                            {
                                double r = (double)End / 0xff; // S gray bottom ratio
                                return Color.FromArgb(0xff, (byte)(color.R * r), (byte)(color.G * r), (byte)(color.B * r));
                            }
                            y = (IsSGrayStep ? (int)(y - height) : y);
                        }
                        #endregion

                        #region H and V
                        if (Direction == Directions.H || Direction == Directions.V)
                        {
                            double cooridinate = Direction == Directions.H ? x : y;
                            double baseValue = Direction == Directions.H ? width : height;
                            double s = CalcGrayStep(cooridinate, baseValue); // step
                            double r = CalcGrayStepRatio(s); // ratio
                            return Color.FromArgb(0xff, (byte)(color.R * r), (byte)(color.G * r), (byte)(color.B * r));
                        }
                        #endregion
                        #region LT -> RB and RB -> LT
                        else if (Direction == Directions.LT2RB || Direction == Directions.RB2LT)
                        {
                            // calculate center point in current diagonal range
                            double a = -1 * height / width;
                            double Y = (y - x * a) * .5;
                            double X = (x - y / a) * .5;
                            double cooridinate = Math.Sqrt(X * X + Y * Y);
                            double baseValue = Math.Sqrt(height * height + width * width);

                            double s = CalcGrayStep(cooridinate, baseValue); // step
                            double r = CalcGrayStepRatio(Direction == Directions.RB2LT ? 1.0 - s : s); // ratio

                            return Color.FromArgb(0xff, (byte)(color.R * r), (byte)(color.G * r), (byte)(color.B * r));
                        }
                        #endregion
                        #region CH and CV
                        else if (Direction == Directions.CH || Direction == Directions.CV)
                        {
                            double baseValue = Direction == Directions.CH ? width * .5 : height * .5;
                            double cooridinate = Direction == Directions.CH ? (x >= baseValue ? x - baseValue : x) : (y >= baseValue ? y - baseValue : y);

                            double s = CalcGrayStep(cooridinate, baseValue); // step

                            double r = 1.0;
                            if (Direction == Directions.CH)
                            {
                                r = CalcGrayStepRatio(x < baseValue ? 1.0 - s : s); // ratio
                            }
                            else
                            {
                                r = CalcGrayStepRatio(y < baseValue ? 1.0 - s : s); // ratio
                            }

                            return Color.FromArgb(0xff, (byte)(color.R * r), (byte)(color.G * r), (byte)(color.B * r));
                        }
                        #endregion
                        #region C2E
                        else if (Direction == Directions.C2E)
                        {
                            double hw = width * .5;
                            double hh = height * .5;
                            double s1 = CalcGrayStep(x > hw ? x - hw : x, hw); // step
                            double s2 = CalcGrayStep(y > hh ? y - hh : y, hh); // step

                            double r = 1.0;
                            if (x >= hw && y >= hh)
                                r = CalcGrayStepRatio(Math.Max(s1, s2));
                            if (x >= hw && y <= hh)
                                r = CalcGrayStepRatio(Math.Max(s1, 1 - s2));
                            if (x <= hw && y >= hh)
                                r = CalcGrayStepRatio(1 - Math.Min(s1, 1 - s2));
                            if (x <= hw && y <= hh)
                                r = CalcGrayStepRatio(1 - Math.Min(s1, s2));

                            return Color.FromArgb(0xff, (byte)(color.R * r), (byte)(color.G * r), (byte)(color.B * r));
                        }
                        #endregion
                        #region R
                        else if (Direction == Directions.R)
                        {
                            double hw = width * .5;
                            double hh = height * .5;
                            double s1 = CalcGrayStep(x > hw ? x - hw : x, hw); // step
                            double s2 = CalcGrayStep(y > hh ? y - hh : y, hh); // step

                            double r = 0;
                            if (x >= hw && y >= hh)
                                r = CalcGrayStepRatio(Math.Sqrt((s1 * s1 + s2 * s2) * .5));
                            if (x >= hw && y <= hh)
                                r = CalcGrayStepRatio(Math.Sqrt((s1 * s1 + (1 - s2) * (1 - s2)) * .5));
                            if (x <= hw && y >= hh)
                                r = CalcGrayStepRatio(Math.Sqrt(((1 - s1) * (1 - s1) + s2 * s2) * .5));
                            if (x <= hw && y <= hh)
                                r = CalcGrayStepRatio(Math.Sqrt(((1 - s1) * (1 - s1) + (1 - s2) * (1 - s2)) * .5));

                            return Color.FromArgb(0xff, (byte)(color.R * r), (byte)(color.G * r), (byte)(color.B * r));
                        }
                        #endregion

                        return color;
                    });
            }

        }

        private double CalcGrayStep(double cooridinate, double baseValue)
        {
            for (int i = 0; i < Step; i++)
            {
                if (cooridinate <= (i + 1) * baseValue / Step && cooridinate >= i * baseValue / Step)
                {
                    return (double)i / (Step - 1);
                }
            }
            return 0.0;
        }

        private double CalcGrayStepRatio(double step)
        {
            return (Start + step * (End - Start)) / 255; // ratio
        }

        public void Copy(GradationModel target)
        {
            this.Start = target.Start;
            this.End = target.End;
            this.Step = target.Step;
            this.Direction = target.Direction;
            this.IsGradationEnabled = target.IsGradationEnabled;
            this.IsGrayStep = target.IsGrayStep;
            this.IsSGrayStep = target.IsSGrayStep;
        }

        #endregion

        #region command

        public ICommand SwapStartEndCommand
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        int temp = Start;
                        Start = End;
                        End = temp;
                    });
            }
        }

        #endregion
    }
}
