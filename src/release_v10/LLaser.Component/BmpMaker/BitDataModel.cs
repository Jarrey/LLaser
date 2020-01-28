//********************************************************* 
// LLaser.Component project - BitDataModel.cs
// Created at 2013-6-22
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LLaser.Common.Command;

namespace LLaser.Component
{
    using System.Diagnostics.CodeAnalysis;
    using System.Xml.Serialization;

    [Serializable]
    public class BitDataModel : BmpMakerBase
    {
        #region constructor

        public BitDataModel()
        {
            YAxisColors = new ObservableCollection<BitDataColorYAxisModel>();
            SetDefaultColors();
        }

        #endregion

        #region properties and fields

        private int _xAxisCount;
        private int _yAxisCount;
        private Color _lColor;
        private Color _rColor;

        public int XAxisCount
        {
            get { return _xAxisCount; }
            set
            {
                SetBmpProperty(ref _xAxisCount, value);
                foreach (var YAxisColor in YAxisColors)
                {
                    UpdateXAxisColorCount(YAxisColor);
                }
            }
        }

        public int YAxisCount
        {
            get { return _yAxisCount; }
            set
            {
                SetBmpProperty(ref _yAxisCount, value);
                UpdateYAxisColorCount();
            }
        }

        public Color LColor
        {
            get { return _lColor; }
            set
            {
                SetBmpProperty(ref _lColor, value);
            }
        }

        public Color RColor
        {
            get { return _rColor; }
            set
            {
                SetBmpProperty(ref _rColor, value);
            }
        }

        public ObservableCollection<BitDataColorYAxisModel> YAxisColors { get; set; }

        #endregion

        #region methods

        public void SetDefaultColors()
        {
            YAxisColors.Clear();
            XAxisCount = 2;
            YAxisCount = 2;
            LColor = Colors.Black;
            RColor = Colors.White;
        }

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            bmp.ForEach((x, y, color) =>
                {
                    int i = x % XAxisCount;
                    int j = y % YAxisCount;

                    if (YAxisColors.Count > j && YAxisColors[j].XAxisColors.Count > i)
                        return YAxisColors[j].XAxisColors[i].BitColor;
                    else
                        return color;
                });
        }

        private void UpdateYAxisColorCount()
        {
            if (YAxisColors.Count == YAxisCount) return;
            while (YAxisColors.Count > YAxisCount)
                YAxisColors.RemoveAt(YAxisColors.Count - 1);
            while (YAxisColors.Count < YAxisCount)
            {
                var YAxisColorItem = new BitDataColorYAxisModel();
                UpdateXAxisColorCount(YAxisColorItem);
                YAxisColors.Add(YAxisColorItem);
            }
        }

        private void UpdateXAxisColorCount(BitDataColorYAxisModel YAxisColor)
        {
            if (YAxisColor.XAxisColors.Count == XAxisCount) return;
            while (YAxisColor.XAxisColors.Count > XAxisCount)
            {
                var xAxisColor = YAxisColor.XAxisColors.Last();
                xAxisColor.BmpPropertyChanged -= BmpPropertyChangedHanlder;
                YAxisColor.XAxisColors.Remove(xAxisColor);
            }
            while (YAxisColor.XAxisColors.Count < XAxisCount)
            {
                var xAxisColor = new BitDataColorXAxisModel();
                xAxisColor.BmpPropertyChanged -= BmpPropertyChangedHanlder;
                xAxisColor.BmpPropertyChanged += BmpPropertyChangedHanlder;
                YAxisColor.XAxisColors.Add(xAxisColor);

            }
        }

        private void BmpPropertyChangedHanlder(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnBmpPropertyChanged("BitData");
        }

        public void Copy(BitDataModel target)
        {
            base.Copy(target);
            this.LColor = target.LColor;
            this.RColor = target.RColor;
            this.XAxisCount = target.XAxisCount;
            this.YAxisCount = target.YAxisCount;
            for (int i = 0; i < YAxisColors.Count; i++)
            {
                // 2013-6-29, Fix bug 33, Cannot open saved file and display the bitdata BMP
                // deseriliaze this object may generate one couple size of YAxisColors collection 
                this.YAxisColors[i].Copy(target.YAxisColors.Count == this.YAxisColors.Count * 2 ? target.YAxisColors[i + target.YAxisColors.Count / 2] : target.YAxisColors[i]);
            }
        }

        #endregion

        #region nested class

        [Serializable]
        public class BitDataColorYAxisModel : BmpMakerBase
        {
            #region constructor

            public BitDataColorYAxisModel()
            {
                XAxisColors = new ObservableCollection<BitDataColorXAxisModel>();
            }

            #endregion

            #region properties and fields

            public ObservableCollection<BitDataColorXAxisModel> XAxisColors { get; set; }

            #endregion

            public void Copy(BitDataColorYAxisModel target)
            {
                for (int i = 0; i < XAxisColors.Count; i++)
                {
                    XAxisColors[i].Copy(target.XAxisColors[i]);
                }
            }
        }


        [Serializable]
        public class BitDataColorXAxisModel : BmpMakerBase
        {
            #region properties and fields

            private Color _bitColor = Colors.Black;
            public Color BitColor
            {
                get { return _bitColor; }
                set
                {
                    SetBmpProperty(ref _bitColor, value);
                }
            }

            #endregion

            #region commands

            public ICommand LeftClickCommand
            {
                get
                {
                    return new RelayCommand<Color>(p =>
                        {
                            BitColor = p;
                        });
                }
            }

            public ICommand RightClickCommand
            {
                get
                {
                    return new RelayCommand<Color>(p =>
                        {
                            BitColor = p;
                        });
                }
            }

            #endregion

            public void Copy(BitDataColorXAxisModel target)
            {
                this.BitColor = target.BitColor;
            }
        }

        #endregion
    }
}
