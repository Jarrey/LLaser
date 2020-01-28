//********************************************************* 
// LLaser.Component project - SizeModel.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Windows.Media.Imaging;

namespace LLaser.Component
{
    [Serializable]
    public class SizeModel : BmpMakerBase
    {
        #region fields and properties

        private int _width = 1024;
        private int _height = 768;

        public int Width
        {
            get { return _width; }
            set { SetBmpProperty(ref _width, value); }
        }

        public int Height
        {
            get { return _height; }
            set { SetBmpProperty(ref _height, value); }
        }

        #endregion

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            if (bmp.PixelWidth != Width || bmp.PixelHeight != Height)
                bmp = BitmapFactory.New(Width, Height);
        }

        public void Copy(SizeModel target)
        {
            this.Width = target.Width;
            this.Height = target.Height;
        }
    }
}
