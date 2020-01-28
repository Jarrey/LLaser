//********************************************************* 
// LLaser project - GradationStepModel.cs
// Created at 2013-5-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

namespace LLaser.Model
{
    public class GradationStepModel
    {
        public GradationStepModel(int step)
        {
            Step = step;
        }
        public int Step { get; private set; }
    }
}
