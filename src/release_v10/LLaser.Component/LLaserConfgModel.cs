//********************************************************* 
// LLaser.Component project - LLaserConfgModel.cs
// Created at 2013-5-21
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.Generic;
using System.IO;
using System.Windows;
using LLaser.Common.Core;
using LLaser.Component.Export;
using LLaser.Component.Import;
using LLaser.Component.Lang;
using Microsoft.Win32;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace LLaser.Component
{
    using System;

    public class LLaserConfgModel
    {
        #region static members

        private static string _currentFile;
        public static string CurrentFile
        {
            get
            {
                return _currentFile;
            }
            set
            {
                _currentFile = value;
                if (CurrentFileChanged != null)
                    CurrentFileChanged(null, new EventArgs());
            }
        }
        public static event EventHandler CurrentFileChanged;

        #endregion

        #region properties

        public TFTTiming TFTTimingModel { get; set; }
        public List<PowerTiming> PowerTimingModels { get; set; }

        #endregion

        #region save and open functions

        /// <summary>
        ///     Open timing config file
        /// </summary>
        /// <param name="model"></param>
        public static void Open(ref LLaserConfgModel model)
        {
            var ofd = new OpenFileDialog
                {
                    Filter = LangManager.Current["lblTimingCfgFileFilters"],
                    Title = LangManager.Current["lblOpenFileDialogTitle"]
                };
            if (ofd.ShowDialog() == true)
            {
                IImport importer = ImportFactory.GetImport(Path.GetExtension(ofd.FileName));
                using (Stream stream = ofd.OpenFile())
                {
                    if (importer != null && importer.Import<LLaserConfgModel>(stream))
                    {
                        if (importer.Target != null && importer.Target is LLaserConfgModel)
                        {
                            model = importer.Target as LLaserConfgModel;
                        }
                        CurrentFile = ofd.FileName;
                    }
                    else
                    {
                        CurrentFile = string.Empty;
                        MessageBox.Show(LangManager.Current["lblImportWrongFileFormatError"],
                                        LangManager.Current["titleImportError"], MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        ///     Save/SaveAs timing config file
        /// </summary>
        /// <param name="model"></param>
        public static void Save(LLaserConfgModel model, bool isSaveAs = false)
        {
            IExport exporter = null;
            string tempFilePath = string.Empty;

            // choose file path from SaveAs or Save functions
            if (isSaveAs || string.IsNullOrEmpty(CurrentFile))
            {
                var sfd = new SaveFileDialog
                {
                    AddExtension = true,
                    Filter = LangManager.Current["lblTimingCfgFileFilters"],
                    Title = isSaveAs ? LangManager.Current["lblSaveAsFileDialogTitle"] : LangManager.Current["lblSaveFileDialogTitle"],
                    FileName = "timing"
                };

                if (sfd.ShowDialog() == true)
                    tempFilePath = sfd.FileName;
                else
                    return;
            }
            else
                tempFilePath = CurrentFile;

            exporter = ExportFactory.GetExport(Path.GetExtension(tempFilePath));
            using (Stream stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                if (exporter == null || !exporter.Export(stream, model))
                {
                    CurrentFile = string.Empty;
                    MessageBox.Show(LangManager.Current["lblExportFailedError"],
                                    LangManager.Current["titleExportError"], MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                else
                    CurrentFile = tempFilePath;
                stream.Flush();
            }
        }

        #endregion

    }
}