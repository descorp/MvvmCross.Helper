namespace MvvmCross.Helper.Core.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.AspNet.SignalR.Client;

    /// <summary>
    /// Implementation of Web socket connector
    /// </summary>
    public interface IWebSocketConnector
    {
        #region Public Methods and Operators

        /// <summary>
        /// Get connection as pair of HubConnection and list of hubs
        /// </summary>
        /// <param name="url">Uri to service</param>
        /// <param name="hubNames">list of hubs to connect</param>
        /// <returns>pair</returns>
        KeyValuePair<HubConnection, Dictionary<string, IHubProxy>>? GetConnection(string url,params string[] hubNames);

        #endregion


    }
}