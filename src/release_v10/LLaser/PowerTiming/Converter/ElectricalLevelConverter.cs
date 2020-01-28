//********************************************************* 
// LLaser project - ElectricalLevelConverter.cs
// Created at 2013-5-18
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LLaser.Component;

namespace LLaser.Converter
{
    public class ElectricalLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ElectricalLevels)
            {
                var level = (ElectricalLevels)value;
                if (parameter!=null && parameter.ToString().ToUpper() == "N")
                    return level != ElectricalLevels.High;
                else
                    return level == ElectricalLevels.High;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                var status = (bool)value;
                if (parameter != null && parameter.ToString().ToUpper() == "N")
                    return status ? ElectricalLevels.Low : ElectricalLevels.High;
                else
                    return status ? ElectricalLevels.High : ElectricalLevels.Low;
            }
            return ElectricalLevels.Low;
        }
    }
}
