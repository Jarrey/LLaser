//********************************************************* 
// LLaser.Common project - IImport.cs
// Created at 2013-5-19
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.IO;

namespace LLaser.Common.Core
{
    public interface IImport
    {
        bool Import<T>(string filePath);
        bool Import<T>(Stream stream);
        object Target { get; set; }
    }
}