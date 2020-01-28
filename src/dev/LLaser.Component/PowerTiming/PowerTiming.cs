//********************************************************* 
// LLaser.Component project - PowerTiming.cs
// Created at 2013-5-9
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using LLaser.Common;

namespace LLaser.Component
{
    [Serializable]
    public class PowerTiming : ModelBase
    {
        #region constructor

        public PowerTiming()
        {
            // Init singles collection
            OnSignals = new SignalCollection(PowerStatus.On);
            OffSignals = new SignalCollection(PowerStatus.Off);

            // register event for signals collection
            // to notify the bound object to update the collection
            OnSignals.CollectionChanged -= SignalsOnCollectionChanged;
            OnSignals.CollectionChanged += SignalsOnCollectionChanged;
            OffSignals.CollectionChanged -= SignalsOnCollectionChanged;
            OffSignals.CollectionChanged += SignalsOnCollectionChanged;

            _timer.Elapsed += (o, e) =>
                {
                    OnPropertyChanged("Signals");
                    _timer.Enabled = false;
                };
        }

        public PowerTiming(string name)
            : this()
        {
            Name = name;
        }

        #endregion

        #region event handlers

        /// <summary>
        ///     Collection changed to register update event on each items in collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SignalsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Signal s in e.NewItems)
                {
                    s.PropertyChanged -= signal_PropertyChanged;
                    s.PropertyChanged += signal_PropertyChanged;

                    s.UpdateTime -= signal_UpdateTime;
                    s.UpdateTime += signal_UpdateTime;

                    s.EndEdited -= signal_EndEdited;
                    s.EndEdited += signal_EndEdited;
                }
            }
            if (e.OldItems != null)
            {
                foreach (Signal s in e.OldItems)
                {
                    s.PropertyChanged -= signal_PropertyChanged;
                    s.UpdateTime -= signal_UpdateTime;
                    s.EndEdited -= signal_EndEdited;
                }
            }
            OnPropertyChanged("OnSignals");
            OnPropertyChanged("OffSignals");
        }

        /// <summary>
        /// Order the signal collection to sure the signal is ordered by time property
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void signal_EndEdited(object sender, EventArgs eventArgs)
        {
            var signal = sender as Signal;
            if (signal != null)
            {
                var signals = signal.Power == PowerStatus.On ? OnSignals : OffSignals;

                if (signals.Count >= 2)
                {
                    for (int i = 0; i < signals.Count; i++)
                    {
                        for (int j = i + 1; j < signals.Count; j++)
                        {
                            if (signals[i].Time > signals[j].Time)
                            {
                                // swap signal element
                                Signal tempsignal_1 = new Signal(signals[i].Time, signals[i].ElectricalLevel);
                                Signal tempsignal_2 = new Signal(signals[j].Time, signals[j].ElectricalLevel);

                                // make these two signal time to an unset value
                                signals[i].Time = int.MaxValue;
                                signals[j].Time = int.MinValue;

                                signals[i].Time = tempsignal_2.Time;
                                signals[i].ElectricalLevel = signals[j].ElectricalLevel;
                                signals[j].Time = tempsignal_1.Time;
                                signals[j].ElectricalLevel = tempsignal_1.ElectricalLevel;
                            }
                        }
                    }
                }
                UpudateOffSingaleFirstElement();
            }
        }

        /// <summary>
        ///     signal object update its time property, to check the new time value is exsited in current collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool signal_UpdateTime(object sender, PropertyChangedExtendedEventArgs<double> e)
        {
            var signal = sender as Signal;
            if (signal != null)
            {
                var signals = signal.Power == PowerStatus.On ? OnSignals : OffSignals;
                return !Existing(e.NewValue, signal.Power);
            }
            return false;
        }

        /// <summary>
        ///     signal object properties changed, to update the signal collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_timer.Enabled == false)
                _timer.Enabled = true;

            if (SignalPropertyChanged!=null)
                SignalPropertyChanged(this, new EventArgs());
        }

        #endregion

        #region Properties and Fields

        // delay to notify the signal collection changed 
        private readonly Timer _timer = new Timer(500) { Enabled = false };
        private string _name;
        private SignalCollection _onSignals;
        private SignalCollection _offSignals;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public SignalCollection OnSignals
        {
            get { return _onSignals; }
            set { SetProperty(ref _onSignals, value); }
        }

        public SignalCollection OffSignals
        {
            get { return _offSignals; }
            set { SetProperty(ref _offSignals, value); }
        }

        #endregion

        #region Signals collection operation methods

        /// <summary>
        /// The upudate off singale first element.
        /// </summary>
        public void UpudateOffSingaleFirstElement()
        {
            if (OnSignals.Count > 0)
            {
                // update off signals 0 time level
                if (Existing(0, PowerStatus.Off)) Update(0, OnSignals.Last().ElectricalLevel, PowerStatus.Off);
                else Add(0, OnSignals.Last().ElectricalLevel, PowerStatus.Off);
            }
        }

        /// <summary>
        /// Add signal into Signals collection
        /// </summary>
        /// <param name="signal">
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        public void Add(Signal signal, PowerStatus status)
        {
            if (Existing(signal, status))
                throw new SignalExistingException(signal.Time);

            var signals = status == PowerStatus.On ? OnSignals : OffSignals;

            if (signals.Count == 0 || signal.Time > signals.Last().Time)
                signals.Add(signal);
            else
            {
                for (int i = 0; i < signals.Count; i++)
                {
                    if (signal.Time < signals[i].Time)
                    {
                        signals.Insert(i, signal);
                        break;
                    }
                    if (i == signals.Count - 1)
                    {
                        signals.Add(signal);
                        break;
                    }
                }
            }
            UpudateOffSingaleFirstElement();
        }

        public void Add(double time, ElectricalLevels level, PowerStatus status)
        {
            Add(new Signal(time, level), status);
        }

        /// <summary>
        /// Remove one signal from collection
        /// </summary>
        /// <param name="time">
        /// </param>
        /// <param name="level">
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        public void Remove(double time, ElectricalLevels level, PowerStatus status)
        {
            var signals = status == PowerStatus.On ? OnSignals : OffSignals;

            for (int i = 0; i < signals.Count; i++)
            {
                if (signals[i].Time == time && signals[i].ElectricalLevel == level)
                {
                    signals.Remove(signals[i]);
                    break;
                }
            }
            UpudateOffSingaleFirstElement();
        }

        public void Remove(Signal signal, PowerStatus status)
        {
            Remove(signal.Time, signal.ElectricalLevel, status);
        }

        /// <summary>
        /// Update existing signal ElectricalLevel value
        /// </summary>
        /// <param name="signal">
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        public void Update(Signal signal, PowerStatus status)
        {
            Update(signal.Time, signal.ElectricalLevel, status);
        }

        public void Update(double time, ElectricalLevels level, PowerStatus status)
        {
            if (Existing(time, status))
            {
                var signals = status == PowerStatus.On ? OnSignals : OffSignals;
                signals.First(p => p.Time == time).ElectricalLevel = level;
            }
            if (Existing(0, PowerStatus.Off))
                OffSignals.First(p => p.Time == 0).ElectricalLevel = OnSignals.Last().ElectricalLevel;
        }

        /// <summary>
        /// Force remove the signal by signal time
        /// </summary>
        /// <param name="time">
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        public void ForceRemove(double time, PowerStatus status)
        {
            var signals = status == PowerStatus.On ? OnSignals : OffSignals;
            for (int i = 0; i < signals.Count; i++)
            {
                if (signals[i].Time == time)
                {
                    signals.Remove(signals[i]);
                    break;
                }
            }
            UpudateOffSingaleFirstElement();
        }

        public void ForceRemove(Signal signal, PowerStatus status)
        {
            ForceRemove(signal.Time, status);
        }

        /// <summary>
        /// Clean all signals from collection
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        public void CleanAll(PowerStatus status)
        {
            var signals = status == PowerStatus.On ? OnSignals : OffSignals;
            signals.Clear();
        }

        /// <summary>
        /// Check one signal exist in the collection
        /// </summary>
        /// <param name="signal">
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// </returns>
        public bool Existing(Signal signal, PowerStatus status)
        {
            return Existing(signal.Time, status);
        }

        public bool Existing(double time, PowerStatus status)
        {
            var signals = status == PowerStatus.On ? OnSignals : OffSignals;
            return signals.Any(p =>
                {
                    if (p.Time == time) return true;
                    else return false;
                });
        }

        #endregion

        #region events

        /// <summary>
        /// Notify signal property changed
        /// </summary>
        public event EventHandler SignalPropertyChanged;

        #endregion
    }
}