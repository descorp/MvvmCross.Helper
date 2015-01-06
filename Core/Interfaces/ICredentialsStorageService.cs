namespace MobileApp.Core.Services
{
    using MobileApp.Core.Models;

    public interface ICredentialsStorageService
    {
        /// <summary>
        /// The load credentials.
        /// </summary>
        /// <returns>
        /// The <see cref="CredentialsModel" />.
        /// </returns>
        CredentialsModel LoadCredentials(out string domain);

        /// <summary>
        /// The save credentials.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        bool SaveCredentials(CredentialsModel model, string domain);
    }
}