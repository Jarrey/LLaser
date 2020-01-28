//********************************************************* 
// LLaser project - ColorSelectViewModel.cs
// Created at 2013-6-16
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using LLaser.Common;
using LLaser.Common.Command;
using LLaser.Component;
using LLaser.Component.Lang;
using Xceed.Wpf.Toolkit;

namespace LLaser.ViewModel
{
    public class ColorSelectViewModel : ViewModelBase
    {
        #region constructor

        public ColorSelectViewModel(ColorBarModel model)
        {
            Model = model;
        }

        #endregion

        #region Properties and Fields

        private ColorBarModel _model;
        public ColorBarModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #endregion

        #region Commands

        public ICommand DefaultCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Model.SetDefaultCustomColors();
                });
            }
        }

        #endregion
    }
}
