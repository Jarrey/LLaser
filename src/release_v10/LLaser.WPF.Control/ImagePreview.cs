//*********************************************************
// LLaser.WPF project - ImagePreview.cs
// Created at 2013-5-14
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LLaser.Common;
using LLaser.Common.Command;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace LLaser.WPF.Controls
{
    using System;

    public class ImagePreview : ContentControl
    {
        public enum PreviewContentTypes
        {
            Visual,
            BitmapSource
        }

        static ImagePreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImagePreview),
                                                     new FrameworkPropertyMetadata(typeof(ImagePreview)));
        }

        #region Dependency properties

        public string SaveButtonName
        {
            get { return (string)GetValue(SaveButtonNameProperty); }
            set { SetValue(SaveButtonNameProperty, value); }
        }
        public static readonly DependencyProperty SaveButtonNameProperty =
            DependencyProperty.Register("SaveButtonName", typeof(string), typeof(ImagePreview), new PropertyMetadata("Save"));

        public string FullButtonName
        {
            get { return (string)GetValue(FullButtonNameProperty); }
            set { SetValue(FullButtonNameProperty, value); }
        }
        public static readonly DependencyProperty FullButtonNameProperty =
            DependencyProperty.Register("FullButtonName", typeof(string), typeof(ImagePreview), new PropertyMetadata("Full Screen"));

        public bool ShowButtonsInLeft
        {
            get { return (bool)GetValue(ShowButtonsInLeftProperty); }
            set { SetValue(ShowButtonsInLeftProperty, value); }
        }
        public static readonly DependencyProperty ShowButtonsInLeftProperty =
            DependencyProperty.Register("ShowButtonsInLeft", typeof(bool), typeof(ImagePreview), new PropertyMetadata(false));

        public PreviewContentTypes ContentType
        {
            get { return (PreviewContentTypes)GetValue(ContentTypeProperty); }
            set { SetValue(ContentTypeProperty, value); }
        }
        public static readonly DependencyProperty ContentTypeProperty =
            DependencyProperty.Register("ContentType", typeof(PreviewContentTypes), typeof(ImagePreview), new PropertyMetadata(PreviewContentTypes.Visual));

        public BitmapSource Bitmap
        {
            get { return (BitmapSource)GetValue(BitmapProperty); }
            set { SetValue(BitmapProperty, value); }
        }
        public static readonly DependencyProperty BitmapProperty =
            DependencyProperty.Register("Bitmap", typeof(BitmapSource), typeof(ImagePreview), new PropertyMetadata(null));

        public Visibility CustomButton1Visibility
        {
            get { return (Visibility)GetValue(CustomButton1VisibilityProperty); }
            set { SetValue(CustomButton1VisibilityProperty, value); }
        }
        public static readonly DependencyProperty CustomButton1VisibilityProperty =
            DependencyProperty.Register("CustomButton1Visibility", typeof(Visibility), typeof(ImagePreview), new PropertyMetadata(Visibility.Collapsed));

        public Visibility CustomButton2Visibility
        {
            get { return (Visibility)GetValue(CustomButton2VisibilityProperty); }
            set { SetValue(CustomButton2VisibilityProperty, value); }
        }
        public static readonly DependencyProperty CustomButton2VisibilityProperty =
            DependencyProperty.Register("CustomButton2Visibility", typeof(Visibility), typeof(ImagePreview), new PropertyMetadata(Visibility.Collapsed));

        public string CustomButton1Name
        {
            get { return (string)GetValue(CustomButton1NameProperty); }
            set { SetValue(CustomButton1NameProperty, value); }
        }
        public static readonly DependencyProperty CustomButton1NameProperty =
            DependencyProperty.Register("CustomButton1Name", typeof(string), typeof(ImagePreview), new PropertyMetadata(string.Empty));
        
        public string CustomButton2Name
        {
            get { return (string)GetValue(CustomButton2NameProperty); }
            set { SetValue(CustomButton2NameProperty, value); }
        }
        public static readonly DependencyProperty CustomButton2NameProperty =
            DependencyProperty.Register("CustomButton2Name", typeof(string), typeof(ImagePreview), new PropertyMetadata(string.Empty));

        public Uri CustomButton1Icon
        {
            get { return (Uri)GetValue(CustomButton1IconProperty); }
            set { SetValue(CustomButton1IconProperty, value); }
        }
        public static readonly DependencyProperty CustomButton1IconProperty =
            DependencyProperty.Register("CustomButton1Icon", typeof(Uri), typeof(ImagePreview), new PropertyMetadata(null));
        
        public Uri CustomButton2Icon
        {
            get { return (Uri)GetValue(CustomButton2IconProperty); }
            set { SetValue(CustomButton2IconProperty, value); }
        }
        public static readonly DependencyProperty CustomButton2IconProperty =
            DependencyProperty.Register("CustomButton2Icon", typeof(Uri), typeof(ImagePreview), new PropertyMetadata(null));

        public ICommand CustomButton1Command
        {
            get { return (ICommand)GetValue(CustomButton1CommandProperty); }
            set { SetValue(CustomButton1CommandProperty, value); }
        }
        public static readonly DependencyProperty CustomButton1CommandProperty =
            DependencyProperty.Register("CustomButton1Command", typeof(ICommand), typeof(ImagePreview), new PropertyMetadata(null));

        public ICommand CustomButton2Command
        {
            get { return (ICommand)GetValue(CustomButton2CommandProperty); }
            set { SetValue(CustomButton2CommandProperty, value); }
        }
        public static readonly DependencyProperty CustomButton2CommandProperty =
            DependencyProperty.Register("CustomButton2Command", typeof(ICommand), typeof(ImagePreview), new PropertyMetadata(null));

        public object CustomButton1CommandParameter
        {
            get { return (object)GetValue(CustomButton1CommandParameterProperty); }
            set { SetValue(CustomButton1CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CustomButton1CommandParameterProperty =
            DependencyProperty.Register("CustomButton1CommandParameter", typeof(object), typeof(ImagePreview), new PropertyMetadata(null));
        
        public object CustomButton2CommandParameter
        {
            get { return (object)GetValue(CustomButton2CommandParameterProperty); }
            set { SetValue(CustomButton2CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CustomButton2CommandParameterProperty =
            DependencyProperty.Register("CustomButton2CommandParameter", typeof(object), typeof(ImagePreview), new PropertyMetadata(null));

        #endregion

        #region component name definitions

        private const string BtnFull = "btnFull";
        private const string BtnSave = "btnSave";

        #endregion component name definitions

        #region components

        private Button _btnFull;
        private Button _btnSave;

        #endregion components

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_btnSave == null)
                _btnSave = GetTemplateChild(BtnSave) as Button;
            if (_btnFull == null)
                _btnFull = GetTemplateChild(BtnFull) as Button;

            #region register events

            if (_btnSave != null)
            {
                _btnSave.Click -= btnSave_Click;
                _btnSave.Click += btnSave_Click;
            }
            if (_btnFull != null)
            {
                _btnFull.Click -= btnFull_Click;
                _btnFull.Click += btnFull_Click;
            }

            #endregion register events
        }
        
        /// <summary>
        /// Switch to FullScreen mode to show the image
        /// </summary>
        public void FullScreen()
        {
            if (ContentType == PreviewContentTypes.Visual)
            {
                var frameworkElement = Content as FrameworkElement;
                if (frameworkElement != null)
                {
                    new FullWindow { VisualContent = frameworkElement }.ShowDialog();
                }
            }
            else if (ContentType == PreviewContentTypes.BitmapSource)
            {
                if (Bitmap != null)
                {
                    new FullZoomWindow { Bitmap = Bitmap.CloneCurrentValue() }.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Save image to image file
        /// </summary>
        public void Save()
        {
            if (ContentType == PreviewContentTypes.Visual)
            {
                var frameworkElement = Content as FrameworkElement;
                if (frameworkElement != null)
                    CommonFunctions.SaveImage(frameworkElement, 1440, 900);
            }
            else if (ContentType == PreviewContentTypes.BitmapSource)
            {
                if (Bitmap != null) CommonFunctions.SaveImage(Bitmap, "BMP (*bmp)|*.bmp");
            }
        }

        /// <summary>
        /// Copy image to clipboard
        /// </summary>
        public void Copy()
        {
            if (ContentType == PreviewContentTypes.Visual)
            {
                var frameworkElement = Content as FrameworkElement;
                if (frameworkElement != null) CommonFunctions.CopyImage(frameworkElement, (int)ActualWidth, (int)ActualHeight);
            }
            else if (ContentType == PreviewContentTypes.BitmapSource)
            {
                if (Bitmap != null) CommonFunctions.CopyImage(Bitmap);
            }
        }

        #region event hanlders

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            FullScreen();
        }

        private void btnFull_Click(object sender, RoutedEventArgs e)
        {
            FullScreen();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        #endregion

        #region static commands

        public static ICommand SaveCommmand
        {
            get
            {
                return new RelayCommand<Visual>(p =>
                {
                    var imgPreview = VisualTreeHelperEx.FindDescendantByType<ImagePreview>(p);
                    if (imgPreview != null)
                        imgPreview.Save();
                });
            }
        }

        public static ICommand CopyCommmand
        {
            get
            {
                return new RelayCommand<Visual>(p =>
                {
                    var imgPreview = VisualTreeHelperEx.FindDescendantByType<ImagePreview>(p);
                    if (imgPreview != null)
                        imgPreview.Copy();
                });
            }
        }

        public static ICommand FullScreenCommand
        {
            get
            {
                return new RelayCommand<Visual>(p =>
                {
                    var imgPreview = VisualTreeHelperEx.FindDescendantByType<ImagePreview>(p);
                    if (imgPreview != null)
                        imgPreview.FullScreen();
                });
            }
        }

        #endregion
    }
}