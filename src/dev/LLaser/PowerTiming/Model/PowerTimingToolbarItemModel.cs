//********************************************************* 
// LLaser project - PowerTimingToolbarItemModel.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Windows.Input;
using LLaser.Common;
using LLaser.ViewModel;
using LLaser.WPF.Controls;

namespace LLaser.Model
{
    public class PowerTimingToolbarItemModel : ToolbarItemModel
    {
        #region commands

        public ICommand OpenCfgCommand
        {
            get { return MainWindowViewModel.OpenCommand; }
        }

        public ICommand SaveCfgCommand
        {
            get { return MainWindowViewModel.SaveCommand; }
        }

        public ICommand SaveAsCfgCommand
        {
            get { return MainWindowViewModel.SaveAsCommand; }
        }

        public ICommand SaveImageCommand
        {
            get { return ImagePreview.SaveCommmand; }
        }

        public ICommand FullScreenCommand
        {
            get { return ImagePreview.FullScreenCommand; }
        }

        public ICommand ExitCommand
        {
            get { return CommonFunctions.ExitCommand; }
        }

        #region Export and Import commands

        public ICommand ImportCommand
        {
            get { return PowerTimingCommonCommands.ImportCommand; }
        }

        public ICommand ExportCommand
        {
            get { return PowerTimingCommonCommands.ExportCommand; }
        }

        #endregion

        #endregion
    }
}