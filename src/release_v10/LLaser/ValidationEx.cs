//*********************************************************
// LLaser project - ValidationEx.cs
// Created at 2013-5-15
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System.Collections.Generic;
using System.Windows;
using System.Linq;
using LLaser.Common;
using LLaser.Component;

namespace LLaser
{
    public class ValidationEx
    {
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.RegisterAttached("ErrorMessage", typeof(string), typeof(ValidationEx),
                                                new PropertyMetadata(string.Empty, (
                                                    o, e) =>
                                                    {
                                                        _errorMessages = _errorMessages ??
                                                                        new Dictionary<DependencyObject, string>();
                                                        if (string.IsNullOrEmpty(e.NewValue.ToString()))
                                                        {
                                                            if (_errorMessages.ContainsKey(o)) _errorMessages.Remove(o);
                                                        }
                                                        else
                                                        {
                                                            _errorMessages[o] = e.NewValue.ToString();
                                                        }
                                                        SyncMessageHelper();
                                                    }));

        public static string GetErrorMessage(DependencyObject obj)
        {
            return (string)obj.GetValue(ErrorMessageProperty);
        }

        public static void SetErrorMessage(DependencyObject obj, string value)
        {
            obj.SetValue(ErrorMessageProperty, value);
        }

        private static Dictionary<DependencyObject, string> _errorMessages;

        private static void SyncMessageHelper()
        {
            if (_errorMessages.Count > 0)
                MessageHelper.Current.SetMessage(_errorMessages.Last().Value, MessageTypes.Error);
            else
                MessageHelper.Current.ResetMessage();
        }
    }
}