namespace MobileApp.Core.Interfaces
{
    #region

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The SqliteService interface.
    /// </summary>
    public interface ISqliteTableService<T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The load all items from table.
        /// </summary>
        /// <returns>
        /// model
        /// </returns>
        List<T> LoadAll();

        /// <summary>
        /// Save new item to table.
        /// </summary>
        /// <param name="item">
        /// The model.
        /// </param>
        /// <returns>
        /// The result <see cref="bool" />.
        /// </returns>
        bool InsertItem(T item);

        /// <summary>
        /// Update existing Item
        /// </summary>
        /// <param name="item">some object to update</param>
        /// <returns>result</returns>
        bool UpdateItem(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ClearTable();

        #endregion
    }
}