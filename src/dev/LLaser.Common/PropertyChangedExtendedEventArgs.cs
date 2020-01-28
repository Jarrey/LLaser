//*********************************************************
// LLaser.WPF.Common project - PropertyChangedExtendedEventArgs.cs
// Created at 2013-5-18
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System.ComponentModel;

namespace LLaser.Common
{
    public class PropertyChangedExtendedEventArgs<T> : PropertyChangedEventArgs
    {
        public virtual T OldValue { get; private set; }
        public virtual T NewValue { get; private set; }

        public PropertyChangedExtendedEventArgs(string propertyName, T oldValue, T newValue)
            : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public delegate bool PropertyChangedExtendedEventHandler<T>(object sender, PropertyChangedExtendedEventArgs<T> e);
}
