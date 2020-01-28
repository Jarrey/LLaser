//********************************************************* 
// LLaser.Common project - Lang.cs
// Created at 2013-4-29
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.ObjectModel;

namespace LLaser.Common
{
    /// <summary>
    ///     A collection for language strings
    /// </summary>
    public class Lang : Collection<L>
    {
    }

    /// <summary>
    ///     A class for language string item defined in resources
    /// </summary>
    public class L
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}