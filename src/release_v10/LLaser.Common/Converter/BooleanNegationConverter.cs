//********************************************************* 
// LLaser.Common project - BooleanNegationConverter.cs
// Created at 2013-5-18
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Windows.Data;

namespace LLaser.Common.Converter
{
    /// <summary>
    ///     Value converter that translates true to false and vice versa.
    /// </summary>
    public sealed class BooleanNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(value is bool && (bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(value is bool && (bool)value);
        }
    }
}