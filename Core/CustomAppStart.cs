namespace MobileApp.Core
{
    using Cirrious.MvvmCross.ViewModels;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Models;
    using MobileApp.Core.Services;
    using MobileApp.Core.ViewModels;

    /// <summary>
    /// Custom app start
    /// </summary>
    public class CustomAppStart : MvxNavigatingObject, IMvxAppStart
    {
        private readonly ICredentialsStorageService service;

        private readonly ISessionContainer session;

        private readonly IAccountService accountService;

        /// <summary>
        /// Create new custom app start
        /// </summary>
        /// <param name="service">credentials loader</param>
        /// <param name="session">current session</param>
        /// <param name="accountService">auth access</param>
        public CustomAppStart(ICredentialsStorageService service, ISessionContainer session, IAccountService accountService)
        {
            this.service = service;
            this.session = session;
            this.accountService = accountService;
        }

        void IMvxAppStart.Start(object hint)
        {
            string domain;
            var credentials = this.service.LoadCredentials(out domain);

            if (credentials.IsValid() && !string.IsNullOrWhiteSpace(domain))
            {
                this.session.CustomDomain = domain;
                this.accountService.Authorize(
                    credentials.Instance,
                    credentials.UserId,
                    credentials.Pin,
                    (result) =>
                        {
                            if (result.Result)
                            {
                                this.ShowViewModel<HomeViewModel>();
                            }
                            else
                            {
                                this.ShowViewModel<AuthViewModel>(new
                                {
                                    BreakoutCause = result.Message,
                                    Instance = credentials.Instance,
                                    UserId = credentials.UserId,
                                    Pin = credentials.Pin
                                });
                            }
                        },
                    (exc) =>
                    this.ShowViewModel<AuthViewModel>(
                        new
                            {
                                BreakoutCause = "Some error appeares during logging in",
                                Instance = credentials.Instance,
                                UserId = credentials.UserId,
                                Pin = credentials.Pin
                            }));
            }
            else
            {
                this.ShowViewModel<AuthViewModel>();
            }
        }
    }
}