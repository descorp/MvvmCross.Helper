namespace MvvmCross.Helper.Core.ViewModels.Abstract
{
    #region

    using System.Collections.Generic;
    using System.Windows.Input;

    using Cirrious.MvvmCross.Plugins.Messenger;
    using Cirrious.MvvmCross.ViewModels;

    using MvvmCross.Helper.Core.Services.AccountService;
    using MvvmCross.Helper.Core.Services.HttpService;
    using MvvmCross.Helper.Core.Services.NotifiactionService;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    #endregion

    /// <summary>
    /// ViewModel with access to web sockets
    /// </summary>
    public abstract class AccountAccessViewModel : BaseViewModel
    {
        #region Fields

        private readonly IAccountService accountData;

        private readonly MvxCommand sendReportCommand;

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
        /// Send crush report
        /// </summary>
        public ICommand SendReportCommand
        {
            get
            {
                return this.sendReportCommand;
            }
        }

        #endregion
    }
}