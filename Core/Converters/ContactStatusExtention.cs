namespace MobileApp.Core.Converters
{
    using System;

    using MobileApp.Core.Models;

    public static class ContactStatusExtention
    {
        /// <summary>
        /// Cast string to Contact status
        /// </summary>
        /// <param name="status">incoming string</param>
        /// <param name="isDirectionInbound">in case it matters</param>
        /// <returns>contact status</returns>
        public static ContactStatus EvaluateState(this string status, bool isDirectionInbound = false)
        {
            switch (status)
            {
                case "channel_answer":
                case "confirmed":
                    return ContactStatus.Confirmed;
                case "early":
                case "channel_create":
                    return isDirectionInbound ? ContactStatus.EarlyOutbound : ContactStatus.EarlyInbound;
                case "terminated":
                case "idle":
                case "channel_destroy":
                    return ContactStatus.Online;
                default:
                    return ContactStatus.Offline;
            }
        }

        /// <summary>
        /// Cast contact status to string
        /// </summary>
        /// <param name="status">incoming status</param>
        /// <returns>som string</returns>
        public static string GetState(this ContactStatus status)
        {
            switch (status)
            {
                case ContactStatus.Dnd:
                    return "dnd";
                case ContactStatus.Offline:
                    return "terminated";
                case ContactStatus.Online:
                    return "idle";
                case ContactStatus.EarlyInbound:
                case ContactStatus.EarlyOutbound:
                    return "early";
                case ContactStatus.Confirmed:
                    return "confirmed";
                default:
                    throw new ArgumentOutOfRangeException("value");
            }
        }
    }
}