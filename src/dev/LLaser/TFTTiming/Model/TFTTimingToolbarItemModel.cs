//********************************************************* 
// LLaser project - TFTTimingToolbarItemModel.cs
// Created at 2013-5-7
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Windows;
using System.Windows.Input;
using LLaser.Common;
using LLaser.Common.Command;
using LLaser.ViewModel;
using LLaser.WPF.Controls;

namespace LLaser.Model
{
    public class TFTTimingToolbarItemModel : ToolbarItemModel
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

        public ICommand SaveImageCommand { get { return ImagePreview.SaveCommmand; } }

        public ICommand FullScreenCommand { get { return ImagePreview.FullScreenCommand; } }

        public ICommand ExitCommand { get { return CommonFunctions.ExitCommand; } }

        #endregion
    }
}