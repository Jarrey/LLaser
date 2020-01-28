//********************************************************* 
// LLaser.Component project - pcfgImport.cs
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

namespace LLaser.Component.Import
{
    public class xmlImport : IImport
    {
        public bool Import<T>(string filePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Import<T>(fs);
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

        public bool Import<T>(Stream stream)
        {
            try
            {
                var x = new XmlSerializer(typeof (T));
                Target = x.Deserialize(stream);
                stream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public object Target { get; set; }
    }
}