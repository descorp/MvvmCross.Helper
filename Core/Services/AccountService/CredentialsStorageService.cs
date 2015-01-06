namespace MobileApp.Core.Services.AccountService
{
    #region

    using System;
    using System.Linq;

    using MobileApp.Core.Models;
    using MobileApp.Core.Models.SqliteEntities;
    using MobileApp.Core.Services.UtilityService;

    #endregion

    internal class CredentialsStorageService : SqlServiceBase<SqliteCredential>, ICredentialsStorageService
    {
        #region Public Methods and Operators

        /// <summary>
        /// The load credentials.
        /// </summary>
        /// <returns>
        /// The <see cref="CredentialsModel" />.
        /// </returns>
        public CredentialsModel LoadCredentials(out string domain)
        {
            var item = this.LoadAll().LastOrDefault();
            if (item != null)
            {
                domain = item.Domain;
                return new CredentialsModel { Instance = item.Instance, Pin = item.Pin, UserId = item.UserId,  };
            }

            domain = null;
            return new CredentialsModel();
        }

        /// <summary>
        /// The save credentials.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="domain">Url to mediation service</param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public bool SaveCredentials(CredentialsModel model, string domain)
        {
            try
            {
                this.ClearTable();

                this.InsertItem(new SqliteCredential{ Instance = model.Instance, UserId = model.UserId, Pin = model.Pin, Domain = domain});

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}