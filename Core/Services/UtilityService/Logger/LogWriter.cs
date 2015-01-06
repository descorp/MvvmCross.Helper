namespace MobileApp.Core.Services.UtilityService.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Cirrious.CrossCore;
    using Cirrious.CrossCore.Platform;

    using MobileApp.Core.Models.SqliteEntities;
    using MobileApp.Core.Properties;

    /// <summary>
    /// Manage and store log data
    /// </summary>
    public abstract class LogWriter<T> : SqlServiceBase<T>, ILogWriter<T>
        where T : IPrimaryKeyHolder, new()
    {
        #region Static Fields

        private bool wrightLog = true;

        #endregion

        #region Fields

        private readonly object locker = new object();

        private readonly int maxLogLength = Convert.ToInt32(Resources.MaxLogItemsCount);

        private readonly Queue<T> queue = new Queue<T>();

        private readonly ManualResetEvent threadController = new ManualResetEvent(false);

        #endregion

        #region Constructors and Destructors

        protected LogWriter() : base()
        {
            Mvx.Trace(MvxTraceLevel.Diagnostic, "LogWriter initiated");
            Task.Factory.StartNew(this.WriteLog, TaskCreationOptions.LongRunning);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Field for log activation/disactivation
        /// </summary>
        public bool WrightLog
        {
            get
            {
                return this.wrightLog;
            }
            set
            {
                this.wrightLog = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Wright new log item
        /// </summary>
        /// <param name="report">new log item</param>
        /// <returns>result</returns>
        public virtual bool Report(T report)
        {
            lock (this.locker)
            {
                this.queue.Enqueue(report);
            }

            return this.threadController.Set();
        }

        #endregion

        #region Methods

        private void WriteLog()
        {
            while (wrightLog)
            {
                this.threadController.WaitOne();

                while (queue.Any())
                {
                    var result = queue.Dequeue();

                    var table = this.Connection.Table<T>();

                    if (table.Count() >= this.maxLogLength)
                    {
                        try
                        {
                            var list1 = table.ToList();
                            var list = list1.OrderByDescending(n => n.Timestamp);
                            var last = list.Take((int)(maxLogLength * (1 - 1 / Math.E)));

                            foreach (var log in list.Where(n => !last.Contains(n)))
                            {
                                this.Connection.Delete<T>(log.Id);
                            }
                        }
                        catch (Exception)
                        {
                        }              
                    }

                    this.InsertItem(result);
                }

                this.threadController.Reset();
            }
        }

        #endregion
    }


    /// <summary>
    /// This class is necessary since Xamarin do not work with generic classes properly
    /// </summary>
    public class DebugLogWriterImpl : LogWriter<SqliteLogItem>
    {
    }
}