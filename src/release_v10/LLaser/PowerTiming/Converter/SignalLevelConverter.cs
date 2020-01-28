//********************************************************* 
// LLaser project - SignalLevelConverter.cs
// Created at 2013-5-17
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
using System.Windows;
using System.Windows.Data;
using LLaser.Component;

namespace LLaser.Converter
{
    public class SignalLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            short id = 0;
            short.TryParse(parameter.ToString(), out id);
            if (value is ElectricalLevels)
            {
                return id * 100 + (((ElectricalLevels)value) == ElectricalLevels.High ? 100 : 50);
            }
            return id * 100 + 50;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
