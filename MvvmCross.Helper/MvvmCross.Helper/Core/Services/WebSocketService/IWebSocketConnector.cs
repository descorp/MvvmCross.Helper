namespace MvvmCross.Helper.Core.Interfaces
{
    using System.Collections.Generic;

    using Microsoft.AspNet.SignalR.Client;

    /// <summary>
    /// Delegate web socket connection logic
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
        KeyValuePair<HubConnection, Dictionary<string, IHubProxy>>? GetConnection(string url, IEnumerable<string> hubNames);

        #endregion
    }
}