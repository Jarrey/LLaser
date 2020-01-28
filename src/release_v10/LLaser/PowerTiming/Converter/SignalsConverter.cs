//********************************************************* 
// LLaser project - SignalsConverter.cs
// Created at 2013-5-17
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using LLaser.Component;
using LLaser.WPF.Controls;

namespace LLaser.Converter
{
    public class SignalsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<Signal> signals = null;
            if (value is ObservableCollection<Signal>)
            {
                signals = new ObservableCollection<Signal>((value as ObservableCollection<Signal>).OrderBy(p => p.Time));
                if (signals.Count == 0 || signals.First().Time != 0.0)
                    signals.Insert(0, new Signal(0, ElectricalLevels.Low));
                signals.Add(new Signal(signals.Last().Time * 1.1, signals.Last().ElectricalLevel));
            }
            return signals;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
