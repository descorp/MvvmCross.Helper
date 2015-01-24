namespace MvvmCross.Helper.Core.Services.StorageService.Credentails
{
    #region

    using System;
    using System.Linq;

    using MvvmCross.Helper.Core.Models;
    using MvvmCross.Helper.Core.Models.SqliteEntities;
    using MvvmCross.Helper.Core.Services.UtilityService;

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