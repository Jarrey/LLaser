//********************************************************* 
// LLaser project - PatternTypeModel.cs
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
using LLaser.Component;

namespace LLaser.Model
{
    public class PatternTypeModel
    {
        public PatternTypeModel(string display, PatternModel.PatternTypes type)
        {
            Display = display;
            Type = type;
        }

        public string Display { get; set; }
        public PatternModel.PatternTypes Type { get; set; }
    }
}
