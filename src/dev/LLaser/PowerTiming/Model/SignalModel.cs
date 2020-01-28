//********************************************************* 
// LLaser project - SignalModel.cs
// Created at 2013-5-9
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Common;

namespace LLaser.Model
{
    public class SignalModel : ModelBase
    {
        public SignalModel(uint time, int value)
        {
            Time = time;
            Value = value;
        }

        private int _value;
        private uint _time;

        public uint Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        public int Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}
