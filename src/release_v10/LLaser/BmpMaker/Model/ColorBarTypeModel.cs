//********************************************************* 
// LLaser project - ColorBarTypeModel.cs
// Created at 2013-5-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Component;

namespace LLaser.Model
{
    public class ColorBarTypeModel
    {
        public ColorBarTypeModel(string display, ColorBarModel.ColorBarTypes type)
        {
            Display = display;
            ColorBarType = type;
        }

        public string Display { get; private set; }
        public ColorBarModel.ColorBarTypes ColorBarType { get; private set; }
    }
}
