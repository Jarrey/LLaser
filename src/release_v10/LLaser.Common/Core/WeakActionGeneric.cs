﻿//********************************************************* 
// LLaser.Common project - WeakActionGeneric.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 

using System;

namespace LLaser.Common.Core
{
    /// <summary>
    ///     Stores an Action without causing a hard reference
    ///     to be created to the Action's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Action's parameter.</typeparam>
    ////[ClassInfo(typeof(Messenger))]
    public class WeakAction<T> : WeakAction, IExecuteWithObject
    {
        private readonly Action<T> _action;

        /// <summary>
        ///     Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(object target, Action<T> action)
            : base(target, null)
        {
            _action = action;
        }

        public WeakAction(Action<T> action)
            : this(action.Target, action)
        {
        }

        public new bool IsStatic
        {
            get { return _action != null && _action.Target == null; }
        }

        /// <summary>
        ///     Gets the Action associated to this instance.
        /// </summary>
        public new Action<T> Action
        {
            get { return _action; }
        }

        /// <summary>
        ///     Executes the action with a parameter of type object. This parameter
        ///     will be casted to T. This method implements <see cref="IExecuteWithObject.ExecuteWithObject" />
        ///     and can be useful if you store multiple WeakAction{T} instances but don't know in advance
        ///     what type T represents.
        /// </summary>
        /// <param name="parameter">
        ///     The parameter that will be passed to the action after
        ///     being casted to T.
        /// </param>
        public void ExecuteWithObject(object parameter)
        {
            var parameterCasted = (T) parameter;
            Execute(parameterCasted);
        }

        /// <summary>
        ///     Executes the action. This only happens if the action's owner
        ///     is still alive. The action's parameter is set to default(T).
        /// </summary>
        public new void Execute()
        {
            if (_action != null
                && IsAlive)
            {
                _action(default(T));
            }
        }

        /// <summary>
        ///     Executes the action. This only happens if the action's owner
        ///     is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public void Execute(T parameter)
        {
            if (_action != null
                && (IsStatic || IsAlive))
            {
                _action(parameter);
            }
        }
    }
}