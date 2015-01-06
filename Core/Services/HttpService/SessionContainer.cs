namespace MobileApp.Core.Services.HttpService
{
    using System.Net;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Properties;

    internal class SessionContainer : ISessionContainer
    {
        /// <summary>
        /// Contain cookies
        /// </summary>
        private CookieContainer cookies;

        private string customDomain;

        /// <summary>
        /// Get cookies
        /// </summary>
        public CookieContainer Cookies
        {
            get
            {
                return this.cookies ?? (this.cookies = new CookieContainer());
            }

            set
            {
                this.cookies = value;
            }
        }

        /// <summary>
        /// The service.
        /// </summary>
        public string ServiceUrl
        {
            get
            {
                return this.Url + Resources.ServiceUrl;
            }
        }

        /// <summary>
        /// The domain.
        /// </summary>
        public string Url
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.CustomDomain))
                    return string.Concat(
                        (Resources.Debug.Equals("true") ? Resources.MediationFirmTelSandboxUrl : Resources.MediationProductionUrl),
                        ":",
                        Resources.UrlPort);

                return this.CustomDomain;
            }
        }

        /// <summary>
        /// Chose domain manualy
        /// </summary>
        public string CustomDomain
        {
            get
            {
                return this.customDomain;
            }
            set
            {
                this.customDomain = value.Contains(Resources.UrlPort) ? value : string.Concat(value, ":", Resources.UrlPort);
            }
        }

        /// <summary>
        /// The clear cookies.
        /// </summary>
        public void ClearCookies()
        {
            this.cookies = new CookieContainer();
        }
    }
}