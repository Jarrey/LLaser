//********************************************************* 
// LLaser.Component project - IBmpExecutor.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Windows.Media.Imaging;

namespace LLaser.Component
{
    public interface IBmpExecutor
    {
        void ExecuteEffect(ref WriteableBitmap bmp);
    }
}
