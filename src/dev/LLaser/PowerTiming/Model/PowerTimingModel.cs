//********************************************************* 
// LLaser project - PowerTimingModel.cs
// Created at 2013-5-9
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using LLaser.Common;
using LLaser.Component;

namespace LLaser.Model
{
    using System;

    public class PowerTimingModel : ModelBase
    {
        #region constructor

        public PowerTimingModel(string id, string name)
        {
            Id = id;
            Name = name;
            Model = new PowerTiming(Name);

            // register signal property changed event hanlder
            Model.SignalPropertyChanged -= Model_SignalPropertyChanged;
            Model.SignalPropertyChanged += Model_SignalPropertyChanged;
        }

        void Model_SignalPropertyChanged(object sender, System.EventArgs e)
        {
            if (SignalPropertyChanged != null)
                SignalPropertyChanged(this, new EventArgs());
        }

        #endregion

        #region Properties and Fields

        private string _name;
        private PowerTiming _model;
        private string _id;

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public PowerTiming Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        #endregion

        #region event

        /// <summary>
        /// Notify signal property changed
        /// </summary>
        public event EventHandler SignalPropertyChanged;

        #endregion
    }
}