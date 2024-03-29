﻿//********************************************************* 
// LLaser.Common project - IExecuteWithObject.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

namespace LLaser.Common.Core
{
    /// <summary>
    ///     This interface is meant for the <see cref="WeakAction{T}" /> class and can be
    ///     useful if you store multiple WeakAction{T} instances but don't know in advance
    ///     what type T represents.
    /// </summary>
    ////[ClassInfo(typeof(WeakAction))]
    public interface IExecuteWithObject
    {
        /// <summary>
        ///     The target of the WeakAction.
        /// </summary>
        object Target { get; }

        /// <summary>
        ///     Executes an action.
        /// </summary>
        /// <param name="parameter">
        ///     A parameter passed as an object,
        ///     to be casted to the appropriate type.
        /// </param>
        void ExecuteWithObject(object parameter);

        /// <summary>
        ///     Deletes all references, which notifies the cleanup method
        ///     that this entry must be deleted.
        /// </summary>
        void MarkForDeletion();
    }
}