namespace MvvmCross.Helper.Core.Services
{
    using System.Collections.Generic;

    using Microsoft.AspNet.SignalR.Client;

    /// <summary>
    /// Get pair of Connection/Proxy for webSocket client
    /// </summary>
    /// <param name="serviceUrl">Uri to service</param>
    /// <param name="pair">pair of Connection/Proxy to fill</param>
    /// <returns>result</returns>
    public delegate bool CreateWebSocketConnection(string serviceUrl, out KeyValuePair<HubConnection, IHubProxy> pair);
}