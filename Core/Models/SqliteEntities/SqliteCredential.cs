namespace MobileApp.Core.Models.SqliteEntities
{
    #region

    using Cirrious.MvvmCross.Community.Plugins.Sqlite;

    #endregion

    /// <summary>
    /// The sqlite credential.
    /// </summary>
    [Table("Accounts")]
    public class SqliteCredential
    {
        #region Public Properties
        
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>      
        public string Instance { get; set; }

        /// <summary>
        /// Gets or sets the pin.
        /// </summary>
        
        public string Pin { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Url where app should connect
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// User's filter preferences
        /// </summary>
        public string Settings { get; set; }        

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return this.Instance + "|" + this.Pin + "|" + this.UserId;
        }

        #endregion
    }
}