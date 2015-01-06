namespace MobileApp.Core
{
    #region

    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.ViewModels;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Models.DataContracts;
    using MobileApp.Core.Models.SqliteEntities;
    using MobileApp.Core.Services;
    using MobileApp.Core.Services.AccountService;
    using MobileApp.Core.Services.DataService;
    using MobileApp.Core.Services.HttpService;
    using MobileApp.Core.Services.Repositories;
    using MobileApp.Core.Services.UtilityService;
    using MobileApp.Core.Services.UtilityService.Logger;
    using MobileApp.Core.Services.WebSocketService;
    using MobileApp.Core.ViewModels;

    using PluginLoader = Cirrious.MvvmCross.Plugins.Json.PluginLoader;

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
            PluginLoader.Instance.EnsureLoaded();

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