//********************************************************* 
// LLaser project - BitDataSettingViewModel.cs
// Created at 2013-6-22
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
using System.Windows.Input;
using LLaser.Common;
using LLaser.Common.Command;
using LLaser.Component;

namespace LLaser.ViewModel
{
    public class BitDataSettingViewModel : ViewModelBase
    {
        #region constructor

        public BitDataSettingViewModel(BitDataModel model)
        {
            Model = model;
        }

        #endregion

        #region Properties and Fields

        private BitDataModel _model;
        public BitDataModel Model
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
                    Model.SetDefaultColors();
                });
            }
        }

        #endregion
    }
}
