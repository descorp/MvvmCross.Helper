namespace MobileApp.Core.Services.UtilityService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Cirrious.CrossCore;
    using Cirrious.CrossCore.Platform;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Properties;

    using ISQLiteConnection = Cirrious.MvvmCross.Community.Plugins.Sqlite.ISQLiteConnection;
    using ISQLiteConnectionFactory = Cirrious.MvvmCross.Community.Plugins.Sqlite.ISQLiteConnectionFactory;

    /// <summary>
    /// Base class for SQLite implementation
    /// </summary>
    public class SqlServiceBase<T> : ISqliteTableService<T>
        where T : new()
    {
        #region Fields

        protected ISQLiteConnection Connection;


        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected SqlServiceBase()
        {
            Mvx.Trace("SqlServiceBase initiated");

            var factory = Mvx.Resolve<ISQLiteConnectionFactory>();

            this.Connection = factory.Create(Resources.SqliteDbName);

            var info = this.Connection.GetTableInfo(this.Connection.GetMapping<T>().TableName);
            try
            {
                this.Connection.CreateTable<T>();
            }
            catch (Exception e)
            {
                Mvx.Trace(MvxTraceLevel.Error, e.Message, e);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Clear table
        /// </summary>
        /// <returns>result</returns>
        public bool ClearTable()
        {
            try
            {
                this.Connection.DropTable<T>();
                this.Connection.CreateTable<T>();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Insert new object
        /// </summary>
        /// <param name="item">object to add</param>
        /// <returns>result</returns>
        public virtual bool InsertItem(T item)
        {
            try
            {
                var count = this.Connection.Insert(item, typeof(T));

                return count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Item by Id
        /// </summary>
        /// <param name="id">item's ID</param>
        /// <returns>result</returns>
        public virtual bool Delete(object id)
        {
            try
            {
                var count = this.Connection.Delete<T>(id);

                return count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get full list of objects
        /// </summary>
        /// <returns>list of objects</returns>
        public virtual List<T> LoadAll()
        {
            var model = this.Connection.Table<T>();
            var loadAll = model.ToList();
            return loadAll;
        }

        /// <summary>
        /// Update item in DB
        /// </summary>
        /// <param name="item">object to update</param>
        /// <returns>result</returns>
        public virtual bool UpdateItem(T item)
        {
            try
            {
                var count = this.Connection.Update(item, typeof(T));

                return count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}