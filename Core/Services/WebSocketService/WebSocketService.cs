namespace MobileApp.Core.Services.WebSocketService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.Plugins.Messenger;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Models.Messages;
    using MobileApp.Core.Models.WebSockets;
    using MobileApp.Core.Services.UtilityService.Logger;

    using Microsoft.AspNet.SignalR.Client;

    public delegate KeyValuePair<HubConnection, Dictionary<string, IHubProxy>>? GetConnection(string url, params string[] hubNames);

    /// <summary>
    /// Provide web socket features
    /// </summary>
    public class WebSocketService : IWebSocketService
    {
        #region Fields

        private readonly ISessionContainer session;

        private readonly IMvxMessenger messanger;

        private HubConnection connection;

        private IHubProxy proxy;

        private Task instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create new web socket client
        /// </summary>
        public WebSocketService(ISessionContainer session, IMvxMessenger messanger)
        {
            this.session = session;
            this.messanger = messanger;
            this.connection = new HubConnection(session.Url);
            this.proxy = this.connection.CreateHubProxy("BLF");
        }

        #endregion

        #region Public Properties

        public static CreateWebSocketConnection ConnectDelegate { get; set; }

        /// <summary>
        /// Make remote connection
        /// </summary>
        public IWebSocketConnector Connector
        {
            get
            {
                return Mvx.Resolve<IWebSocketConnector>();
            }
        }

        /// <summary>
        /// Status of client
        /// </summary>
        /// <returns>is connected</returns>
        public bool IsConnected
        {
            get
            {
                var hubConnection = this.connection;
                return hubConnection != null && hubConnection.State == ConnectionState.Connected;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Connect client
        /// </summary>
        /// <returns>result</returns>
        public async Task<bool> Connect(string greeting)
        {
            try
            {
                if (this.Connector != null)
                {
                    KeyValuePair<HubConnection, IHubProxy> valuePair;
                    try
                    {
                        var connection = new HubConnection(this.session.Url);
                        var proxy = connection.CreateHubProxy("BLF");

                        await connection.Start();

                        valuePair = new KeyValuePair<HubConnection, IHubProxy>(connection, proxy);
                    }
                    catch (Exception exc)
                    {
                        var keyValuePair = this.Connector.GetConnection(this.session.Url, "BLF");
                        if (!keyValuePair.HasValue && !ConnectDelegate(this.session.Url, out valuePair))
                        {
                            Mvx.Resolve<ILogger>().Error(exc, this);
                        }
                    }

                    var pair = valuePair;
                    this.connection = pair.Key;
                    this.proxy = pair.Value;
                    
					// Init all Methods here
                    this.proxy.On("NotifyMessage", (string message) => this.messanger.Publish(new Message<string>(this)));

                    await this.Send("Connected " + greeting);
                    return true;
                }
            }
            catch (Exception exc)
            {
                Mvx.Resolve<ILogger>().Error(exc, this);
            }

            return false;
        }

        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task Disconnect()
        {
            if (instance != null)
            {
                this.instance = this.proxy.Invoke("UnsubscribeOnInstanceUpdates", instance);
            }

            this.connection.Stop();
        }

        /// <summary>
        /// Send message to other clients
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>result task</returns>
        public Task Send(string message)
        {
            return this.proxy.Invoke("Send", message);
        }

        /// <summary>
        /// Subscribe on Notify updates
        /// </summary>
        /// <param name="instance">Tenant's name</param>
        /// <returns>result task</returns>
        public Task Subscribe(string instance)
        {
            return !string.IsNullOrWhiteSpace(instance) ? this.proxy.Invoke("SubscribeOnInstanceUpdates", instance) : null;
        }

        #endregion
    }
}