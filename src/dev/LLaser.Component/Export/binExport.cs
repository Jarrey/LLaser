//********************************************************* 
// LLaser.Component project - binExport.cs
// Created at 2013-5-20
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using LLaser.Common.Core;

namespace LLaser.Component.Export
{
    public class binExport : IExport
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
                using (var ms = new MemoryStream())
                {
                    var x = new XmlSerializer(typeof (T));
                    x.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    var buffer = new byte[ms.Length];
                    ms.Read(buffer, 0, (int) ms.Length);

                    string outputContent = Convert.ToBase64String(buffer);
                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, outputContent);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}