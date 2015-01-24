namespace MobileApp.IOS.UI
{
    using System;

    using Cirrious.CrossCore;

    using MobileApp.Core.Interfaces;

    using MonoTouch.UIKit;

    /// <summary>
    /// The error displayer.
    /// </summary>
    public class ErrorDisplayer
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDisplayer" /> class.
        /// </summary>
        public ErrorDisplayer()
        {
            var source = Mvx.Resolve<IMessageSource>();
            source.MessageReported += (sender, args) => ShowError(args.Message, args.NotificationType, args.Id);
        }

        #endregion

        #region Methods

        private static void ShowError(string message, NotificationType type, int id)
        {
            UIAlertView errorView;
            switch (type)
            {
                case NotificationType.Warning:
                    errorView = new UIAlertView("FirmTel Operator", message, null, "OK", null);
            errorView.Show();
                    break;
                case NotificationType.LongWarning:
                    break;
                case NotificationType.Error:
                                errorView = new UIAlertView("Warning", message, null, "OK", null);
            errorView.Show();
                    break;
                case NotificationType.Notification:
                    break;
                case NotificationType.CloseNotification:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        #endregion
    }
}