//********************************************************* 
// LLaser.Component project - SolidColorModel.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LLaser.Component
{
    [Serializable]
    public class SolidColorModel : BmpMakerBase
    {
        #region fields and properties

        private Color _solidColor = Colors.Black;
        public Color SolidColor
        {
            get { return _solidColor; }
            set { SetBmpProperty(ref _solidColor, value); }
        }

        #endregion

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            bmp.FillRectangle(0, 0, bmp.PixelWidth, bmp.PixelHeight, SolidColor);
        }

        public void Copy(SolidColorModel target)
        {
            this.SolidColor = target.SolidColor;
        }
    }
}
