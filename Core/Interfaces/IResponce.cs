namespace MobileApp.Core.Interfaces
{
    using MobileApp.Core.Models.DataContracts;

    using Newtonsoft.Json;

    /// <summary>
    /// Interface of object with result, error and error code
    /// </summary>
    public interface IResponce : IResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        [JsonProperty("errorcode")]
        int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the data
        /// </summary>
        [JsonProperty("message")]
        string Message { get; set; }

        #endregion
    }
}