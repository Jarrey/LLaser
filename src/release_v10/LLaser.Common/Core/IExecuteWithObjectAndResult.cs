//********************************************************* 
// LLaser.Common project - IExecuteWithObjectAndResult.cs
// Created at 2013-4-25
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code. 
// 
//********************************************************* 


namespace LLaser.Common.Core
{
    public interface IExecuteWithObjectAndResult<TResult>
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
        TResult ExecuteWithObject(object p1, object p2 = null);

        /// <summary>
        ///     Deletes all references, which notifies the cleanup method
        ///     that this entry must be deleted.
        /// </summary>
        void MarkForDeletion();
    }
}