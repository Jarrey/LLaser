//********************************************************* 
// LLaser.Component project - ImportBMPModel.cs
// Created at 2013-6-22
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LLaser.Common;
using LLaser.Component.Lang;
using Microsoft.Win32;

namespace LLaser.Component
{
    [Serializable]
    public class ImportBMPModel : BmpMakerBase
    {
        #region properties and fields

        private string _filePath = string.Empty;
        public string FilePath
        {
            get { return _filePath; }
            set { SetBmpProperty(ref _filePath, value.Trim()); }
        }

        private bool _isImportBmpEnabled = false;
        public bool IsImportBmpEnabled
        {
            get { return _isImportBmpEnabled; }
            set { SetBmpProperty(ref _isImportBmpEnabled, value); }
        }

        #endregion

        #region methods

        public void ImportBmpFile()
        {
            string initFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (!string.IsNullOrEmpty(FilePath))
            {
                string folder = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
                    initFolder = folder;
            }

            var ofd = new OpenFileDialog()
                {
                    InitialDirectory = initFolder,
                    Filter = LangManager.Current["lblImportBmpFileFilter"],
                    Title = LangManager.Current["btnImportBMPOpen"]
                };
            if (ofd.ShowDialog() == true)
            {
                FilePath = ofd.FileName;
            }
        }

        public override void ExecuteEffect(ref WriteableBitmap bmp)
        {
            if (!IsImportBmpEnabled || string.IsNullOrEmpty(FilePath)) return;

            if (!File.Exists(FilePath))
            {
                MessageHelper.Current.SetMessage(string.Format(LangManager.Current["lblImportBmpFileNotExisted"], FilePath), MessageTypes.Error);
                return;
            }

            try
            {
                using (var fs = new FileStream(FilePath, FileMode.Open))
                {
                    var decoder = new BmpBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    if (decoder.Frames.Count > 0)
                    {
                        BitmapFrame frame = decoder.Frames[0];

                        int width = frame.PixelWidth;
                        int height = frame.PixelHeight;
                        int stride = (width * frame.Format.BitsPerPixel + 7) / 8;
                        int bytePerPixel = frame.Format.BitsPerPixel / 8;
                        byte[] pixels = new byte[stride * height];
                        frame.CopyPixels(pixels, stride, 0);

                        bmp.ForEach((x, y, color) =>
                        {
                            try
                            {
                                if (x >= width || y >= height) return color;
                                int i = y * stride + bytePerPixel * x;
                                return Color.FromRgb(pixels[i + 2], pixels[i + 1], pixels[i]);
                            }
                            catch (Exception)
                            {
                                return color;
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.Current.SetMessage(ex.Message, MessageTypes.Error);
            }
        }

        public void Copy(ImportBMPModel target)
        {
            this.IsImportBmpEnabled = target.IsImportBmpEnabled;
            this.FilePath = target.FilePath;
        }

        #endregion
    }
}
