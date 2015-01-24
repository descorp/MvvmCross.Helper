namespace MvvmCross.Helper.Core.Interfaces
{
    #region

    using System;

    #endregion

    /// <summary>
    /// The error event args.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEventArgs" /> class.
        /// Error event arguments
        /// </summary>
        /// <param name="message">
        /// message
        /// </param>
        /// <param name="notificationType">
        /// is event error
        /// </param>
        /// <param name="id">identifier</param>
        /// <param name="longtext">body's text</param>
        public MessageEventArgs(NotificationType notificationType, string message, int id, string longtext)
        {
            this.Longtext = longtext;
            this.Message = message;
            this.NotificationType = notificationType;
            this.Id = id;
        }

        #endregion

        #region Public Properties

        /// <summary>   
        /// MEssage Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Big text for message body
        /// </summary>
        public string LongText { get; set; }

        /// <summary>
        /// Notification's BODY text
        /// </summary>
        public string Longtext { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets error
        /// </summary>
        public NotificationType NotificationType { get; private set; }

        #endregion
    }
}