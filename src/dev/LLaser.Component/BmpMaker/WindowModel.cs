//********************************************************* 
// LLaser.Component project - WindowModel.cs
// Created at 2013-6-23
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LLaser.Component
{
    [Serializable]
    public class WindowModel : BmpMakerBase
    {
        public enum WindowTypes
        {
            Single,
            Cross,
            Matrix,
            Window2,
            Frame,
            Checker
        }

        public enum WindowBackgroundTypes
        {
            Inside,
            Outside
        }

        #region constructor

        public WindowModel()
        {
            WindowType = WindowTypes.Single;
        }

        #endregion

        #region properties and fields

        private bool _isWindowEnabled = false;
        public bool IsWindowEnabled
        {
            get { return _isWindowEnabled; }
            set { SetBmpProperty(ref _isWindowEnabled, value); }
        }

        private WindowTypes _windowType;
        public WindowTypes WindowType
        {
            get { return _windowType; }
            set
            {
                IsPoint1Enabled = false;
                IsPoint2Enabled = false;
                IsLengthEnabled = false;
                IsSpaceEnabled = false;
                switch (value)
                {
                    case WindowTypes.Single:
                    case WindowTypes.Frame:
                        IsPoint1Enabled = true;
                        IsPoint2Enabled = true;
                        break;
                    case WindowTypes.Cross:
                    case WindowTypes.Window2:
                        IsPoint1Enabled = true;
                        break;
                    case WindowTypes.Matrix:
                        IsPoint1Enabled = true;
                        IsPoint2Enabled = true;
                        IsLengthEnabled = true;
                        IsSpaceEnabled = true;
                        break;
                    case WindowTypes.Checker:
                        IsLengthEnabled = true;
                        break;
                }
                SetBmpProperty(ref _windowType, value);
            }
        }

        private WindowBackgroundTypes _windowBackgroundType = WindowBackgroundTypes.Inside;
        public WindowBackgroundTypes WindowBackgroundType
        {
            get { return _windowBackgroundType; }
            set { SetBmpProperty(ref _windowBackgroundType, value); }
        }

        private Color _windowColor = Colors.Black;
        public Color WindowColor
        {
            get { return _windowColor; }
            set { SetBmpProperty(ref _windowColor, value); }
        }

        private int _grayStep = 255;
        public int GrayStep
        {
            get { return _grayStep; }
            set { SetBmpProperty(ref _grayStep, value); }
        }

        private int _x1 = 100;
        public int X1
        {
            get { return _x1; }
            set { SetBmpProperty(ref _x1, value); }
        }

        private int _y1 = 100;
        public int Y1
        {
            get { return _y1; }
            set { SetBmpProperty(ref _y1, value); }
        }

        private int _x2 = 200;
        public int X2
        {
            get { return _x2; }
            set { SetBmpProperty(ref _x2, value); }
        }

        private int _y2 = 200;
        public int Y2
        {
            get { return _y2; }
            set { SetBmpProperty(ref _y2, value); }
        }

        private int _hLength = 1;
        public int HLength
        {
            get { return _hLength; }
            set { SetBmpProperty(ref _hLength, value); }
        }

        private int _vLength = 1;
        public int VLength
        {
            get { return _vLength; }
            set { SetBmpProperty(ref _vLength, value); }
        }

        private int _hSpace;
        public int HSpace
        {
            get { return _hSpace; }
            set { SetBmpProperty(ref _hSpace, value); }
        }

        private int _vSpace;
        public int VSpace
        {
            get { return _vSpace; }
            set { SetBmpProperty(ref _vSpace, value); }
        }

        private bool _isPoint1Enabled;
        public bool IsPoint1Enabled
        {
            get { return _isPoint1Enabled; }
            set { SetProperty(ref _isPoint1Enabled, value); }
        }

        private bool _isPoint2Enabled;
        public bool IsPoint2Enabled
        {
            get { return _isPoint2Enabled; }
            set { SetProperty(ref _isPoint2Enabled, value); }
        }

        private bool _isLengthEnabled;
        public bool IsLengthEnabled
        {
            get { return _isLengthEnabled; }
            set { SetProperty(ref _isLengthEnabled, value); }
        }

        private bool _isSpaceEnabled;
        public bool IsSpaceEnabled
        {
            get { return _isSpaceEnabled; }
            set { SetProperty(ref _isSpaceEnabled, value); }
        }

        #endregion

        #region methods

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            if (!IsWindowEnabled) return;

            double r = GrayStep / 255.0;
            Color windowColor = Color.FromArgb(0xff, (byte)(WindowColor.R * r), (byte)(WindowColor.G * r), (byte)(WindowColor.B * r));
            bmp.ForEach((x, y, color) =>
                {
                    switch (WindowType)
                    {
                        case WindowTypes.Single:
                            if (X2 > X1 && Y2 > Y1 &&
                                x >= X1 && x <= X2 && y >= Y1 && y <= Y2) return WindowBackgroundType == WindowBackgroundTypes.Inside ? windowColor : color;
                            else return WindowBackgroundType == WindowBackgroundTypes.Inside ? color : windowColor;
                        case WindowTypes.Cross:
                            if (x == X1 || y == Y1) return WindowBackgroundType == WindowBackgroundTypes.Inside ? windowColor : color;
                            else return WindowBackgroundType == WindowBackgroundTypes.Inside ? color : windowColor;
                        case WindowTypes.Window2:
                            if ((x <= X1 && y <= Y1) ||
                                (x > X1 && y > Y1)) return WindowBackgroundType == WindowBackgroundTypes.Inside ? windowColor : color;
                            else return WindowBackgroundType == WindowBackgroundTypes.Inside ? color : windowColor;
                        case WindowTypes.Frame:
                            if (X2 > X1 && Y2 > Y1 &&
                                x >= X1 && x <= X2 && y >= Y1 && y <= Y2 &&
                               (x == X1 || x == X2 || y == Y1 || y == Y2)) return WindowBackgroundType == WindowBackgroundTypes.Inside ? windowColor : color;
                            else return WindowBackgroundType == WindowBackgroundTypes.Inside ? color : windowColor;
                        case WindowTypes.Checker:
                            int checkerInnerX = x % (2 * HLength);
                            int checkerInnerY = y % (2 * VLength);
                            if ((checkerInnerX < HLength && checkerInnerY < VLength) ||
                                (checkerInnerX >= HLength && checkerInnerY >= VLength)) 
                                return WindowBackgroundType == WindowBackgroundTypes.Inside ? windowColor : color;
                            else return WindowBackgroundType == WindowBackgroundTypes.Inside ? color : windowColor;
                        case WindowTypes.Matrix:
                            if (X2 > X1 && Y2 > Y1 &&
                                x >= X1 && x <= X2 && y >= Y1 && y <= Y2)
                            {
                                int matrixInnerX = x % (HSpace + HLength);
                                int matrixInnerY = y % (VSpace + VLength);
                                if (matrixInnerX < HLength && matrixInnerY < VLength) 
                                    return WindowBackgroundType == WindowBackgroundTypes.Inside ? windowColor : color;
                                else return WindowBackgroundType == WindowBackgroundTypes.Inside ? color : windowColor;
                            }
                            else return WindowBackgroundType == WindowBackgroundTypes.Inside ? color : windowColor;
                    }
                    return color;
                });
        }

        public void Copy(WindowModel target)
        {
            this.IsWindowEnabled = target.IsWindowEnabled;
            this.WindowType = target.WindowType;
            this.GrayStep = target.GrayStep;
            this.WindowColor = target.WindowColor;
            this.WindowBackgroundType = target.WindowBackgroundType;
            this.X1 = target.X1;
            this.X2 = target.X2;
            this.Y1 = target.Y1;
            this.Y2 = target.Y2;
            this.HLength = target.HLength;
            this.VLength = target.VLength;
            this.HSpace = target.HSpace;
            this.VSpace = target.VSpace;
            this.IsPoint1Enabled = target.IsPoint1Enabled;
            this.IsPoint2Enabled = target.IsPoint2Enabled;
            this.IsLengthEnabled = target.IsLengthEnabled;
            this.IsSpaceEnabled = target.IsSpaceEnabled;
        }

        #endregion
    }
}
