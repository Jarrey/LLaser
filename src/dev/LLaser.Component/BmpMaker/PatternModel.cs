//********************************************************* 
// LLaser.Component project - PatternModel.cs
// Created at 2013-6-21
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
    public class PatternModel : BmpMakerBase
    {
        public enum PatternTypes
        {
            Black,          // black background without border
            White,          // white background without border
            BlackWBorder,   // black background with border
            WhiteWBorder    // white background with border
        }

        #region Fields

        private PatternTypes _patternType = PatternTypes.WhiteWBorder;

        #endregion

        #region Properties

        public PatternTypes PatternType
        {
            get { return _patternType; }
            set { SetBmpProperty(ref _patternType, value); }
        }

        #endregion

        #region methods

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            int width = bmp.PixelWidth;
            int height = bmp.PixelHeight;
            Color bg = Colors.Black;
            Color fg = Colors.White;
            bool hasBorder = true;

            switch (PatternType)
            {
                case PatternTypes.WhiteWBorder:
                    bg = Colors.White;
                    fg = Colors.Black;
                    hasBorder = true;
                    break;
                case PatternTypes.BlackWBorder:
                    bg = Colors.Black;
                    fg = Colors.White;
                    hasBorder = true;
                    break;
                case PatternTypes.Black:
                    bg = Colors.Black;
                    fg = Colors.White;
                    hasBorder = false;
                    break;
                case PatternTypes.White:
                    bg = Colors.White;
                    fg = Colors.Black;
                    hasBorder = false;
                    break;
            }
            int border = hasBorder ? 1 : 0;

            bmp.FillRectangle(border, border, width - border, height - border, bg);
            if (hasBorder) bmp.DrawRectangle(0, 0, width, height, fg);
            bmp.DrawLine(0, 0, width, height, fg);
            bmp.DrawLine(width, 0, 0, height, fg);
        }

        public void Copy(PatternModel target)
        {
            this.PatternType = target.PatternType;
        }

        #endregion
    }
}
