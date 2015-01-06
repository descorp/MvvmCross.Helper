namespace MobileApp.Core.Interfaces
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;

    using MobileApp.Core.Models.DataContracts;
    using MobileApp.Core.ViewModels;

    #endregion

    /// <summary>
    /// The DataService interface.
    /// </summary>
    public interface IDataService
    {
        #region Public Properties

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        IEnumerable<ContactViewModel> Contacts { get; }

        /// <summary>
        /// List of external numbers
        /// </summary>
        IEnumerable<string> ExternalNumberList { get; }

        /// <summary>
        /// Gets the Queues
        /// </summary>
        IEnumerable<VmcQueue> Queues { get; }

        /// <summary>
        /// Gets the voicemails.
        /// </summary>
        IEnumerable<VoicemailViewModel> Voicemails { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get external number didid by number full form
        /// </summary>
        string GetExNbrDididByNumber(string number);

        /// <summary>
        /// Get external number in full form by didid
        /// </summary>
        string GetExNbrNumberByDidid(string didid);

        /// <summary>
        /// Update list of user's devices
        /// </summary>
        /// <param name="successAction"></param>
        void UpdateDevices(Action<bool> successAction);

        /// <summary>
        /// The get media stream.
        /// </summary>
        /// <param name="successAction">
        /// The success action.
        /// </param>
        /// <param name="errorAction">
        /// The error action.
        /// </param>
        /// <param name="mediaId">
        /// The media id.
        /// </param>
        void GetMediaStream(Action<Stream> successAction, Action<Exception> errorAction, string mediaId);

        /// <summary>
        /// Get raw contacts
        /// </summary>
        /// <returns>Raw contacts</returns>
        IEnumerable<VmcContact> GetRawContacts();

        /// <summary>
        /// The update contacts.
        /// </summary>
        /// <param name="successAction">
        /// The success action.
        /// </param>
        void UpdateContacts(Action<string> successAction);

        /// <summary>
        /// The update external numbers.
        /// </summary>
        /// <param name="successAction">
        /// The success action.
        /// </param>
        void UpdateExternalNumbers(Action<bool> successAction);

        /// <summary>
        /// The update queue
        /// </summary>
        /// <param name="successAction">
        /// The success Action.
        /// </param>
        void UpdateQueue(Action<bool> successAction);

        /// <summary>
        /// The update voicemail.
        /// </summary>
        /// <param name="successAction">
        /// The success action.
        /// </param>
        void UpdateVoicemail(Action<string> successAction);

        #endregion
    }
}