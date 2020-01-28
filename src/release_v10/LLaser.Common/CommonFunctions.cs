//********************************************************* 
// LLaser.Common project - CommonFunctions.cs
// Created at 2013-5-8
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LLaser.Common.Command;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace LLaser.Common
{
    public class CommonFunctions
    {
        public static bool SaveImage(FrameworkElement frameworkElement, int width, int height, string filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg|BMP (*bmp)|*.bmp|TIFF (*.tif,*.tiff)|*.tif,*.tiff")
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");
            if (width < 1 || width > int.MaxValue)
                throw new ArgumentException("width is invalid", "width");
            if (height < 1 || height > int.MaxValue)
                throw new ArgumentException("height is invalid", "height");

            var sfd = new SaveFileDialog
                {
                    AddExtension = true,
                    Filter = filter
                };
            string fileExtension = "";

            if (sfd.ShowDialog() == true)
            {
                string extension = Path.GetExtension(sfd.FileName);
                if (extension != null) fileExtension = extension.ToLower();

                Stream s = sfd.OpenFile();
                try
                {
                    var renderBitmap = GetRenderBitmapFromVisual(frameworkElement, width, height);

                    // choose encoder for data
                    BitmapEncoder encoder;
                    if (fileExtension.Contains("jpg"))
                        encoder = new JpegBitmapEncoder();
                    else if (fileExtension.Contains("bmp"))
                        encoder = new BmpBitmapEncoder();
                    else if (fileExtension.Contains("tif"))
                        encoder = new TiffBitmapEncoder();
                    else // default is png format
                        encoder = new PngBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                    // save the data to the stream
                    encoder.Save(s);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    s.Dispose();
                }
            }
            else
                return false;
        }

        public static bool SaveImage(BitmapSource bitmap, string filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg|BMP (*bmp)|*.bmp|TIFF (*.tif,*.tiff)|*.tif,*.tiff")
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = filter
            };
            string fileExtension = "";

            if (sfd.ShowDialog() == true)
            {
                string extension = Path.GetExtension(sfd.FileName);
                if (extension != null) fileExtension = extension.ToLower();

                Stream s = sfd.OpenFile();
                try
                {
                    // choose encoder for data
                    BitmapEncoder encoder;
                    if (fileExtension.Contains("jpg"))
                        encoder = new JpegBitmapEncoder();
                    else if (fileExtension.Contains("bmp"))
                        encoder = new BmpBitmapEncoder();
                    else if (fileExtension.Contains("tif"))
                        encoder = new TiffBitmapEncoder();
                    else // default is png format
                        encoder = new PngBitmapEncoder();
                    // push the rendered bitmap to it
                    encoder.Frames.Add(BitmapFrame.Create(bitmap));
                    // save the data to the stream
                    encoder.Save(s);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    s.Dispose();
                }
            }
            else
                return false;
        }

        public static bool CopyImage(FrameworkElement frameworkElement, int width, int height)
        {
            if (frameworkElement == null)
                throw new ArgumentNullException("frameworkElement");
            if (width < 1 || width > int.MaxValue)
                throw new ArgumentException("width is invalid", "width");
            if (height < 1 || height > int.MaxValue)
                throw new ArgumentException("height is invalid", "height");

            try
            {
                var renderBitmap = GetRenderBitmapFromVisual(frameworkElement, width, height);

                Clipboard.SetImage(renderBitmap);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CopyImage(BitmapSource bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            try
            {
                Clipboard.SetImage(bitmap);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static RenderTargetBitmap GetRenderBitmapFromVisual(FrameworkElement frameworkElement, int width, int height)
        {
            var renderBitmap = new RenderTargetBitmap(
                       width,
                       height,
                       96d,
                       96d,
                       PixelFormats.Pbgra32);

            // render content
            var brush = new VisualBrush
                {
                    Visual = frameworkElement
                };
            var rectangle = new Rectangle
                {
                    Fill = brush,
                    Width = width,
                    Height = height
                };

            rectangle.Measure(new Size(width, height));
            rectangle.Arrange(new Rect(0, 0, width, height));
            renderBitmap.Render(rectangle);
            return renderBitmap;
        }

        #region static commands

        public static ICommand ExitCommand { get { return new RelayCommand(() => Application.Current.Dispatcher.InvokeShutdown()); } }

        #endregion
    }
}