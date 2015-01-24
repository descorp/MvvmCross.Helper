namespace MvvmCross.Helper.Core.Services.DataService
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Cirrious.MvvmCross.Plugins.Messenger;

    using MvvmCross.Helper.Core.Models.DataContracts;
    using MvvmCross.Helper.Core.Models.Messages;
    using MvvmCross.Helper.Core.Services.HttpService;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    using MvvmCross.Helper.Core.Converters;

    #endregion

    /// <summary>
    /// The data service.
    /// </summary>
    public class ContactsService : UpdatableStorageBase<Contact>, IContactService
    {
        #region Fields

        private readonly IMvxMessenger messenger;

        private Dictionary<string, Contact> deviceDictionary;

        private int isBusy;

        private Contact outherWorldDevice;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create new contact's collection
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="messenger">messenger</param>
        public ContactsService(ILogger logger, IMvxMessenger messenger)
            : base(typeof(Contact), logger)
        {
            this.messenger = messenger;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of matches devices and contacts
        /// </summary>
        public Dictionary<string, Contact> DeviceDictionary
        {
            get
            {
                return this.deviceDictionary ?? (this.deviceDictionary = new Dictionary<string, Contact>());
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Check is service busy
        /// </summary>
        protected bool IsBusy
        {
            get
            {
                return this.isBusy > 0;
            }
            set
            {
                if (value)
                {
                    this.isBusy++;
                }
                else
                {
                    this.isBusy--;
                }

                if (this.isBusy == 0)
                {
                    this.messenger.Publish(new SyncMessage(this));
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get contact data by device name
        /// </summary>
        /// <param name="aliases">device's name</param>
        /// <returns>Contact data</returns>
        public Contact FindByAlias(string[] aliases)
        {
            return (from alias in aliases
                    where !string.IsNullOrWhiteSpace(alias)
                    select PhoneNumberHelper.ToComparable(alias)
                    into key
                    select this.DeviceDictionary.ContainsKey(key) ? this.DeviceDictionary[key] : this.FindByUserId(key)).FirstOrDefault(contact => contact != null);
        }

        /// <summary>
        /// Get contact data by user ID
        /// </summary>
        /// <param name="userName">user's ID</param>
        /// <returns>Contact data</returns>
        public Contact FindByUserId(string userName)
        {
            int id;
            return int.TryParse(userName.ToLower(), out id) ? this.Storage.FirstOrDefault(c => c.Userid.Equals(id)) : null;
        }

        /// <summary>
        /// Set some specific contact to dictionary
        /// </summary>
        /// <param name="contact">some specific contact</param>
        public void SetSpecificContact(Contact contact)
        {
            this.outherWorldDevice = contact;
            foreach (var device in contact.Devices)
            {
                this.deviceDictionary[device] = contact;
            }
        }

        /// <summary>
        /// Sync collection
        /// </summary>
        /// <returns></returns>
        public override void SynchronizeRepository()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            
            ApiInteraction.Create(ApiMethodType.GetRegisteredDevices, this.DevicesUpdate, this.ErrorAction);
        }

        #endregion

        #region Methods

        protected override void UpdateStorage(List<Contact> list)
        {
            base.UpdateStorage(list);

            foreach (var contact in this.Storage)
            {
                foreach (var key in contact.Devices.Select(PhoneNumberHelper.ToComparable).Where(key => !this.DeviceDictionary.ContainsKey(key)))
                {
                    this.DeviceDictionary.Add(key, contact);
                }
            }
        }

        private void DevicesUpdate(object obj)
        {
            var list = Responce<string>.ProcessListResponce(obj, this.Logger, this);
            var b = list != null;

            foreach (var contact in this.Storage)
            {
                contact.Registered = b && list.Select(n => Convert.ToInt32(n)).Contains(contact.Userid);
            }

            this.IsBusy = false;
        }

        private void ErrorAction(Exception obj)
        {
            this.IsBusy = false;
        }

        #endregion
    }
}