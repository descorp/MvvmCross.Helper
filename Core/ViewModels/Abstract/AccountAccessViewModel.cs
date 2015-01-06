namespace MobileApp.Core.ViewModels.Abstract
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.Plugins.Messenger;
    using Cirrious.MvvmCross.Plugins.WebBrowser;
    using Cirrious.MvvmCross.ViewModels;

    using FESS.MobileApp.Core.Interfaces;
    using FESS.MobileApp.Core.Models;
    using FESS.MobileApp.Core.Models.Messages;
    using FESS.MobileApp.Core.Properties;
    using FESS.MobileApp.Core.Services.HttpService;
    using FESS.MobileApp.Core.Services.UtilityService.Logger;

    #endregion

    /// <summary>
    /// ViewModel with access to web sockets
    /// </summary>
    public abstract class AccountAccessViewModel : BaseViewModel
    {
        #region Fields

        private readonly IAccountService accountData;

        private readonly MvxCommand sendReportCommand;

        private readonly MvxCommand openHelpCommand;

        private MvxSubscriptionToken userStatusToken;

        #endregion

        #region Constructors and Destructors

        protected AccountAccessViewModel(IAccountService accountData, IMvxMessenger events, INotificationService message, ILogger logger)
            : base(logger, message, events)
        {
            this.accountData = accountData;

            sendReportCommand = new MvxCommand(
                () =>
                    {
                        var model = this.accountData.Credentials;
                        var user = model != null ? string.Format("{0}@{1}", model.Value.UserId, model.Value.Instance) : "not login";

                        var report = this.Logger.CreateReport();

                        ApiInteraction.Create(
                            ApiMethodType.PostReport,
                            o =>
                                {
                                    this.IsLoading = false;
                                    this.NotificationService.ShowMessageBox("Sent", false);
                                },
                            o => { this.IsLoading = false; },
                            new Dictionary<string, object> { { "BODY", report }, { "USER", user } });
                    });

            openHelpCommand = new MvxCommand(
                () =>
                    {
                        var task = Mvx.Resolve<IMvxWebBrowserTask>();
                        try
                        {
                            task.ShowWebPage(Resources.HelpPageUrl);
                        }
                        catch (Exception exc)
                        {
                            this.NotificationService.ShowMessageBox(exc.Message, false);
                        }
                    });
        }

        /// <summary>
        /// Get all managed objects here
        /// </summary>
        public override void MakeMess()
        {
            this.userStatusToken = this.EventHub.Subscribe<Message<ContactStatus>>(this.UserDataChanged);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the auth service.
        /// </summary>
        protected IAccountService Account
        {
            get
            {
                return this.accountData;
            }
        }

        /// <summary>
        /// Free all managed objects here
        /// </summary>
        public override void CleansAfter()
        {
            var token = this.userStatusToken;
            if (token != null)
            {
                this.EventHub.Unsubscribe<Message<ContactStatus>>(token);
            }
        }

        /// <summary>
        /// is user in DND state
        /// </summary>
        public bool IsDnd
        {
            get
            {
                return this.Account.UserData.Dnd;
            }
            set
            {
                this.Account.UserData.Dnd = value;
                this.RaisePropertyChanged(() => this.IsDnd);
                this.RaisePropertyChanged(() => this.UserStatus);
            }
        }

        /// <summary>
        /// Gets the open help command.
        /// </summary>
        public ICommand OpenHelpCommand
        {
            get
            {
                return this.openHelpCommand;
            }
        }

        /// <summary>
        /// Send crush report
        /// </summary>
        public ICommand SendReportCommand
        {
            get
            {
                return this.sendReportCommand;
            }
        }

        /// <summary>
        /// Gets the user status.
        /// </summary>
        public ContactStatus UserStatus
        {
            get
            {
                var contactStatus = this.Account.UserData.Dnd ? ContactStatus.Dnd : this.Account.UserData.Status;
                return contactStatus;
            }
        }

        #endregion

        #region Methods

        protected virtual void UserDataChanged(Message<ContactStatus> message)
        {
            try
            {
                if (message == null)
                {
                    throw new ArgumentNullException("message");
                }

                this.RaisePropertyChanged(() => this.UserStatus);
                this.RaisePropertyChanged(() => this.IsDnd);
            }
            catch (Exception e)
            {
                this.NotificationService.ShowMessageBox("Error", true);
            }
        }

        #endregion
    }
}