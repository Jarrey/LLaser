//********************************************************* 
// LLaser project - BmpMakerViewModel.cs
// Created at 2013-5-20
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LLaser.Common;
using LLaser.Common.Command;
using LLaser.Component;
using LLaser.Component.Lang;
using LLaser.Model;
using LLaser.View;
using Xceed.Wpf.Toolkit;

namespace LLaser.ViewModel
{
    using System.Windows;

    using LLaser.WPF.Controls;

    using Xceed.Wpf.Toolkit.Zoombox;

    public class BmpMakerViewModel : ViewModelBase
    {
        #region Constructor

        public BmpMakerViewModel()
        {
            Model = new BmpMaker();
        }

        #endregion

        #region Properties and Fields

        public static ObservableCollection<ColorItem> StandardColors
        {
            get
            {
                return new ObservableCollection<ColorItem>()
                    {
                        new ColorItem(Colors.Red, LangManager.Current["ColorRedName"]),
                        new ColorItem(Colors.Green, LangManager.Current["ColorGreenName"]),
                        new ColorItem(Colors.Blue, LangManager.Current["ColorBlueName"]),
                        new ColorItem(Colors.Yellow, LangManager.Current["ColorYellowName"]),
                        new ColorItem(Colors.Magenta, LangManager.Current["ColorMagentaName"]),
                        new ColorItem(Colors.Cyan, LangManager.Current["ColorCyanName"]),
                        new ColorItem(Colors.Black, LangManager.Current["ColorBlackName"]),
                        new ColorItem(Colors.White, LangManager.Current["ColorWhiteName"])
                    };
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public List<ColorBarTypeModel> ColorBarTypes
        {
            get
            {
                return new List<ColorBarTypeModel>()
                {
                    new ColorBarTypeModel(LangManager.Current["colorBarTypeHColor"], ColorBarModel.ColorBarTypes.HColor),
                    new ColorBarTypeModel(LangManager.Current["colorBarTypeVColor"], ColorBarModel.ColorBarTypes.VColor),
                    new ColorBarTypeModel(LangManager.Current["colorBarTypeHRGB"], ColorBarModel.ColorBarTypes.HRGB),
                    new ColorBarTypeModel(LangManager.Current["colorBarTypeVRGB"], ColorBarModel.ColorBarTypes.VRGB),
                    new ColorBarTypeModel(LangManager.Current["colorBarTypeHRGBW"], ColorBarModel.ColorBarTypes.HRGBW),
                    new ColorBarTypeModel(LangManager.Current["colorBarTypeVRGBW"], ColorBarModel.ColorBarTypes.VRGBW)
                };
            }
        }

        public List<GradationStepModel> GradationSteps
        {
            get
            {
                return new List<GradationStepModel>()
                {
                    new GradationStepModel(2),
                    new GradationStepModel(4),
                    new GradationStepModel(8),
                    new GradationStepModel(16),
                    new GradationStepModel(24),
                    new GradationStepModel(32),
                    new GradationStepModel(64),
                    new GradationStepModel(128),
                    new GradationStepModel(256)
                };
            }
        }

        public List<GradationDirectionModel> GradationDirections
        {
            get
            {
                return new List<GradationDirectionModel>()
                {
                    new GradationDirectionModel(LangManager.Current["gradationDirectionH"], GradationModel.Directions.H),
                    new GradationDirectionModel(LangManager.Current["gradationDirectionV"], GradationModel.Directions.V),
                    new GradationDirectionModel(LangManager.Current["gradationDirectionLT2RB"], GradationModel.Directions.LT2RB),
                    new GradationDirectionModel(LangManager.Current["gradationDirectionRB2LT"], GradationModel.Directions.RB2LT),
                    new GradationDirectionModel(LangManager.Current["gradationDirectionCH"], GradationModel.Directions.CH),
                    new GradationDirectionModel(LangManager.Current["gradationDirectionCV"], GradationModel.Directions.CV),
                    new GradationDirectionModel(LangManager.Current["gradationDirectionCE"], GradationModel.Directions.C2E),
                    new GradationDirectionModel(LangManager.Current["gradationDirectionR"], GradationModel.Directions.R)
                };
            }
        }

        public List<PatternTypeModel> PatternTypes
        {
            get
            {
                return new List<PatternTypeModel>()
                {
                    new PatternTypeModel(LangManager.Current["patternTypeWhiteWBorder"], PatternModel.PatternTypes.WhiteWBorder),
                    new PatternTypeModel(LangManager.Current["patternTypeBlackWBorder"], PatternModel.PatternTypes.BlackWBorder),
                    new PatternTypeModel(LangManager.Current["patternTypeWhite"], PatternModel.PatternTypes.White),
                    new PatternTypeModel(LangManager.Current["patternTypeBlack"], PatternModel.PatternTypes.Black)
                };
            }
        }

        public List<WindowTypeModel> WindowTypes
        {
            get
            {
                return new List<WindowTypeModel>()
                {
                    new WindowTypeModel(LangManager.Current["windowTypeSingle"], WindowModel.WindowTypes.Single),
                    new WindowTypeModel(LangManager.Current["windowTypeCross"], WindowModel.WindowTypes.Cross),
                    new WindowTypeModel(LangManager.Current["windowTypeMatrix"], WindowModel.WindowTypes.Matrix),
                    new WindowTypeModel(LangManager.Current["windowTypeWindow2"], WindowModel.WindowTypes.Window2),
                    new WindowTypeModel(LangManager.Current["windowTypeFrame"], WindowModel.WindowTypes.Frame),
                    new WindowTypeModel(LangManager.Current["windowTypeChecker"], WindowModel.WindowTypes.Checker)
                };
            }
        }

        public List<WindowBackgroundTypeModel> WindowBackgroundTypes
        {
            get
            {
                return new List<WindowBackgroundTypeModel>()
                {
                    new WindowBackgroundTypeModel(LangManager.Current["windowBackgroundTypeInside"], WindowModel.WindowBackgroundTypes.Inside),
                    new WindowBackgroundTypeModel(LangManager.Current["windowBackgroundTypeOutside"], WindowModel.WindowBackgroundTypes.Outside)
                };
            }
        }

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        private Color _pointColor;
        public Color PointColor
        {
            get { return _pointColor; }
            set { SetProperty(ref _pointColor, value); }
        }

        #region bmp models

        private BmpMaker _model;
        public BmpMaker Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        #endregion

        #endregion

        #region Commands

        public ICommand ShowColorSelectCommand
        {
            get
            {
                return new RelayCommand<object>(p =>
                    {
                        var colorBarModel = p as ColorBarModel;
                        if (colorBarModel != null)
                        {
                            new ColorSelectView(new ColorSelectViewModel(colorBarModel)).ShowDialog();
                        }
                    });
            }
        }

        public ICommand ShowBitdataSettingCommand
        {
            get
            {
                return new RelayCommand<object>(p =>
                    {
                        var bitDataModel = p as BitDataModel;
                        if (bitDataModel != null)
                        {
                            new BitDataSettingView(new BitDataSettingViewModel(bitDataModel)).ShowDialog();
                        }
                    });
            }
        }

        public ICommand ImportBMPCommand
        {
            get { return new RelayCommand(() => Model.ImportBmp.ImportBmpFile()); }
        }

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

        public ICommand MouseMoveOnImageCommand
        {
            get
            {
                return new RelayCommand<object>(p =>
                    {
                        var parameters = p as object[];
                        if (parameters != null && parameters.Length >= 2)
                        {
                            FrameworkElement element = parameters[0] as FrameworkElement;
                            MouseEventArgs args = parameters[1] as MouseEventArgs;
                            if (element != null && args != null)
                            {
                                Position = args.GetPosition(element);
                                PointColor = Model.Bitmap.GetPixel((int)Position.X, (int)Position.Y);
                            }
                        }
                    });
            }
        }

        public ICommand ZoomInCommand
        {
            get
            {
                return new RelayCommand<FrameworkElement>(p =>
                    {
                        Zoombox zoombox = p as Zoombox;
                        if (zoombox != null) zoombox.Scale += zoombox.Scale > 2 ? 0.5 : 0.1;
                    });
            }
        }

        public ICommand ZoomOutCommand
        {
            get
            {
                return new RelayCommand<FrameworkElement>(p =>
                    {
                        Zoombox zoombox = p as Zoombox;
                        if (zoombox != null) zoombox.Scale -= zoombox.Scale > 2 ? 0.5 : 0.1;
                    });
            }
        }

        #endregion
    }
}