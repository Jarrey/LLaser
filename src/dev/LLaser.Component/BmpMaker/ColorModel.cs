//********************************************************* 
// LLaser.Component project - ColorModel.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LLaser.Component
{
    [Serializable]
    public class ColorModel : BmpMakerBase
    {
        #region constructor

        public ColorModel()
        {
            SolidColor = new SolidColorModel();
            ColorBar = new ColorBarModel();
            Pattern = new PatternModel();
            BitData = new BitDataModel();

            #region register events

            SolidColor.BmpPropertyChanged -= BmpPropertyChangedHanlder;
            ColorBar.BmpPropertyChanged -= BmpPropertyChangedHanlder;
            Pattern.BmpPropertyChanged -= BmpPropertyChangedHanlder;
            BitData.BmpPropertyChanged -= BmpPropertyChangedHanlder;

            SolidColor.BmpPropertyChanged += BmpPropertyChangedHanlder;
            ColorBar.BmpPropertyChanged += BmpPropertyChangedHanlder;
            Pattern.BmpPropertyChanged += BmpPropertyChangedHanlder;
            BitData.BmpPropertyChanged += BmpPropertyChangedHanlder;

            #endregion
        }

        #endregion

        #region Fields

        private bool _isSolidColorEnabled = true;
        private bool _isColorBarEnabled = false;
        private bool _isPatternEnabled = false;
        private bool _isBitDataEnabled = false;
        private SolidColorModel _solidColor;
        private ColorBarModel _colorBar;
        private PatternModel _pattern;
        private BitDataModel _bitData;

        #endregion

        #region Properties

        public bool IsSolidColorEnabled
        {
            get { return _isSolidColorEnabled; }
            set { SetBmpProperty(ref _isSolidColorEnabled, value); }
        }

        public bool IsColorBarEnabled
        {
            get { return _isColorBarEnabled; }
            set { SetBmpProperty(ref _isColorBarEnabled, value); }
        }

        public bool IsPatternEnabled
        {
            get { return _isPatternEnabled; }
            set { SetBmpProperty(ref _isPatternEnabled, value); }
        }

        public bool IsBitDataEnabled
        {
            get { return _isBitDataEnabled; }
            set { SetBmpProperty(ref _isBitDataEnabled, value); }
        }

        public SolidColorModel SolidColor
        {
            get { return _solidColor; }
            set { SetBmpProperty(ref _solidColor, value); }
        }

        public ColorBarModel ColorBar
        {
            get { return _colorBar; }
            set { SetBmpProperty(ref _colorBar, value); }
        }

        public PatternModel Pattern
        {
            get { return _pattern; }
            set { SetBmpProperty(ref _pattern, value); }
        }

        public BitDataModel BitData
        {
            get { return _bitData; }
            set { SetBmpProperty(ref _bitData, value); }
        }

        #endregion

        #region event handlers

        private void BmpPropertyChangedHanlder(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnBmpPropertyChanged("ColorModel");
        }

        #endregion

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            if (IsSolidColorEnabled)
                SolidColor.ExecuteEffect(ref bmp);
            else if (IsColorBarEnabled)
                ColorBar.ExecuteEffect(ref bmp);
            else if (IsPatternEnabled)
                Pattern.ExecuteEffect(ref bmp);
            else if (IsBitDataEnabled)
                BitData.ExecuteEffect(ref bmp);
        }

        public void Copy(ColorModel target)
        {
            this.IsBitDataEnabled = target.IsBitDataEnabled;
            this.IsColorBarEnabled = target.IsColorBarEnabled;
            this.IsPatternEnabled = target.IsPatternEnabled;
            this.IsSolidColorEnabled = target.IsSolidColorEnabled;
            this.Pattern.Copy(target.Pattern);
            this.SolidColor.Copy(target.SolidColor);
            this.BitData.Copy(target.BitData);
            this.ColorBar.Copy(target.ColorBar);
        }
    }
}
