//********************************************************* 
// LLaser project - GradationDirectionModel.cs
// Created at 2013-6-17
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Component;

namespace LLaser.Model
{
    public class GradationDirectionModel
    {
        public GradationDirectionModel(string display, GradationModel.Directions direction)
        {
            Display = display;
            Direction = direction;
        }

        public string Display { get; private set; }
        public GradationModel.Directions Direction { get; private set; }
    }
}
