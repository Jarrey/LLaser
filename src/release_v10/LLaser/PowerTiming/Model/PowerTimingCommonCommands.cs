//********************************************************* 
// LLaser project - PowerTimingCommonCommands.cs
// Created at 2013-5-17
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LLaser.Common.Command;
using LLaser.Component;
using LLaser.Component.Export;
using LLaser.Component.Import;
using LLaser.Component.Lang;
using LLaser.ViewModel;
using LLaser.WPF.Controls;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit.Core.Utilities;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace LLaser.Model
{
    public class PowerTimingCommonCommands
    {
        #region Export and Import commands

        public static void Import(IEnumerable<PowerTimingModel> models)
        {
            var ofd = new OpenFileDialog
            {
                Filter = LangManager.Current["lblPowerTimingCfgFileFilters"],
                Title = LangManager.Current["lblPowerImport"]
            };
            if (ofd.ShowDialog() == true)
            {
                var importer = ImportFactory.GetImport(Path.GetExtension(ofd.FileName));
                if (importer != null && importer.Import<List<PowerTiming>>(ofd.FileName))
                {
                    if (importer.Target != null && importer.Target is List<PowerTiming>)
                        foreach (var powerTiming in importer.Target as List<PowerTiming>)
                        {
                            foreach (
                                var powerTimingModel in
                                    models.Where(powerTimingModel => powerTimingModel.Name == powerTiming.Name))
                            {
                                powerTiming.UpudateOffSingaleFirstElement();
                                powerTimingModel.Model = powerTiming;
                                break;
                            }
                        }
                }
                else
                    MessageBox.Show(LangManager.Current["lblImportWrongFileFormatError"],
                                    LangManager.Current["titleImportError"], MessageBoxButton.OK,
                                    MessageBoxImage.Error);
            }

        }

        public static void Export(IEnumerable<PowerTimingModel> models)
        {
            var powerTimings = models.Select(powerTimingModel => powerTimingModel.Model).ToList();
            var sfd = new SaveFileDialog
            {
                AddExtension = true,
                Filter = LangManager.Current["lblPowerTimingCfgFileFilters"],
                Title = LangManager.Current["lblPowerExport"],
                FileName = "power_timing"
            };
            if (sfd.ShowDialog() == true)
            {
                var exporter = ExportFactory.GetExport(Path.GetExtension(sfd.FileName));
                if (exporter == null || !exporter.Export(sfd.FileName, powerTimings))
                    MessageBox.Show(LangManager.Current["lblExportFailedError"],
                                    LangManager.Current["titleExportError"], MessageBoxButton.OK,
                                    MessageBoxImage.Error);
            }

        }

        #endregion

        #region import and export commands

        public static ICommand ImportCommand = new RelayCommand<Visual>(p =>
            {
                var imgPreview = VisualTreeHelperEx.FindDescendantByType<ImagePreview>(p);
                if (imgPreview != null && imgPreview.DataContext is PowerTimingViewModel)
                {
                    PowerTimingCommonCommands.Import((imgPreview.DataContext as PowerTimingViewModel).Models);
                }
            });

        public static ICommand ExportCommand = new RelayCommand<Visual>(p =>
            {
                var imgPreview = VisualTreeHelperEx.FindDescendantByType<ImagePreview>(p);
                if (imgPreview != null && imgPreview.DataContext is PowerTimingViewModel)
                {
                    PowerTimingCommonCommands.Export((imgPreview.DataContext as PowerTimingViewModel).Models);
                }
            });

        #endregion
    }
}
