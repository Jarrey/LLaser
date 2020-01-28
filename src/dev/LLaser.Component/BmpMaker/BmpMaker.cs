//********************************************************* 
// LLaser.Component project - BmpMaker.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using LLaser.Common.Core;
using LLaser.Component.Export;
using LLaser.Component.Import;
using LLaser.Component.Lang;
using Microsoft.Win32;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using System.Runtime.CompilerServices;

namespace LLaser.Component
{
    [Serializable]
    public class BmpMaker : BmpMakerBase
    {
        // locker
        static object locker = new object();

        #region constructor

        public BmpMaker()
        {
            Size = new SizeModel();
            Color = new ColorModel();
            Gradation = new GradationModel();
            ImportBmp = new ImportBMPModel();
            Window = new WindowModel();

            #region register events

            // only work in GUI client
            if (Application.Current != null)
            {
                _backgroundWorker.DoWork -= WorkerDoWork;
                _backgroundWorker.DoWork += WorkerDoWork;
                _backgroundWorker.RunWorkerCompleted -= WorkerRunWorkerCompleted;
                _backgroundWorker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            }

            foreach (var bmpMakerBase in _bmpExecutors.Values)
            {
                bmpMakerBase.BmpPropertyChanged -= BmpPropertyChangedHandler;
                bmpMakerBase.BmpPropertyChanged += BmpPropertyChangedHandler;
            }
            #endregion

            // initialize bmp bimap color array
            BmpPropertyChangedHandler(this, new PropertyChangedEventArgs("Bitmap"));
        }

        #endregion

        #region fields and properties

        private readonly Dictionary<string, BmpMakerBase> _bmpExecutors = new Dictionary<string, BmpMakerBase>();
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private WriteableBitmap _bitmap;

        private SizeModel _sizeModel;
        private ColorModel _colorModel;
        private GradationModel _gradationModel;
        private ImportBMPModel _importBmpModel;
        private WindowModel _windowModel;

        public SizeModel Size
        {
            get { return _sizeModel; }
            set
            {
                SetBmpProperty(ref _sizeModel, value);
                UpdateExecutors(value);
            }
        }

        public ColorModel Color
        {
            get { return _colorModel; }
            set
            {
                SetBmpProperty(ref _colorModel, value);
                UpdateExecutors(value);
            }
        }

        public GradationModel Gradation
        {
            get { return _gradationModel; }
            set
            {
                SetBmpProperty(ref _gradationModel, value);
                UpdateExecutors(value);
            }
        }

        public ImportBMPModel ImportBmp
        {
            get { return _importBmpModel; }
            set
            {
                SetBmpProperty(ref _importBmpModel, value);
                UpdateExecutors(value);
            }
        }

        public WindowModel Window
        {
            get { return _windowModel; }
            set
            {
                SetBmpProperty(ref _windowModel, value);
                UpdateExecutors(value);
            }
        }

        [XmlIgnore]
        public WriteableBitmap Bitmap
        {
            get { return _bitmap; }
        }

        [XmlIgnore]
        public bool IsBusy
        {
            get { return _backgroundWorker.IsBusy; }
        }

        #endregion

        #region event handlers

        private void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            WriteableBitmap bitmap = BitmapFactory.New(1024, 768);

            foreach (var executor in _bmpExecutors.Values)
            {
                executor.ExecuteEffect(ref bitmap);
            }

            // freeze bitmap object to share it between threads
            if (Application.Current != null)
            {
                bitmap.Freeze();
                RunInAppThread(() => { _bitmap = bitmap.CloneCurrentValue(); });
                bitmap = null;
            }
            else
            {
                _bitmap = bitmap;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnPropertyChanged("IsBusy");
            OnPropertyChanged("Bitmap");
        }

        private void BmpPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (Application.Current != null)
            {
                lock (locker)
                {
                    if (!_backgroundWorker.IsBusy)
                    {
                        _backgroundWorker.RunWorkerAsync();
                        OnPropertyChanged("IsBusy");
                    }
                }
            }
            else
            {
                lock (locker)
                {
                    WorkerDoWork(null, null);
                }
            }
        }

        #endregion

        #region save and open functions

        /// <summary>
        ///     Open timing config file
        /// </summary>
        /// <param name="model"></param>
        public static void Open(ref BmpMaker model)
        {
            var ofd = new OpenFileDialog
            {
                Filter = LangManager.Current["lblBMPCfgFileFilters"],
                Title = LangManager.Current["lblOpenFileDialogTitle"]
            };
            if (ofd.ShowDialog() == true)
            {
                IImport importer = ImportFactory.GetImport(Path.GetExtension(ofd.FileName));
                using (Stream stream = ofd.OpenFile())
                {
                    if (importer != null && importer.Import<BmpMaker>(stream))
                    {
                        if (importer.Target != null && importer.Target is BmpMaker)
                        {
                            model = importer.Target as BmpMaker;
                        }
                    }
                    else
                    {
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
        public static void Save(BmpMaker model)
        {
            IExport exporter = null;
            string tempFilePath = string.Empty;

            // choose file path from SaveAs or Save functions
            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = LangManager.Current["lblBMPCfgFileFilters"],
                Title = LangManager.Current["lblSaveFileDialogTitle"],
                FileName = "bmp_config"
            };

            if (sfd.ShowDialog() == true)
                tempFilePath = sfd.FileName;
            else
                return;

            exporter = ExportFactory.GetExport(Path.GetExtension(tempFilePath));
            using (Stream stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                if (exporter == null || !exporter.Export(stream, model))
                {
                    MessageBox.Show(LangManager.Current["lblExportFailedError"],
                                    LangManager.Current["titleExportError"], MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                stream.Flush();
            }
        }

        #endregion

        #region methods

        public void UpdateBitmap()
        {
            BmpPropertyChangedHandler(null, null);
        }

        public void Copy(BmpMaker target)
        {
            base.Copy(target);
            this.Size.Copy(target.Size);
            this.Color.Copy(target.Color);
            this.ImportBmp.Copy(target.ImportBmp);
            this.Gradation.Copy(target.Gradation);
            this.Window.Copy(target.Window);
        }

        private void UpdateExecutors(BmpMakerBase value, [CallerMemberName] string propertyName = null)
        {
            _bmpExecutors[propertyName] = value;
        }

        #endregion
    }
}
