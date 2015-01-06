namespace MobileApp.Core.Services
{
    using System;

    using Cirrious.CrossCore.Core;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Services.UtilityService.Logger;

    public class NotificationService : MvxMainThreadDispatchingObject, INotificationService, IMessageSource
    {
        #region Public Events

        /// <summary>
        /// The error reported.
        /// </summary>
        public event EventHandler<MessageEventArgs> MessageReported;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="id">notification's Id</param>
        public void CloseNotification(int id = 1010)
        {
            this.ReportMessage(string.Empty, NotificationType.CloseNotification, id: id);
        }

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="shortText">Message</param>
        /// <param name="id">notification's Id</param>
        public void ReportNotification(string shortText, int id = 1010)
        {
            this.ReportMessage(shortText, NotificationType.Notification, id: id);
        }

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="shortText">Message</param>
        /// <param name="id">notification's Id</param>
        /// <param name="isRinging"></param>
        public void ReportNotification(string shortText, bool isRinging, string longText = null, int id = 1010)
        {
            this.ReportMessage(shortText, isRinging ? NotificationType.NotificationRinging : NotificationType.NotificationInCall, longText, id);
        }

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="isError">
        /// Display as Error or common message
        /// </param>
        /// <param name="isLong">Duration</param>
        public void ShowMessageBox(string error, bool isError, bool isLong = false)
        {
            this.ReportMessage(error, isError ? NotificationType.Error : NotificationType.Warning);
        }

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// Display as Error or common message
        /// </param>
        /// <param name="logger"></param>
        /// <param name="text"></param>
        public void ShowMessageBox(Exception error, ILogger logger = null, string text = null)
        {
            if (text != null)
            {
                this.ReportMessage(error.ToString(), NotificationType.Error);
            }

            if (logger != null)
            {
                logger.Error(error);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The report type.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type">
        /// The error.
        /// </param>
        protected void ReportMessage(string message, NotificationType type = NotificationType.Warning, string longText = null, int id = 1010)
        {
            if (this.MessageReported == null)
            {
                return;
            }

            this.InvokeOnMainThread(
                () =>
                    {
                        var handler = this.MessageReported;
                        if (handler != null)
                        {
                            handler(this, new MessageEventArgs(type, message, id, longText));
                        }
                    });
        }

        #endregion
    }
}