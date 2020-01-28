//********************************************************* 
// LLaser.Component project - TFTTiming.cs
// Created at 2013-5-13
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using LLaser.Common;

namespace LLaser.Component
{
    [Serializable]
    public class TFTTiming : ModelBase
    {
        public TFTTiming()
        {
            #region intialize parameters

            DCK = 33.3;
            DCKPolarity = Polarities.High2Low;
            HSyncWidth = 2;
            HBackPorch = 4;
            HValid = 480;
            HFrontPorch = 2;
            VSyncWidth = 2;
            VBackPorch = 2;
            VValid = 240;
            VFrontPorch = 2;
            HVDelay = 0;
            HPolarity = Polarities.LowEnable;
            VPolarity = Polarities.LowEnable;
            HENPolarity = Polarities.HighEnable;
            DataBlankPolarity = Polarities.High;

            #endregion
        }

        #region Properties and Fields

        private Polarities _dataBlankPolarity;
        private double _dck;
        private Polarities _dckPolarity;
        private Polarities _henPolarity;
        private double _hvDelay;

        public double DCK
        {
            get { return _dck; }
            set
            {
                SetProperty(ref _dck, value);
                NotifyCalculatedProperties();
            }
        }

        public Polarities DCKPolarity
        {
            get { return _dckPolarity; }
            set { SetProperty(ref _dckPolarity, value); }
        }

        public double HVDelay
        {
            get { return _hvDelay; }
            set { SetProperty(ref _hvDelay, value); }
        }

        public Polarities HENPolarity
        {
            get { return _henPolarity; }
            set { SetProperty(ref _henPolarity, value); }
        }

        public Polarities DataBlankPolarity
        {
            get { return _dataBlankPolarity; }
            set { SetProperty(ref _dataBlankPolarity, value); }
        }

        #region calc result

        public uint HT
        {
            get { return CalcHT(); }
        }

        public uint HBP
        {
            get { return CalcHBP(); }
        }

        public uint VT
        {
            get { return CalcVT(); }
        }

        public uint VBP
        {
            get { return CalcVBP(); }
        }

        public double Period
        {
            get { return CalcPeriod(); }
        }

        public double VerTime
        {
            get { return CalcVerTime(); }
        }

        public double FraTime
        {
            get { return CalcFraTime(); }
        }

        #endregion

        #region Hsync parameters

        private uint _hBackPorch;
        private uint _hFrontPorch;
        private Polarities _hPolarity;
        private uint _hSyncWidth;
        private uint _hValid;

        public uint HSyncWidth
        {
            get { return _hSyncWidth; }
            set
            {
                SetProperty(ref _hSyncWidth, value);
                NotifyCalculatedProperties();
            }
        }

        public uint HBackPorch
        {
            get { return _hBackPorch; }
            set
            {
                SetProperty(ref _hBackPorch, value);
                NotifyCalculatedProperties();
            }
        }

        public uint HValid
        {
            get { return _hValid; }
            set
            {
                SetProperty(ref _hValid, value);
                NotifyCalculatedProperties();
            }
        }

        public uint HFrontPorch
        {
            get { return _hFrontPorch; }
            set
            {
                SetProperty(ref _hFrontPorch, value);
                NotifyCalculatedProperties();
            }
        }

        public Polarities HPolarity
        {
            get { return _hPolarity; }
            set { SetProperty(ref _hPolarity, value); }
        }

        #endregion

        #region Vsync parameters

        private uint _vBackPorch;
        private uint _vFrontPorch;
        private Polarities _vPolarity;
        private uint _vSyncWidth;
        private uint _vValid;

        public uint VSyncWidth
        {
            get { return _vSyncWidth; }
            set
            {
                SetProperty(ref _vSyncWidth, value);
                NotifyCalculatedProperties();
            }
        }

        public uint VBackPorch
        {
            get { return _vBackPorch; }
            set
            {
                SetProperty(ref _vBackPorch, value);
                NotifyCalculatedProperties();
            }
        }

        public uint VValid
        {
            get { return _vValid; }
            set
            {
                SetProperty(ref _vValid, value);
                NotifyCalculatedProperties();
            }
        }

        public uint VFrontPorch
        {
            get { return _vFrontPorch; }
            set
            {
                SetProperty(ref _vFrontPorch, value);
                NotifyCalculatedProperties();
            }
        }

        public Polarities VPolarity
        {
            get { return _vPolarity; }
            set { SetProperty(ref _vPolarity, value); }
        }

        #endregion

        #endregion

        #region Calculation methods

        private void NotifyCalculatedProperties()
        {
            OnPropertyChanged("Period");
            OnPropertyChanged("HT");
            OnPropertyChanged("HBP");
            OnPropertyChanged("VT");
            OnPropertyChanged("VBP");
            OnPropertyChanged("VerTime");
            OnPropertyChanged("FraTime");
        }

        private uint CalcHT()
        {
            return HFrontPorch + HValid + CalcHBP();
        }

        private uint CalcHBP()
        {
            return HBackPorch + HSyncWidth;
        }

        private uint CalcVT()
        {
            return VFrontPorch + VValid + CalcVBP();
        }

        private uint CalcVBP()
        {
            return VBackPorch + VSyncWidth;
        }

        private double CalcPeriod()
        {
            return 1000.0/DCK;
        }

        private double CalcVerTime()
        {
            return 1000.0*DCK/CalcHT();
        }

        private double CalcFraTime()
        {
            return 1000.0*CalcVerTime()/CalcVT();
        }

        #endregion

        public override string ToString()
        {
            return string.Format(@"DCK: {0} DCKPolarity: {1} " +
                                 @"HSyncWidth: {2} HBackPorch: {3} HValid: {4} HFrontPorch: {5} HPolarity: {6} " +
                                 @"VSyncWidth: {7} VBackPorch: {8} VValid: {9} VFrontPorch: {10} VPolarity: {11} " +
                                 @"HVDelay: {12} HENPolarity: {13} DataBlankPolarity: {14} " +
                                 @"Period: {15} HT: {16} HBP: {17} VT: {18} VBP: {19} VerTime: {20} FraTime: {21}",
                                 DCK, DCKPolarity,
                                 HSyncWidth, HBackPorch, HValid, HFrontPorch, HPolarity,
                                 VSyncWidth, VBackPorch, VValid, VFrontPorch, VPolarity,
                                 HVDelay, HENPolarity, DataBlankPolarity,
                                 Period, HT, HBP, VT, VBP, VerTime, FraTime);
        }
    }
}