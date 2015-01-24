namespace MvvmCross.Helper.Core.Services.AccountService
{
    #region

    using System;
    using System.Globalization;

    using Cirrious.MvvmCross.Plugins.Messenger;

    using MvvmCross.Helper.Core.Interfaces;
    using MvvmCross.Helper.Core.Models;
    using MvvmCross.Helper.Core.Models.DataContracts;
    using MvvmCross.Helper.Core.Models.Messages;
    using MvvmCross.Helper.Core.Properties;
    using MvvmCross.Helper.Core.Services.HttpService;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    using Newtonsoft.Json;

    using Responce = MvvmCross.Helper.Core.Services.HttpService.Responce;

    #endregion

    /// <summary>
    /// The auth service.
    /// </summary>
    public class UserDataService : IAccountService
    {
        #region Fields

        private readonly ILogger logger;

        private readonly IMvxMessenger messenger;

        private Action<Exception> error;

        private bool isAuthorized = true;

        private bool isValid;

        private Action<Responce> success;

        private bool isTransferAvailable;

        private readonly MvxSubscriptionToken contactToken;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataService" /> class.
        /// </summary>
        public UserDataService(ILogger logger, IMvxMessenger messenger)
        {
            this.logger = logger;
            this.messenger = messenger;

            this.contactToken = this.messenger.Subscribe<Message<Contact>>(this.OnContactReceived);
        }

        private void OnContactReceived(Message<Contact> message)
        {
            if (message == null)
            {
                return;
            }

            if (message.Data == null || !message.Data.Equals(this.UserData))
            {
                return;
            }

            this.UserData.Registered = message.Data.Registered;
            this.messenger.Publish(new Message<ContactStatus>(this, this.UserData.Status));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        public CredentialsModel? Credentials { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether is authorized.
        /// </summary>
        public bool IsAuthorized
        {
            get
            {
                return this.isAuthorized;
            }
            set
            {
                this.isAuthorized = value;
            }
        }

        /// <summary>
        /// Gets or sets the user data.
        /// </summary>
        public Contact UserData { get; set; }

        /// <summary>
        /// Get or set transfer availability
        /// </summary>
        public bool IsTransferAvailable
        {
            get
            {
                return this.isTransferAvailable;
            }
            set
            {
                this.isTransferAvailable = value;
                this.messenger.Publish(new TransferMessage(this));
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The authorize.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userPin">
        /// The user pin.
        /// </param>
        /// <param name="externalSuccess">
        /// The success.
        /// </param>
        /// <param name="externalError">
        /// The error.
        /// </param>
        public void Authorize(string instance, string userId, string userPin, Action<Responce> externalSuccess, Action<Exception> externalError = null)
        {
            this.error = externalError;
            this.success = externalSuccess;

            var valid = this.ValidateAuthData(instance, userId, userPin);

            if (this.isValid)
            {
                var cred = this.Credentials.Value;
                ApiInteraction.Create(
                    ApiMethodType.GetAuth,
                    this.OnSuccess,
                    this.OnError,
                    new { INSTANCE = cred.Instance, PIN = cred.Pin, USERID = cred.UserId, VERSION = Resources.BuildVersion });
            }
            else
            {
                externalSuccess(new Responce { Result = false, Data = valid });
            }
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            this.Credentials = null;
            this.UserData = null;
        }

        #endregion

        #region Methods

        private void OnError(Exception webError)
        {
            var action = this.error;
            if (action != null)
            {
                action(webError);
            }
        }

        private void OnSuccess(object data)
        {
            var result = data as Responce ?? JsonConvert.DeserializeObject<Responce>(data.ToString());
            if (!result.Result)
            {
                var cred = this.Credentials.Value;
                this.logger.Debug(
                    string.Format(
                        "{0} - Loggin failed {1}@{2} - {4} : {3}",
                        this.GetType().Name,
                        cred.UserId,
                        cred.Instance,
                        result.Message,
                        result.ErrorCode), this);

                this.success(new Responce { Result = false, Message = result.Message});
            }
            else
            {
                this.IsAuthorized = true;
                this.logger.Debug(string.Format("{0} - Logged : {1}@{2}", this.GetType().Name, this.Credentials.Value.UserId, this.Credentials.Value.Instance), this);

                var contact = result.Data.ToString();
                this.UserData = JsonConvert.DeserializeObject<Contact>(contact, new JsonSerializerSettings());

                ApiInteraction.Authentication = this.Credentials.Value;

                this.success(Responce.Ok);
            }
        }

        /// <summary>
        /// The validate auth data.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <param name="userPin">
        /// The user pin.
        /// </param>
        /// <returns>
        /// The <see cref="ValidationReport" />.
        /// </returns>
        private ValidationReport ValidateAuthData(string instance, string userId, string userPin)
        {
            var validation = new bool[3];

            if (!string.IsNullOrEmpty(instance) && !string.IsNullOrWhiteSpace(instance))
            {
                validation[0] = true;
            }

            int id;
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrWhiteSpace(userId))
            {
                validation[1] = true;
            }

            if (int.TryParse(userId, NumberStyles.Integer, CultureInfo.CurrentCulture, out id))
            {
                validation[1] = true;
            }

            if (!string.IsNullOrEmpty(userPin) && !string.IsNullOrWhiteSpace(userPin))
            {
                validation[2] = true;
            }

            this.isValid = validation[0] && validation[1] && validation[2];
            if (this.isValid)
            {
                this.Credentials = new CredentialsModel { Instance = instance, UserId = userId, Pin = userPin };
            }

            return new ValidationReport(validation);
        }

        #endregion
    }
}