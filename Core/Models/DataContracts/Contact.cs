namespace MobileApp.Core.Models.DataContracts
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    using Cirrious.CrossCore;

    using MobileApp.Core.ViewModels;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The contact.
    /// </summary>
    public class Contact : ModelBase<ContactViewModel>, IResult
    {
        #region Fields

        [JsonIgnore]
        private List<string> devices;

        [JsonIgnore]
        private Dictionary<string, ContactStatus> statuses;

        [JsonIgnore]
        private int userid;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the mobile_phone
        /// </summary>
        [JsonProperty("callerid")]
        public string Callerid { get; set; }

        /// <summary>
        /// Collection of statuses
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, ContactStatus> Channels
        {
            get
            {
                return this.statuses ?? (this.statuses = new Dictionary<string, ContactStatus>());
            }
        }

        /// <summary>
        /// User devices' names
        /// </summary>
        [JsonProperty("devices")]
        public List<string> Devices
        {
            get
            {
                return this.devices;
            }
            set
            {
                this.devices = value;
                var item = this.Userid;
                if (!this.devices.Contains(item.ToString()))
                {
                    this.devices.Add(item.ToString());
                }
            }
        }

        /// <summary>
        /// Gets or sets the donotdisturb
        /// </summary>
        [JsonProperty("donotdisturb")]
        public bool Dnd { get; set; }

        /// <summary>
        /// Gets or sets the dnd nr
        /// </summary>
        [JsonProperty("dndnr")]
        public string Dndnr { get; set; }

        /// <summary>
        /// Gets or sets the first name
        /// </summary>
        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the last name
        /// </summary>
        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the mobile_phone
        /// </summary>
        [JsonProperty("registered")]
        public bool Registered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether method call was successful
        /// </summary>
        [JsonProperty("result")]
        public bool Result { get; set; }

        /// <summary>
        /// Get status enum
        /// </summary>
        public ContactStatus Status
        {
            get
            {
                if (this.Channels.Any(n => n.Value == ContactStatus.EarlyInbound))
                {
                    return ContactStatus.EarlyInbound;
                }

                if (this.Channels.Any(n => n.Value == ContactStatus.EarlyOutbound))
                {
                    return ContactStatus.EarlyOutbound;
                }

                if (this.Channels.Any(n => n.Value == ContactStatus.Confirmed))
                {
                    return ContactStatus.Confirmed;
                }

                return this.Registered ? ContactStatus.Online : ContactStatus.Offline;
            }
        }

        /// <summary>
        /// Gets or sets the mobile_phone
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string StatusExternal { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [JsonProperty(PropertyName = "userid", Order = 1, Required = Required.Always)]
        public int Userid
        {
            get
            {
                return this.userid;
            }
            set
            {
                this.userid = value;
                if (this.devices != null && !this.devices.Contains(value.ToString()))
                {
                    this.devices.Add(value.ToString());
                }
            }
        }

        /// <summary>
        /// Gets or sets the vm emailaddr
        /// </summary>
        [JsonProperty("vmemailaddr")]
        public string VmEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the vm emailnotify
        /// </summary>
        [JsonProperty("vmemailnotify")]
        public bool VmEmailNotify { get; set; }

        /// <summary>
        /// Gets or sets the vm enabled
        /// </summary>
        [JsonProperty("vmenabled")]
        public bool VmEnabled { get; set; }

        #endregion

        #region Properties

        public override long Key
        {
            get
            {
                return this.Userid;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get contact's full name in propreate format
        /// </summary>
        /// <returns>Contact's name</returns>
        public string GetFullName()
        {
            return string.Format("{0} {1}", this.Firstname.Equals("-") ? string.Empty : this.Firstname, this.Lastname.Equals("-") ? string.Empty : this.Lastname).Trim();
        }

        /// <summary>
        /// Create ViewModel from Model
        /// </summary>
        /// <returns>ViewModel</returns>
        public override ContactViewModel GetViewModel()
        {
            var viewModel = Mvx.IocConstruct<ContactViewModel>();

            viewModel.UpdateFromModel(this);

            return viewModel;
        }

        public override string ToString()
        {
            return string.Format("{0}; state: {1};", this.GetFullName(), this.Status);
        }

        #endregion
    }
}