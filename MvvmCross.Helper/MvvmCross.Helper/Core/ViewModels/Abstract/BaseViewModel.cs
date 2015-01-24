namespace MvvmCross.Helper.Core.ViewModels.Abstract
{
    using System;

    using Cirrious.MvvmCross.Plugins.Messenger;
    using Cirrious.MvvmCross.ViewModels;

    using MvvmCross.Helper.Core.Interfaces;
    using MvvmCross.Helper.Core.Properties;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    /// <summary>
    /// Most basic logic every view model should obtain
    /// </summary>
    public abstract class BaseViewModel : MvxViewModel, ICleanable
    {
        #region Static Fields

        private static bool isDebug;

        #endregion

        #region Fields

        private readonly ILogger logger;

        private readonly IMvxMessenger messenger;

        private readonly INotificationService notificationService;

        private bool isLoading;

        #endregion

        #region Constructors and Destructors

        protected BaseViewModel(ILogger logger, INotificationService notificationService, IMvxMessenger messenger)
        {
            this.logger = logger;
            this.notificationService = notificationService;
            this.messenger = messenger;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Access to system resources
        /// </summary>
        /// <returns>resource</returns>
        public string BuildVersion
        {
            get
            {
                return Resources.BuildVersionMessage + Resources.BuildVersion;
            }
        }

        /// <summary>
        /// HockeyApp Id
        /// </summary>
        public string HockeyAppAndroid
        {
            get
            {
                return Resources.Debug.Equals("true") ? Resources.HockeyAppDev : Resources.HockeyAppProd;
            }
        }

        /// <summary>
        /// Determent is App in debug mode
        /// </summary>
        public bool IsDebugMode
        {
            get
            {
                return isDebug;
            }

            set
            {
                isDebug = value;
                this.RaisePropertyChanged(() => this.IsDebugMode);
            }
        }

        /// <summary>
        /// ViewModel busy flag
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                this.RaisePropertyChanged(() => this.IsLoading);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Provide internal events
        /// </summary>
        protected IMvxMessenger EventHub
        {
            get
            {
                return this.messenger;
            }
        }

        /// <summary>
        /// Provide logger
        /// </summary>
        protected ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        /// <summary>
        /// Provides notifications
        /// </summary>
        protected INotificationService NotificationService
        {
            get
            {
                return this.notificationService;
            }
        }

        #endregion

        #region Public Methods and Operators

        public abstract void CleansAfter();

        public abstract void MakeMess();

        #endregion

        #region Methods

        /// <summary>
        /// Fires message with exception
        /// </summary>
        /// <param name="exception">some exception</param>
        protected virtual void ErrorAction(Exception exception)
        {
            this.Logger.Error(exception, this);
        }

        #endregion
    }
}