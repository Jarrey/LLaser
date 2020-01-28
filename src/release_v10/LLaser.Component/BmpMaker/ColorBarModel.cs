//********************************************************* 
// LLaser.Component project - ColorBarModel.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LLaser.Component
{
    using System.Linq;
    using System.Xml.Serialization;

    [Serializable]
    public class ColorBarModel : BmpMakerBase
    {
        public readonly static int MaxCustomColors = 8;
        public readonly static Color[] CustomDefaultColors = new Color[]
                {
                    Colors.White,
                    Colors.Yellow,
                    Colors.Cyan,
                    Colors.Green,
                    Colors.Magenta,
                    Colors.Red,
                    Colors.Blue,
                    Colors.Black
                };

        public enum ColorBarTypes
        {
            HColor,
            VColor,
            HRGB,
            VRGB,
            HRGBW,
            VRGBW
        }

        #region constructor

        public ColorBarModel()
        {
            SetDefaultCustomColors();
        }

        #endregion

        #region properties and fields

        private ColorBarTypes _colorBarType;
        private int _customColorBarCount = MaxCustomColors;
        private List<CustomColorItem> _customColors;

        public ColorBarTypes ColorBarType
        {
            get { return _colorBarType; }
            set
            {
                SetBmpProperty(ref _colorBarType, value);
                OnPropertyChanged("IsEnableColorSelect");
                OnBmpPropertyChanged("IsEnableColorSelect");
            }
        }

        public int CustomColorBarCount
        {
            get { return _customColorBarCount; }
            set
            {
                SetBmpProperty(ref _customColorBarCount, value);

                if (CustomColors != null)
                {
                    for (int i = 0; i < value; i++)
                    {
                        CustomColors[i].IsEnabled = true;
                    }
                    for (int i = value; i < MaxCustomColors; i++)
                    {
                        CustomColors[i].IsEnabled = false;
                    }
                }
            }
        }

        public List<CustomColorItem> CustomColors
        {
            get { return _customColors; }
            set { SetBmpProperty(ref _customColors, value); }
        }

        [XmlIgnore]
        public bool IsEnableColorSelect
        {
            get
            {
                return ColorBarType == ColorBarTypes.HColor || ColorBarType == ColorBarTypes.VColor;
            }
        }

        #endregion

        #region methods

        public void SetDefaultCustomColors()
        {
            if (CustomColors == null)
            {
                CustomColors = new List<CustomColorItem>();
                for (int i = 0; i < CustomDefaultColors.Length; i++)
                {
                    var customColorItem = new CustomColorItem(i + 1, CustomDefaultColors[i]);
                    customColorItem.BmpPropertyChanged -= BmpPropertyChangedHanlder;
                    customColorItem.BmpPropertyChanged += BmpPropertyChangedHanlder;
                    CustomColors.Add(customColorItem);
                }
            }
            else
            {
                for (int i = 0; i < CustomDefaultColors.Length; i++)
                {
                    CustomColors[i].Color = CustomDefaultColors[i];
                }
            }
            CustomColorBarCount = MaxCustomColors;
        }

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            #region HRGB
            if (ColorBarType == ColorBarTypes.HRGB)
            {
                int rRange = bmp.PixelHeight / 3;
                int gRange = bmp.PixelHeight * 2 / 3;
                int bRange = bmp.PixelHeight;
                bmp.ForEach((x, y) =>
                    {
                        if (y >= 0 && y <= rRange)
                            return Colors.Red;
                        if (y >= rRange && y <= gRange)
                            return Colors.Green;
                        if (y >= gRange && y <= bRange)
                            return Colors.Blue;
                        return Colors.Black;
                    });
            }
            #endregion
            #region VRGB
            else if (ColorBarType == ColorBarTypes.VRGB)
            {
                int rRange = bmp.PixelWidth / 3;
                int gRange = bmp.PixelWidth * 2 / 3;
                int bRange = bmp.PixelWidth;
                bmp.ForEach((x, y) =>
                {
                    if (x >= 0 && x <= rRange)
                        return Colors.Red;
                    if (x >= rRange && x <= gRange)
                        return Colors.Green;
                    if (x >= gRange && x <= bRange)
                        return Colors.Blue;
                    return Colors.Black;
                });
            }
            #endregion
            #region HRGBW
            else if (ColorBarType == ColorBarTypes.HRGBW)
            {
                int rRange = bmp.PixelHeight / 4;
                int gRange = bmp.PixelHeight * 2 / 4;
                int bRange = bmp.PixelHeight * 3 / 4;
                int wRange = bmp.PixelHeight;
                bmp.ForEach((x, y) =>
                {
                    if (y >= 0 && y <= rRange)
                        return Colors.Red;
                    if (y >= rRange && y <= gRange)
                        return Colors.Green;
                    if (y >= gRange && y <= bRange)
                        return Colors.Blue;
                    if (y >= bRange && y <= wRange)
                        return Colors.White;
                    return Colors.Black;
                });
            }
            #endregion
            #region VRGBW
            else if (ColorBarType == ColorBarTypes.VRGBW)
            {
                int rRange = bmp.PixelWidth / 4;
                int gRange = bmp.PixelWidth * 2 / 4;
                int bRange = bmp.PixelWidth * 3 / 4;
                int wRange = bmp.PixelWidth;
                bmp.ForEach((x, y) =>
                {
                    if (x >= 0 && x <= rRange)
                        return Colors.Red;
                    if (x >= rRange && x <= gRange)
                        return Colors.Green;
                    if (x >= gRange && x <= bRange)
                        return Colors.Blue;
                    if (x >= bRange && x <= wRange)
                        return Colors.White;
                    return Colors.Black;
                });
            }
            #endregion
            #region VColor
            else if (ColorBarType == ColorBarTypes.VColor)
            {
                List<int> ranges = new List<int>();
                for (int i = 0; i <= CustomColorBarCount; i++)
                {
                    ranges.Add(bmp.PixelWidth * i / CustomColorBarCount);
                }
                bmp.ForEach((x, y) =>
                {
                    for (int i = 0; i < ranges.Count - 1; i++)
                    {
                        if (x >= ranges[i] && x <= ranges[i + 1])
                            return CustomColors[i].Color;
                    }
                    return Colors.Black;
                });
            }
            #endregion
            #region HColor
            else if (ColorBarType == ColorBarTypes.HColor)
            {
                List<int> ranges = new List<int>();
                for (int i = 0; i <= CustomColorBarCount; i++)
                {
                    ranges.Add(bmp.PixelHeight * i / CustomColorBarCount);
                }
                bmp.ForEach((x, y) =>
                {
                    for (int i = 0; i < ranges.Count - 1; i++)
                    {
                        if (y >= ranges[i] && y <= ranges[i + 1])
                            return CustomColors[i].Color;
                    }
                    return Colors.Black;
                });
            }
            #endregion
        }

        public void Copy(ColorBarModel target)
        {
            this.ColorBarType = target.ColorBarType;
            this.CustomColorBarCount = target.CustomColorBarCount;
            foreach (var c in this.CustomColors)
            {
                foreach (var t in target.CustomColors.Where(t => c.Index == t.Index))
                {
                    c.Copy(t);
                }
            }
        }

        #endregion

        #region event handlers

        private void BmpPropertyChangedHanlder(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnBmpPropertyChanged("ColorBarModel");
        }

        #endregion

        #region nested class

        [Serializable]
        public class CustomColorItem : BmpMakerBase
        {
            public CustomColorItem()
            {
                Index = 0;
                Color = Colors.Black;
                IsEnabled = true;
            }

            public CustomColorItem(int index, Color color)
            {
                Index = index;
                Color = color;
                IsEnabled = true;
            }

            private int _index;
            private Color _color;
            private bool _isEnabled;

            public int Index
            {
                get { return _index; }
                set { _index = value; }
            }

            public Color Color
            {
                get { return _color; }
                set { SetBmpProperty(ref _color, value); }
            }

            public bool IsEnabled
            {
                get { return _isEnabled; }
                set { SetBmpProperty(ref _isEnabled, value); }
            }

            public void Copy(CustomColorItem target)
            {
                this.Color = target.Color;
                this.Index = target.Index;
                this.IsEnabled = target.IsEnabled;
            }
        }

        #endregion
    }
}
