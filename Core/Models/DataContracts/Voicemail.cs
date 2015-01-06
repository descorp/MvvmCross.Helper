namespace MobileApp.Core.Models.DataContracts
{
    #region

    using System;

    using Cirrious.CrossCore;

    using MobileApp.Core.ViewModels;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The vmc voicemail.
    /// </summary>
    public class Voicemail : ModelBase<VoicemailViewModel>, IResult
    {
        #region Constants

        private const long BH = 62167219200;

        #endregion

        #region Fields

        private long key;

        private long timestamp;

        #endregion

        #region Public Properties

        /// <summary>
        /// ID of caller
        /// </summary>
        [JsonProperty("call_id")]
        public string CallId { get; set; }

        /// <summary>
        /// Name of caller
        /// </summary>
        [JsonProperty("caller_id_name")]
        public string CallerIdName { get; set; }

        /// <summary>
        /// Caller Id Number
        /// </summary>
        [JsonProperty("caller_id_number")]
        public string CallerIdNumber { get; set; }

        /// <summary>
        /// Id of media file in media library
        /// </summary>
        [JsonProperty("folder")]
        public string Folder { get; set; }

        /// <summary>
        /// Caller Name
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }

        public override long Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Number of bytes in media
        /// </summary>
        [JsonProperty("length")]
        public int Length { get; set; }

        /// <summary>
        /// Id of media file in media library
        /// </summary>
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        [JsonProperty("Result")]
        public bool Result { get; set; }

        /// <summary>
        /// When call made
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp
        {
            get
            {
                return this.timestamp;
            }
            set
            {
                this.timestamp = value;
                this.key = FromUnixTime(value).ToBinary();
            }
        }

        /// <summary>
        /// Callee Name
        /// </summary>
        [JsonProperty("to")]
        public string To { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get View Model
        /// </summary>
        /// <returns>view model</returns>
        public override VoicemailViewModel GetViewModel()
        {
            var viewModel = Mvx.IocConstruct<VoicemailViewModel>();
            viewModel.From = this.CallerIdName.Replace("-", string.Empty).Trim();
            viewModel.IsNew = this.Folder.Equals("new");
            viewModel.Date = FromUnixTime(this.Timestamp);
            viewModel.Mediaid = this.MediaId;

            return viewModel;
        }

        public override string ToString()
        {
            return string.Format("{0}; date: {2}; state: {1}", this.From, this.Folder, FromUnixTime(this.Timestamp).ToString("O"));
        }

        #endregion

        #region Methods

        private static DateTime FromUnixTime(long timestamp)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            return epoch.AddSeconds(timestamp - BH);
        }

        #endregion
    }
}