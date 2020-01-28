//********************************************************* 
// LLaser.Component project - pcfgExport.cs
// Created at 2013-5-19
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.IO;
using System.Xml.Serialization;
using LLaser.Common.Core;

namespace LLaser.Component.Export
{
    public class xmlExport : IExport
    {
        public bool Export<T>(string filePath, T obj)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                fs.Seek(0, SeekOrigin.Begin);
                Export(fs, obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                    fs = null;
                }
            }
        }

        public bool Export<T>(Stream stream, T obj)
        {
            try
            {
                var x = new XmlSerializer(typeof (T));
                x.Serialize(stream, obj);
                stream.Flush();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}