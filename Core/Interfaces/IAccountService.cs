namespace MobileApp.Core.Interfaces
{
    #region

    using System;

    using MobileApp.Core.Models;
    using MobileApp.Core.Models.DataContracts;

    using Responce = MobileApp.Core.Services.HttpService.Responce;

    #endregion

    /// <summary>
    /// The UserDataService interface.
    /// </summary>
    public interface IAccountService
    {
        #region Public Properties

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        CredentialsModel? Credentials { get; }

        /// <summary>
        /// Gets or sets a value indicating whether is authorized.
        /// </summary>
        bool IsAuthorized { get; set; }

        /// <summary>
        /// User info
        /// </summary>
        Contact UserData { get; set; }

        /// <summary>
        /// Get or set transfer availability
        /// </summary>
        bool IsTransferAvailable { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The authorize.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userPin">
        /// The user pin.
        /// </param>
        /// <param name="externalSuccess">
        /// The succses.
        /// </param>
        /// <param name="externalError">
        /// The error.
        /// </param>
        void Authorize(string instance, string userId, string userPin, Action<Responce> externalSuccess, Action<Exception> externalError = null);

        /// <summary>
        /// The clear.
        /// </summary>
        void Clear();

        #endregion
    }
}