namespace MvvmCross.Helper.Core.Interfaces
{
    using System;

    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    public interface INotificationService
    {
        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="id">notification's Id</param>
        void CloseNotification(int id = 1010);

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
        void ShowMessageBox(string error, bool isError, bool isLong = false);

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="error">
        /// The error.
        /// Display as Error or common message
        /// </param>
        /// <param name="source">where message came from</param>
        /// <param name="text"></param>
        void ShowMessageBox(Exception error, ILogger logger = null, string text = null);

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="shortText">Message</param>
        /// <param name="id">notification's Id</param>
        void ReportNotification(string shortText, int id = 1010);

        /// <summary>
        /// The report error.
        /// </summary>
        /// <param name="shortText">Message</param>
        /// <param name="id">notification's Id</param>
        /// <param name="isRinging"></param>
        void ReportNotification(string shortText, bool isRinging, string longText = null, int id = 1010);
    }
}