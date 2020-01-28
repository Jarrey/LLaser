//********************************************************* 
// LLaser.Common project - IExport.cs
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
    public interface IExport
    {
        bool Export<T>(string filePath, T obj);
        bool Export<T>(Stream stream, T obj);
    }
}