
#define PORTABLE

namespace MobileApp.iOS.UI.Services
{
    using System;
    using System.Collections.Generic;

    using MobileApp.Core.Interfaces;

    using Microsoft.AspNet.SignalR.Client;
    using Microsoft.AspNet.SignalR.Client.Http;

    /// <summary>
    /// Implementation of Web socket connector
    /// </summary>
    public class SignalRConnector : IWebSocketConnector
    {
        #region Public Methods and Operators

        /// <summary>
        /// Get connection as pair of HubConnection and list of hubs
        /// </summary>
        /// <param name="url">Uri to service</param>
        /// <param name="hubNames">list of hubs to connect</param>
        /// <returns>pair</returns>
        public KeyValuePair<HubConnection, Dictionary<string, IHubProxy>>? GetConnection(string url, params string[] hubNames)
        {
            try
            {
                var connection = new HubConnection(url);
                var hubs = new Dictionary<string, IHubProxy>();

                foreach (string hub in hubNames)
                {
                    IHubProxy proxy = connection.CreateHubProxy(hub);
                    hubs.Add(hub, proxy);
                }

                var http = new DefaultHttpHandler(connection);
                http.PreAuthenticate = false;

                connection.Start().Wait();

                return new KeyValuePair<HubConnection, Dictionary<string, IHubProxy>>(connection, hubs);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion
    }
}