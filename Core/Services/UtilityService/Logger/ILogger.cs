namespace MobileApp.Core.Services.UtilityService.Logger
{
    using System;
    using System.Collections.Generic;

    public interface ILogger
    {
        /// <summary>
        /// Report info
        /// </summary>
        /// <param name="message">info</param>
        /// <param name="sender">where it came from</param>
        /// <returns>result</returns>
        bool Debug(string message, object sender = null);

        /// <summary>
        /// Report error into log
        /// </summary>
        /// <param name="exception">more specific detales</param>
        /// <param name="sender">where error came from</param>
        /// <returns>result</returns>
        bool Error(Exception exception, object sender = null);


        /// <summary>
        /// Create report for crash reporter
        /// </summary>
        /// <returns>list of strings - the report</returns>
        List<string> CreateReport();
    }
}