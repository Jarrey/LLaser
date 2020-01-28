//********************************************************* 
// LLaser.Component project - csvExport.cs
// Created at 2013-5-20
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLaser.Common.Core;

namespace LLaser.Component.Export
{
    public class csvExport : IExport
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

        public bool Export<T>(System.IO.Stream stream, T obj)
        {
            try
            {
                var models = obj as IEnumerable<PowerTiming>;
                if (models != null)
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.AutoFlush = true;

                        foreach (var powerTiming in models)
                        {
                            // Write the header
                            writer.WriteLine(string.Concat(powerTiming.Name, ",", PowerStatus.On, ",,", powerTiming.Name, ",", PowerStatus.Off));

                            // Export each entity separately
                            for (int i = 0; i < Math.Max(powerTiming.OnSignals.Count, powerTiming.OffSignals.Count); i++)
                            {
                                string outputLine = string.Empty;
                                if (i < powerTiming.OnSignals.Count)
                                    outputLine = string.Concat(powerTiming.OnSignals[i].Time, ",", powerTiming.OnSignals[i].ElectricalLevel, ",,");
                                else
                                    outputLine = ",,,";

                                if (i < powerTiming.OffSignals.Count)
                                    outputLine += string.Concat(powerTiming.OffSignals[i].Time, ",", powerTiming.OffSignals[i].ElectricalLevel);
                                else
                                    outputLine += ",";
                                writer.WriteLine(outputLine);
                            }
                        }
                    }
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
