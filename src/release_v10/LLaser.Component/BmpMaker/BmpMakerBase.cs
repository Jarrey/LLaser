//********************************************************* 
// LLaser.Component project - BmpMakerBase.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using LLaser.Common;

namespace LLaser.Component
{
    public class BmpMakerBase : ModelBase, IBmpPropertyChangedNotification, IBmpExecutor
    {
        public event PropertyChangedEventHandler BmpPropertyChanged;

        protected void OnBmpPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = BmpPropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected bool SetBmpProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (storage != null && storage.Equals(value)) return false;
            bool rtn = SetProperty(ref storage, value, propertyName);
            OnBmpPropertyChanged();
            return rtn;
        }

        protected void RunInAppThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public virtual void ExecuteEffect(ref System.Windows.Media.Imaging.WriteableBitmap bmp)
        {
            // igonre
        }

        public virtual void Copy(BmpMakerBase target) { }
    }
}
