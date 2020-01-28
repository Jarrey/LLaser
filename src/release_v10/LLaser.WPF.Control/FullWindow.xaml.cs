//*********************************************************
// LLaser.WPF project - FullWindow.xaml.cs
// Created at 2013-5-14
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System;
using System.Windows;
using System.Windows.Input;
using LLaser.Common;

namespace LLaser.WPF.Controls
{
    /// <summary>
    ///     Interaction logic for FullWindow.xaml
    /// </summary>
    public partial class FullWindow : Window
    {
        #region constructor

        public FullWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region dependency properties

        public static readonly DependencyProperty VisualContentProperty =
            DependencyProperty.Register("VisualContent", typeof(Object), typeof(FullWindow),
                                        new PropertyMetadata(null));
        public Object VisualContent
        {
            get { return GetValue(VisualContentProperty); }
            set { SetValue(VisualContentProperty, value); }
        }

        #endregion

        #region event hanlders

        private void btnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_OnClick(object sender, RoutedEventArgs e)
        {
            var frameworkElement = VisualContent as FrameworkElement;
            if (frameworkElement != null)
                CommonFunctions.SaveImage(frameworkElement, 1440, 900);
        }

        private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnCopyImage(object sender, ExecutedRoutedEventArgs e)
        {
            var frameworkElement = VisualContent as FrameworkElement;
            if (frameworkElement != null)
                CommonFunctions.CopyImage(frameworkElement, (int)frameworkElement.ActualWidth, (int)frameworkElement.ActualHeight);
        }

        private void FullWindow_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        #endregion
    }
}