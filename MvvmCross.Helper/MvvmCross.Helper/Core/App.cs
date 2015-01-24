namespace MvvmCross.Helper.Core
{
    #region

    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.ViewModels;

    using MvvmCross.Helper.Core;
    using MvvmCross.Helper.Core.Interfaces;
    using MvvmCross.Helper.Core.Models.SqliteEntities;
    using MvvmCross.Helper.Core.Services;
    using MvvmCross.Helper.Core.Services.AccountService;
    using MvvmCross.Helper.Core.Services.HttpService;
    using MvvmCross.Helper.Core.Services.StorageService;
    using MvvmCross.Helper.Core.Services.StorageService.Credentails;
    using MvvmCross.Helper.Core.Services.UtilityService;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;
    using MvvmCross.Helper.Core.Services.WebSocketService;

    #endregion

    /// <summary>
    /// Core part of App class. Register ViewModel classes and interfaces
    /// </summary>
    public class App : MvxApplication
    {
        #region Public Methods and Operators

        /// <summary>
        /// Initialize App and register ViewModels. Here <see cref="ContactListViewModel" />
        /// </summary>
        public override void Initialize()
        {
            var errorLogWriterImpl = new DebugLogWriterImpl();
            Mvx.RegisterSingleton<ILogWriter<SqliteLogItem>>(errorLogWriterImpl);
            Mvx.RegisterSingleton<ISqliteTableService<SqliteLogItem>>(errorLogWriterImpl);
			
            Mvx.ConstructAndRegisterSingleton<ISessionContainer, SessionContainer>();
            Mvx.RegisterType<ILogger, Logger>();
			
            var errorApplicationObject = new NotificationService();
            Mvx.RegisterSingleton<INotificationService>(errorApplicationObject);
            Mvx.RegisterSingleton<IMessageSource>(errorApplicationObject);
			
            Mvx.RegisterType<ICredentialsStorageService, CredentialsStorageService>();
            Mvx.ConstructAndRegisterSingleton<IAccountService, UserDataService>();
            Mvx.RegisterType<IMvxAppStart, CustomAppStart>();
            Mvx.RegisterType<INavigateMyselfService, NavigateMyselfService>();
            Mvx.ConstructAndRegisterSingleton<IWebSocketService, WebSocketService>();       
        }

        #endregion
    }
}