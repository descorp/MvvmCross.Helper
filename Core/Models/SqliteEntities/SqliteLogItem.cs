namespace MobileApp.Core.Models.SqliteEntities
{
    using System;

    using Cirrious.MvvmCross.Community.Plugins.Sqlite;

    [Table("Trace")]
    public class SqliteLogItem : IPrimaryKeyHolder
    {
        /// <summary>
        /// Incrementation Id
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public long Timestamp { get; set; }

        [Ignore]
        /// <summary>
        /// Where item created
        /// </summary>
        public string Time
        {
            get
            {
                return DateTime.FromBinary(this.Timestamp).ToString("o");
            }
        }

        [MaxLength(2000)]
        /// <summary>
        /// Item
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Where item came from
        /// </summary>
        public string Source { get; set; }

        [MaxLength(10)]
        /// <summary>
        /// Where item came from
        /// </summary>
        public string Level { get; set; }
    } 
}
