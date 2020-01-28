//********************************************************* 
// LLaser.Component project - CommonEnums.cs
// Created at 2013-5-9
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

namespace LLaser.Component
{
    /// <summary>
    ///     LLaser function types
    /// </summary>
    public enum FunctionTypes
    {
        TFTTiming,
        PowerTiming,
        BmpMaker
    }

    /// <summary>
    /// The power status.
    /// </summary>
    public enum PowerStatus
    {
        On,
        Off
    }

    public enum ElectricalLevels : uint
    {
        High = 1U,
        Low = 0U
    }

    public enum Polarities : uint
    {
        High2Low = 1, // High to Low
        Low2High = 2, // Low to High
        HighEnable = 3, // High enable
        LowEnable = 4, // Low enable
        High = 5, // Long High
        Low = 6 // Long Low
    }
}