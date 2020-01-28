// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="SignalCollection.cs">
//   
// </copyright>
// <summary>
//   The signal collection.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace LLaser.Component
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// The signal collection.
    /// </summary>
    [Serializable]
    public class SignalCollection : ObservableCollection<Signal>
    {
        #region Fields

        /// <summary>
        /// The _status.
        /// </summary>
        private readonly PowerStatus _status;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalCollection"/> class.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        public SignalCollection(PowerStatus status)
        {
            this._status = status;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The insert item.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        protected override void InsertItem(int index, Signal item)
        {
            // auto increase one to ensure the new added item is unique
            while (this.Any(p => p.Time == item.Time))
            {
                item.Time++;
            }

            item.Power = this._status;
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            if (_status == PowerStatus.Off && this[index].Time == 0) return;
            base.RemoveItem(index);
        }

        #endregion
    }
}