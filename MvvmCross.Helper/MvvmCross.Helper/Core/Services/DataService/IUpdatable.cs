namespace MvvmCross.Helper.Core.Interfaces
{
    using System;
    using System.Collections.Generic;

    using MvvmCross.Helper.Core.Services.HttpService;

    /// <summary>
    /// Store and sync specific type of data
    /// </summary>
    /// <typeparam name="T">Specific type of data</typeparam>
    public interface IUpdatable<T>
    {
        /// <summary>
        /// Storage
        /// </summary>
        IList<T> Snapshot { get; }

        /// <summary>
        /// Sync collection
        /// </summary>
        /// <returns></returns> 
        void SynchronizeRepository();

        /// <summary>
        /// Get new collection
        /// </summary>
        /// <param name="successAction">action after success synchronization</param>
        void Refresh(Action<Responce> successAction);
    }
}