namespace MvvmCross.Helper.Core.Models.DataContracts
{
    #region

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// Interface for xml-rpc results
    /// </summary>
    public interface IResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether method call was successful
        /// </summary>
        [JsonProperty("result")]
        bool Result { get; set; }

        #endregion
    }
}