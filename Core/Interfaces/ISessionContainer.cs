namespace MobileApp.Core.Interfaces
{
    using System.Net;

    public interface ISessionContainer
    {
        CookieContainer Cookies { get; set; }

        /// <summary>
        /// The domain.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Chose domain manualy
        /// </summary>
        string CustomDomain { get; set; }

        /// <summary>
        /// The clear cookies.
        /// </summary>
        void ClearCookies();

        /// <summary>
        /// The service.
        /// </summary>
        string ServiceUrl { get; }
    }
}