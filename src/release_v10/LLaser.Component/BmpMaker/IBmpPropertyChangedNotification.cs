//********************************************************* 
// LLaser.Component project - IBmpPropertyChangedNotification.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.ComponentModel;

namespace LLaser.Component
{
    public interface IBmpPropertyChangedNotification
    {
        event PropertyChangedEventHandler BmpPropertyChanged;
    }
}
