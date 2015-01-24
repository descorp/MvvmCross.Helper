namespace MvvmCross.Helper.Core.Interfaces
{
    #region

    using System;

    #endregion

    /// <summary>
    /// The ErrorSource interface.
    /// </summary>
    public interface IMessageSource
    {
        #region Public Events

        /// <summary>
        /// The error reported.
        /// </summary>
        event EventHandler<MessageEventArgs> MessageReported;

        #endregion
    }
}