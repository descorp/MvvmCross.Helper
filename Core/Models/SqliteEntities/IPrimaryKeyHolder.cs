namespace MobileApp.Core.Models.SqliteEntities
{
    using Cirrious.MvvmCross.Plugins.Sqlite;

    /// <summary>
    /// Object with primary key
    /// </summary>
    public interface IPrimaryKeyHolder
    {
        /// <summary>
        /// Incrementation Id
        /// </summary>
        [PrimaryKey]
        [AutoIncrement]
        int Id { get; set; }

        long Timestamp { get; set; }
    }
}