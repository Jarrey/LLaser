//*********************************************************
// LLaser.WPF project - MetroIconButton.cs
// Created at 2013-5-14
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LLaser.WPF.Controls
{
    public class MetroIconButton : Button
    {
        static MetroIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroIconButton), new FrameworkPropertyMetadata(typeof(MetroIconButton)));
        }

        #region dependency properties

        public Uri Icon
        {
            get { return (Uri)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Uri), typeof(MetroIconButton), new PropertyMetadata(null));

        #endregion

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            VisualStateManager.GoToState(this, "MouseEnter", false);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            VisualStateManager.GoToState(this, "MouseLeave", false);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            VisualStateManager.GoToState(this, "MouseDown", false);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            VisualStateManager.GoToState(this, "MouseUp", false);
        }
    }
}
