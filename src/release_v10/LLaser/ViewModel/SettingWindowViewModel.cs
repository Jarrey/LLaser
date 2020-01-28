//*********************************************************
// LLaser project - SettingWindowViewModel.cs
// Created at 2013-6-25
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
using System.Windows.Media;
using LLaser.Common;
using LLaser.Common.Extension;

namespace LLaser.ViewModel
{
    using System.Windows;
    using System.Windows.Input;

    using LLaser.Common.Command;
    using LLaser.Component.Lang;

    public class SettingWindowViewModel : ViewModelBase
    {
        public SettingWindowViewModel(AppSettings setting)
        {
            Setting = setting;
            ReadSettings();
        }

        #region properties and fields

        private readonly AppSettings Setting;

        private string _language = (string)AppSettings.Instance[AppSettings.GLOBAL_LANGUAGE];
        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    Message = LangManager.Current["lblSettingWindowRestartMessage"];
                    this.SetProperty(ref _language, value);
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { this.SetProperty(ref _message, value); }
        }

        public Dictionary<string, string> SupportLanguages
        {
            get { return Setting[AppSettings.GLOBAL_SUPPORT_LANGUAGES].ToString().StringToDictionary(); }
        }

        private double _textSize;
        public double TextSize
        {
            get { return _textSize; }
            set { this.SetProperty(ref _textSize, value); }
        }

        private Color _textColor;
        public Color TextColor
        {
            get { return _textColor; }
            set { this.SetProperty(ref _textColor, value); }
        }

        private Color _textHighlightColor;
        public Color TextHighlightColor
        {
            get { return _textHighlightColor; }
            set { this.SetProperty(ref _textHighlightColor, value); }
        }

        private Color _valueTextColor;
        public Color ValueTextColor
        {
            get { return _valueTextColor; }
            set { this.SetProperty(ref _valueTextColor, value); }
        }

        private Color _lineColor;
        public Color LineColor
        {
            get { return _lineColor; }
            set { this.SetProperty(ref _lineColor, value); }
        }

        private Color _signalColor;
        public Color SignalColor
        {
            get { return _signalColor; }
            set { this.SetProperty(ref _signalColor, value); }
        }

        private Color _signalHighlightColor;
        public Color SignalHighlightColor
        {
            get { return _signalHighlightColor; }
            set { this.SetProperty(ref _signalHighlightColor, value); }
        }

        #endregion

        #region methods

        private void ReadSettings()
        {
            Language = (string)Setting[AppSettings.GLOBAL_LANGUAGE];
            TextSize = (double)Setting[AppSettings.APP_TEXT_SIZE];
            TextColor = (Color)Setting[AppSettings.APP_TEXT_COLOR];
            TextHighlightColor = (Color)Setting[AppSettings.APP_TEXT_HIGHLIGHT_COLOR];
            ValueTextColor = (Color)Setting[AppSettings.APP_VALUE_TEXT_COLOR];
            LineColor = (Color)Setting[AppSettings.APP_LINE_COLOR];
            SignalColor = (Color)Setting[AppSettings.APP_SIGNAL_LINE_COLOR];
            SignalHighlightColor = (Color)Setting[AppSettings.APP_SIGNAL_LINE_HIGHLIGHT_COLOR];
        }

        private void WriteSettings()
        {
            Setting[AppSettings.GLOBAL_LANGUAGE] = Language;
            Setting[AppSettings.APP_TEXT_SIZE] = TextSize;
            Setting[AppSettings.APP_TEXT_COLOR] = TextColor;
            Setting[AppSettings.APP_TEXT_HIGHLIGHT_COLOR] = TextHighlightColor;
            Setting[AppSettings.APP_VALUE_TEXT_COLOR] = ValueTextColor;
            Setting[AppSettings.APP_LINE_COLOR] = LineColor;
            Setting[AppSettings.APP_SIGNAL_LINE_COLOR] = SignalColor;
            Setting[AppSettings.APP_SIGNAL_LINE_HIGHLIGHT_COLOR] = SignalHighlightColor;

            AppSettings.SaveSettings(Setting);
        }

        #endregion

        #region commands

        public ICommand OkCommand
        {
            get
            {
                return new RelayCommand<Window>(p =>
                {
                    WriteSettings();
                    if (p != null) p.Close();
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand<Window>(p =>
                {
                    if (p != null) p.Close();
                });
            }
        }

        public ICommand ApplyCommand
        {
            get
            {
                return new RelayCommand(this.WriteSettings);
            }
        }

        #endregion
    }
}
