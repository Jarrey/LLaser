//********************************************************* 
// LLaser.Component project - SignalExistingException.cs
// Created at 2013-5-11
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;

namespace LLaser.Component
{
    public class SignalExistingException : Exception
    {
        public SignalExistingException(double time) :
            base(String.Format(@"The single (time = {0}) has existed in list.", time))
        {
            Time = time;
        }

        public double Time { get; private set; }
    }
}