//********************************************************* 
// LLaser project - BmpMakerWindowTypeModel.cs
// Created at 2013-6-23
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Component;

namespace LLaser.Model
{
    public class WindowTypeModel
    {
        public WindowTypeModel(string display, WindowModel.WindowTypes type)
        {
            Display = display;
            WindowType = type;
        }

        public string Display { get; private set; }
        public WindowModel.WindowTypes WindowType { get; private set; }
    }
}
