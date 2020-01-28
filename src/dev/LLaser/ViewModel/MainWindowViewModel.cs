//*********************************************************
// LLaser project - MainWindowViewModel.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LLaser.Common;
using LLaser.Common.Command;
using LLaser.Common.Core;
using LLaser.Component;
using LLaser.Component.Lang;
using LLaser.Model;
using LLaser.View;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;

namespace LLaser.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ComputerInfo _computerInfo = new ComputerInfo();
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
        private ObservableCollection<CompomentModel> _compomentItems;

        public MainWindowViewModel()
        {
            // initialize components
            CompomentItems = new ObservableCollection<CompomentModel>
                {
                    // initialize TFTTiming panel model
                    new CompomentModel
                        {
                            Type = FunctionTypes.TFTTiming,
                            TabItem = new TabItemModel
                                {
                                    Header = LangManager.Current["TFTTimingTitle"],
                                    Content = new TFTTimingViewModel {Title = LangManager.Current["TFTTimingTitle"]}
                                },
                            Menu = new TFTTimingMenuItemModel(),
                            Toolbar = new TFTTimingToolbarItemModel()
                        },
                    // initialize PowerTiming panel model
                    new CompomentModel
                        {
                            Type = FunctionTypes.PowerTiming,
                            TabItem = new TabItemModel
                                {
                                    Header = LangManager.Current["PowerTimingTitle"],
                                    Content = new PowerTimingViewModel {Title = LangManager.Current["PowerTimingTitle"]}
                                },
                            Menu = new PowerTimingMenuItemModel(),
                            Toolbar = new PowerTimingToolbarItemModel()
                        },
                    // initialize BmpMaker panel model
                    new CompomentModel
                        {
                            Type = FunctionTypes.BmpMaker,
                            TabItem = new TabItemModel
                                {
                                    Header = LangManager.Current["BmpMakerTitle"],
                                    Content = new BmpMakerViewModel {Title = LangManager.Current["BmpMakerTitle"]}
                                },
                            Menu = new BmpMakerMenuItemModel(),
                            Toolbar = new BmpMakerToolbarItemModel()
                        }
                };

            #region register events

            // register timer tick event
            _timer.Tick += (o, e) =>
                {
                    OnPropertyChanged("Now");
                    OnPropertyChanged("UsedMemory");
                    OnPropertyChanged("TotalMemory");
                };
            // start the timer to update the time in view status bar
            _timer.IsEnabled = true;

            // static property update event for apptitle
            LLaserConfgModel.CurrentFileChanged -= LLaserConfgModel_CurrentFileChanged;
            LLaserConfgModel.CurrentFileChanged += LLaserConfgModel_CurrentFileChanged;
            #endregion
        }

        public ObservableCollection<CompomentModel> CompomentItems
        {
            get { return _compomentItems; }
            set
            {
                _compomentItems = value;
                SetProperty(ref _compomentItems, value);
            }
        }

        #region window title

        public static string AppTitle
        {
            get
            {
                return LangManager.Current["AppName"] + (!string.IsNullOrEmpty(LLaserConfgModel.CurrentFile) ? " - " + LLaserConfgModel.CurrentFile : string.Empty);
            }
        }
        public static void LLaserConfgModel_CurrentFileChanged(object sender, EventArgs e)
        {
            if (AppTitleChanged != null) AppTitleChanged(null, new EventArgs());
        }
        public static event EventHandler AppTitleChanged;

        #endregion

        #region status bar information properties

        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public string UsedMemory
        {
            get { return _computerInfo.AvailablePhysicalMemory.ToBinarySize(); }
        }

        public string TotalMemory
        {
            get { return _computerInfo.TotalPhysicalMemory.ToBinarySize(); }
        }

        #endregion

        #region commands

        public ICommand ExitCommand
        {
            get { return CommonFunctions.ExitCommand; }
        }

        #region save and open commands

        public static ICommand SaveCommand
        {
            get
            {
                return new RelayCommand<FrameworkElement>(p => SaveTimingConfig(p));
            }
        }

        public static ICommand SaveBMPMakerCommand
        {
            get
            {
                return new RelayCommand<FrameworkElement>(p => SaveBMPMakerConfig(p));
            }
        }

        public static ICommand SaveAsCommand
        {
            get
            {
                return new RelayCommand<FrameworkElement>(p => SaveTimingConfig(p, true));
            }
        }

        private static void SaveTimingConfig(FrameworkElement element, bool isSaveAs = false)
        {
            var model = element.DataContext as MainWindowViewModel;
            if (model != null)
            {
                var confgModel = new LLaserConfgModel();
                foreach (var item in model.CompomentItems)
                {
                    if (item.Type == FunctionTypes.TFTTiming)
                        confgModel.TFTTimingModel = (item.TabItem.Content as TFTTimingViewModel).TFTTimingModel;
                    if (item.Type == FunctionTypes.PowerTiming)
                    {
                        confgModel.PowerTimingModels = (item.TabItem.Content as PowerTimingViewModel).Models.Select(powerTimingModel => powerTimingModel.Model).ToList();
                    }
                }
                LLaserConfgModel.Save(confgModel, isSaveAs);
            }
        }

        private static void SaveBMPMakerConfig(FrameworkElement element)
        {
            var model = element.DataContext as MainWindowViewModel;
            if (model != null)
            {
                BmpMaker config = (from item in model.CompomentItems
                                   where item.Type == FunctionTypes.BmpMaker
                                   select (item.TabItem.Content as BmpMakerViewModel).Model).FirstOrDefault();
                BmpMaker.Save(config);
            }
        }

        public static ICommand OpenBMPMakerCommand
        {
            get
            {
                return new RelayCommand<FrameworkElement>(p =>
                    {
                        var model = p.DataContext as MainWindowViewModel;
                        if (model != null)
                        {
                            var confgModel = new BmpMaker();
                            BmpMaker.Open(ref confgModel);
                            foreach (var item in model.CompomentItems.Where(item => item.Type == FunctionTypes.BmpMaker && confgModel != null))
                            {
                                (item.TabItem.Content as BmpMakerViewModel).Model.Copy(confgModel);
                                break;
                            }
                        }
                    });
            }
        }

        public static ICommand OpenCommand
        {
            get
            {
                return new RelayCommand<FrameworkElement>(p =>
                    {
                        var model = p.DataContext as MainWindowViewModel;
                        if (model != null)
                        {
                            var confgModel = new LLaserConfgModel();
                            LLaserConfgModel.Open(ref confgModel);
                            foreach (var item in model.CompomentItems)
                            {
                                if (item.Type == FunctionTypes.TFTTiming && confgModel.TFTTimingModel != null)
                                    (item.TabItem.Content as TFTTimingViewModel).TFTTimingModel = confgModel.TFTTimingModel;
                                if (item.Type == FunctionTypes.PowerTiming && confgModel.PowerTimingModels != null)
                                {
                                    foreach (var m in (item.TabItem.Content as PowerTimingViewModel).Models)
                                    {
                                        m.Model = confgModel.PowerTimingModels.First(powerTimingModel => powerTimingModel.Name == m.Model.Name);
                                    }
                                }
                            }
                        }
                    });
            }
        }

        #endregion

        public static ICommand OpenSettingCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new SettingWindow().ShowDialog();
                });
            }
        }

        public static ICommand OpenHelpCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Process.Start(@"LLaser.chm");
                });
            }
        }
        #endregion
    }
}