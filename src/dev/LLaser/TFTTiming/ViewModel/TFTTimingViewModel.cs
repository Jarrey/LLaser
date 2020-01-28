//*********************************************************
// LLaser project - TFTTimingViewModel.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using LLaser.Common;
using LLaser.Common.Command;
using LLaser.Component;
using LLaser.Component.Lang;
using LLaser.Model;
using LLaser.WPF.Controls;
using Xceed.Wpf.Toolkit.Core.Utilities;

namespace LLaser.ViewModel
{
    public class TFTTimingViewModel : ViewModelBase
    {
        #region Constructor

        public TFTTimingViewModel()
        {
            TFTTimingModel = new TFTTiming();

            // initialize DCK polarity options
            DCKPolarities = new List<PolarityModel>
                {
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityH2L"],
                            Polarity = Component.Polarities.High2Low,
                            PathData = "M0,5L15,5L15,25L30,25 M12,10L15,17L19,10"
                        },
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityL2H"],
                            Polarity = Component.Polarities.Low2High,
                            PathData = "M0,25L15,25L15,5L30,5 M12,20L15,13L19,20"
                        }          
                };

            // initialize polarity options
            Polarities = new List<PolarityModel>
                {
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityHEN"],
                            Polarity = Component.Polarities.HighEnable,
                            PathData = "M0,25L10,25L10,5L20,5L20,25L30,25"
                        },
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityLEN"],
                            Polarity = Component.Polarities.LowEnable,
                            PathData = "M0,5L10,5L10,25L20,25L20,5L30,5"
                        },
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityH"],
                            Polarity = Component.Polarities.High,
                            PathData = "M0,5L30,5"
                        },
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityL"],
                            Polarity = Component.Polarities.Low,
                            PathData = "M0,25L30,25"
                        }
                };


            // initialize data blank area polarity options
            DataPolarities = new List<PolarityModel>()
                {
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityH"],
                            Polarity = Component.Polarities.High,
                            PathData = "M0,5L30,5"
                        },
                    new PolarityModel
                        {
                            Display = LangManager.Current["polarityL"],
                            Polarity = Component.Polarities.Low,
                            PathData = "M0,25L30,25"
                        }
                };
        }

        #endregion

        #region Properties and Fields

        private TFTTiming _tftTimingModel;
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public TFTTiming TFTTimingModel
        {
            get { return _tftTimingModel; }
            set { SetProperty(ref _tftTimingModel, value); }
        }

        public List<PolarityModel> DCKPolarities { get; private set; }
        public List<PolarityModel> Polarities { get; private set; }
        public List<PolarityModel> DataPolarities { get; private set; }

        #endregion

        #region Commands

        public ICommand SaveImageCommand
        {
            get { return ImagePreview.SaveCommmand; }
        }

        public ICommand CopyImageCommand
        {
            get { return ImagePreview.CopyCommmand; }
        }

        public ICommand FullScreenCommand
        {
            get { return ImagePreview.FullScreenCommand; }
        }

        #endregion
    }
}