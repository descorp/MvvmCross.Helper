namespace MvvmCross.Helper.Core.Models
{
    /// <summary>
    /// The credentials model.
    /// </summary>
    public struct CredentialsModel
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

        #endregion
    }

    public static class CredentialsExtention
    {
        public static bool IsValid(this CredentialsModel credentials)
        {
            return !(string.IsNullOrWhiteSpace(credentials.Instance) || string.IsNullOrWhiteSpace(credentials.UserId) || string.IsNullOrWhiteSpace(credentials.Pin));
        }
    }
}