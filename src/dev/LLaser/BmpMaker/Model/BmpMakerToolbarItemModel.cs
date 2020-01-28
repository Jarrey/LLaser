//********************************************************* 
// LLaser project - BmpMakerToolbarItemModel.cs
// Created at 2013-5-20
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
using LLaser.Model;

namespace LLaser.Model
{
    using System.Windows.Input;

    using LLaser.Common;
    using LLaser.ViewModel;
    using LLaser.WPF.Controls;

    public class BmpMakerToolbarItemModel : ToolbarItemModel
    {
        #region commands

        public ICommand OpenCfgCommand
        {
            get { return MainWindowViewModel.OpenBMPMakerCommand; }
        }

        public ICommand SaveCfgCommand
        {
            get { return MainWindowViewModel.SaveBMPMakerCommand; }
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

        #endregion
    }
}
