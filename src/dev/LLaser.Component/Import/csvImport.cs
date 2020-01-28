//********************************************************* 
// LLaser.Component project - csvImport.cs
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
using LLaser.Common.Extension;

namespace LLaser.Component.Import
{
    public class csvImport : IImport
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
                var models = new List<PowerTiming>();
                PowerTiming model = null;
                int onIndex = 0, offIndex = 0;

                using (var reader = new StreamReader(stream))
                {
                    string inputLine = reader.ReadLine();
                    while (!string.IsNullOrEmpty(inputLine))
                    {
                        string[] values = inputLine.Split(',');
                        if (values.Length > 4)
                        {
                            if ((values[1].ToUpper() == "ON" && values[4].ToUpper() == "OFF")
                                || (values[1].ToUpper() == "OFF" && values[4].ToUpper() == "ON"))
                            {
                                // read model name
                                model = new PowerTiming(values[0].ToUpper());
                                models.Add(model);

                                onIndex = values[1].ToUpper() == "ON" ? 0 : 3;
                                offIndex = values[1].ToUpper() == "OFF" ? 0 : 3;
                            }
                            else if (model != null)
                            {
                                // read signals
                                if (!string.IsNullOrEmpty(values[offIndex]) && !string.IsNullOrEmpty(values[offIndex + 1]))
                                    model.OffSignals.Add(new Signal(values[offIndex].StringToDouble(), values[offIndex + 1].ToUpper().Trim() == "HIGH" ? ElectricalLevels.High : ElectricalLevels.Low));
                                if (!string.IsNullOrEmpty(values[onIndex]) && !string.IsNullOrEmpty(values[onIndex + 1]))
                                    model.OnSignals.Add(new Signal(values[onIndex].StringToDouble(), values[onIndex + 1].ToUpper().Trim() == "HIGH" ? ElectricalLevels.High : ElectricalLevels.Low));
                            }
                        }

                        // read next line
                        inputLine = reader.ReadLine();
                    }
                }

                Target = models;
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
