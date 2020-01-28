//*********************************************************
// LLaser.WPF project - ViewModelExtension.cs
// Created at 2013-5-14
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System.Windows;

namespace LLaser.WPF.Controls
{
    public sealed class ViewModelExtension : DependencyObject
    {
        public static readonly DependencyProperty InstanceProperty =
            DependencyProperty.Register("Instance", typeof(object), typeof(ViewModelExtension),
                                        new PropertyMetadata(null));

        public object Instance
        {
            get { return GetValue(InstanceProperty); }
            set { SetValue(InstanceProperty, value); }
        }
    }
}
