//********************************************************* 
// LLaser.Component project - Signal.cs
// Created at 2013-5-9
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

namespace LLaser.Component
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;
    using LLaser.Common;

    [Serializable]
    public class Signal : ModelBase, IEditableObject
    {
        #region constrcutors

        public Signal()
            : this(0, ElectricalLevels.Low)
        {
        }

        public Signal(double time, ElectricalLevels level)
        {
            Time = time;
            ElectricalLevel = level;
        }

        #endregion

        #region properties and fields

        private ElectricalLevels _electricalLevel;
        private bool _isSelected;
        private double _time;
        private PowerStatus _power;

        public double Time
        {
            get { return _time; }
            set
            {
                if (OnUpdateTime(_time, value))
                    SetProperty(ref _time, value);
                else
                    OnPropertyChanged("Time");
            }
        }

        public ElectricalLevels ElectricalLevel
        {
            get { return _electricalLevel; }
            set { SetProperty(ref _electricalLevel, value); }
        }

        [XmlIgnore]
        public PowerStatus Power
        {
            get { return _power; }
            set { SetProperty(ref _power, value); }
        }

        [XmlIgnore]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        /// <summary>
        /// property to ingore the EndCommit event
        /// </summary>
        [XmlIgnore]
        public static bool IngoreCommitEvent { get; set; }

        #endregion

        #region updatetime property event

        public event PropertyChangedExtendedEventHandler<double> UpdateTime;

        private bool OnUpdateTime(double oldValue, double newValue)
        {
            return UpdateTime == null ||
                   UpdateTime(this, new PropertyChangedExtendedEventArgs<double>("Time", oldValue, newValue));
        }

        #endregion

        #region methods from IEditableObject

        public void BeginEdit() { }

        public void CancelEdit() { }

        public void EndEdit()
        {
            if (!IngoreCommitEvent && EndEdited != null)
                EndEdited(this, new EventArgs());
        }

        /// <summary>
        /// the event to notify DataGrid end edited this signal object
        /// </summary>
        public event EventHandler EndEdited;

        #endregion
    }
}