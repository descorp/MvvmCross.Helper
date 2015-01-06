namespace MobileApp.Core.Services.UtilityService.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Models.SqliteEntities;

    /// <summary>
    /// Controll collection of logs
    /// </summary>
    public class Logger : ILogger
    {
        private readonly ILogWriter<SqliteLogItem> errorLoger;

        /// <summary>
        /// Create new logger
        /// </summary>
        /// <param name="errorLoger">access to error's table</param>
        public Logger(ILogWriter<SqliteLogItem> errorLoger )
        {            
            this.errorLoger = errorLoger;
        }

        /// <summary>
        /// Log some common event
        /// </summary>
        /// <param name="message">What is happened</param>
        /// <param name="sender">Where it came from</param>
        /// <returns>is logged</returns>
        public bool Debug(string message, object sender = null)
        {
            // TODO: why MvxTrace crashes
            ////MvxTrace.Trace(MvxTraceLevel.Diagnostic, this.ToDiagnostics(message));
            return this.errorLoger.Report(new SqliteLogItem { Level = "Debug", Source = (sender != null ? sender.GetType().Name : string.Empty), Data = message, Timestamp = DateTime.Now.ToBinary() });
        }

        /// <summary>
        /// Log this error
        /// </summary>
        /// <param name="exception">What is happened</param>
        /// <param name="sender">Where it took place</param>
        /// <returns>is logged</returns>
        public bool Error(Exception exception, object sender = null)
        {
            // TODO: why MvxTrace crashes
            ////MvxTrace.Trace(MvxTraceLevel.Error, this.ToDiagnostics(exception.Message));
            return this.errorLoger.Report(new SqliteLogItem { Level = "Error", Data = exception.ToString(), Source = sender != null ? sender.GetType().Name : string.Empty, Timestamp = DateTime.Now.ToBinary() });
        }

        /// <summary>
        /// Create report for crash reporter
        /// </summary>
        /// <returns>list of strings - the report</returns>
        public List<string> CreateReport()
        {
            var service = this.errorLoger as ISqliteTableService<SqliteLogItem>;
            return service != null ? service.LoadAll().Select(n => string.Format("{0} - {1}", n.Time, n.Data)).ToList() : new List<string>();
        }

        private string ToDiagnostics(string str)
        {
            var prepare = (str ?? string.Empty).Trim();
            if (prepare.Length > 50)
            {
                prepare = prepare.Substring(0, 50);
            }

            return prepare;
        }
    }
}