//********************************************************* 
// LLaser project - PowerTimingViewModel.cs
// Created at 2013-5-7
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LLaser.Common;
using LLaser.Common.Command;
using LLaser.Component;
using LLaser.Component.Export;
using LLaser.Component.Import;
using LLaser.Converter;
using LLaser.Component.Lang;
using LLaser.Model;
using LLaser.WPF.Controls;
using Microsoft.Win32;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace LLaser.ViewModel
{
    public class PowerTimingViewModel : ViewModelBase
    {
        #region Constructor

        public PowerTimingViewModel()
        {
            // default power is on
            IsPowerOn = true;

            var VSHDModel = new PowerTimingModel("0", LangManager.Current["lblPowerVSHD"]);
            var VSHAModel = new PowerTimingModel("1", LangManager.Current["lblPowerVSHA"]);
            var VLEDModel = new PowerTimingModel("2", LangManager.Current["lblPowerVLED"]);
            var AC0Model = new PowerTimingModel("3", LangManager.Current["lblPowerAC0"]);
            var AC1Model = new PowerTimingModel("4", LangManager.Current["lblPowerAC1"]);
            var RGBModel = new PowerTimingModel("5", LangManager.Current["lblPowerRGB"]);

            this.Models = new ObservableCollection<PowerTimingModel>
                              {
                                  VSHDModel,
                                  VSHAModel,
                                  VLEDModel,
                                  AC0Model,
                                  AC1Model,
                                  RGBModel
                              };

            // initialize polarity options
            ElectricalLevels = new List<ElectricalLevelModel>
                {
                    new ElectricalLevelModel
                        {
                            Display = LangManager.Current["lblPowerLevelH"],
                            Level = Component.ElectricalLevels.High
                        },
                    new ElectricalLevelModel
                        {
                            Display = LangManager.Current["lblPowerLevelL"],
                            Level = Component.ElectricalLevels.Low
                        }
                };

            // initialize a default signal instance
            NewSignal = new Signal();

            #region register events
            VSHDModel.SignalPropertyChanged -= Models_SignalPropertyChanged;
            VSHAModel.SignalPropertyChanged -= Models_SignalPropertyChanged;
            VLEDModel.SignalPropertyChanged -= Models_SignalPropertyChanged;
            AC0Model.SignalPropertyChanged -= Models_SignalPropertyChanged;
            AC1Model.SignalPropertyChanged -= Models_SignalPropertyChanged;
            RGBModel.SignalPropertyChanged -= Models_SignalPropertyChanged;
            VSHDModel.SignalPropertyChanged += Models_SignalPropertyChanged;
            VSHAModel.SignalPropertyChanged += Models_SignalPropertyChanged;
            VLEDModel.SignalPropertyChanged += Models_SignalPropertyChanged;
            AC0Model.SignalPropertyChanged += Models_SignalPropertyChanged;
            AC1Model.SignalPropertyChanged += Models_SignalPropertyChanged;
            RGBModel.SignalPropertyChanged += Models_SignalPropertyChanged;
            #endregion
        }

        void Models_SignalPropertyChanged(object sender, System.EventArgs e)
        {
            OnPropertyChanged("Models");
        }

        #endregion

        #region Properties and Fields

        private bool _isPowerOn;
        private ObservableCollection<PowerTimingModel> _models;
        private Signal _newSignal;
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                SetProperty(ref _title, value);
            }
        }

        public bool IsPowerOn
        {
            get { return _isPowerOn; }
            set
            {
                OnPropertyChanged("Models");
                SetProperty(ref _isPowerOn, value);
            }
        }

        public ObservableCollection<PowerTimingModel> Models
        {
            get { return _models; }
            set { SetProperty(ref _models, value); }
        }

        public List<ElectricalLevelModel> ElectricalLevels { get; private set; }

        public Signal NewSignal
        {
            get { return _newSignal; }
            set { SetProperty(ref _newSignal, value); }
        }

        public SignalCollection EmptySignelCollection
        {
            get { return new SignalCollection(PowerStatus.On); }
        }

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

        #region datagrid operation commands

        public ICommand AddDataCommand
        {
            get
            {
                return new RelayCommand<PowerTiming>(p =>
                    {
                        try
                        {
                            p.Add(NewSignal, IsPowerOn ? PowerStatus.On : PowerStatus.Off);
                            NewSignal = new Signal();
                        }
                        catch (SignalExistingException e)
                        {
                            MessageBoxResult result =
                                MessageBox.Show(string.Format(LangManager.Current["lblPowerTimeExistingError"], e.Time),
                                                LangManager.Current["titlePowerParameterError"], MessageBoxButton.YesNo,
                                                MessageBoxImage.Error);
                            if (result == MessageBoxResult.Yes)
                            {
                                p.Update(NewSignal, IsPowerOn ? PowerStatus.On : PowerStatus.Off);
                                NewSignal = new Signal();
                            }
                        }
                    });
            }
        }

        #endregion

        #region Export and Import commands

        public ICommand ImportCommand
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        PowerTimingCommonCommands.Import(Models);
                    });
            }
        }

        public ICommand ExportCommand
        {
            get
            {
                return new RelayCommand(() =>
                    {
                        PowerTimingCommonCommands.Export(Models);
                    });
            }
        }

        #endregion

        #endregion
    }
}