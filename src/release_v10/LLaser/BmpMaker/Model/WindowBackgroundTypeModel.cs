//********************************************************* 
// LLaser project - BmpMakerWindowBackgroundTypeModel.cs
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
    public class WindowBackgroundTypeModel
    {
        public WindowBackgroundTypeModel(string display, WindowModel.WindowBackgroundTypes type)
        {
            Display = display;
            WindowBackgroundType = type;
        }

        public string Display { get; private set; }
        public WindowModel.WindowBackgroundTypes WindowBackgroundType { get; private set; }
    }
}
