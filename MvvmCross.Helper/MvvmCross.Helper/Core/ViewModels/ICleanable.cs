namespace MvvmCross.Helper.Core.ViewModels
{
    using System;
    using System.IO;

    using MvvmCross.Helper.Core.Services.HttpService;

    /// <summary>
    /// Define object that will use events and shared source and release them after use 
    /// </summary>
    public interface ICleanable
    {
        #region Public Methods and Operators
        
        /// <summary>
        /// Free all shared sources and unsubscribe all events here
        /// </summary>
        void CleansAfter();

        /// <summary>
        /// Initiate all shared sources and subscribe all events here
        /// </summary>
        void MakeMess();

        #endregion
    }
}