namespace MobileApp.Android.Utils
{
    using System;
    using System.Collections.Generic;

    using MobileApp.Core.Interfaces;

    using Microsoft.AspNet.SignalR.Client;

    internal class WebSocketsDelegate : IWebSocketConnector
    {
        #region Public Methods and Operators

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

                connection.Start().Wait();

                return new KeyValuePair<HubConnection, Dictionary<string, IHubProxy>>(connection, hubs);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}