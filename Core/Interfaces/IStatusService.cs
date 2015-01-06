namespace MobileApp.Core.Interfaces
{
    using System.Collections.Generic;

    using MobileApp.Core.Models.DataContracts;

    /// <summary>
    /// PRovide storage of statuses
    /// </summary>
    public interface IStatusService
    {
        #region Public Properties

        /// <summary>
        /// Contains users and their statuses
        /// </summary>
        Dictionary<Contact, string> Storage { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add or create new item
        /// </summary>
        /// <param name="contact">contact</param>
        /// <param name="status">contact's status</param>
        void AddOrUpdate(Contact contact, string status);

        #endregion
    }
}