namespace MvvmCross.Helper.Core.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provide web socket features
    /// </summary>
    public interface IWebSocketService
    {
        /// <summary>
        /// Connect client
        /// </summary>
        /// <param name="greeting">greeting message</param>
        /// <returns>result</returns>
        Task<bool> Connect(string greeting);

        /// <summary>
        /// Unsubscribe
        /// </summary>
        /// <returns>result</returns>
        Task Disconnect();

        /// <summary>
        /// Status of client
        /// </summary>
        /// <returns>is connected</returns>
        bool IsConnected { get; }

        /// <summary>
        /// Send message to other clients
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>result task</returns>
        Task Send(string message);

        /// <summary>
        /// Subscribe on Notify updates
        /// </summary>
        /// <param name="instance">Tenant's name</param>
        /// <returns>result task</returns>
        Task Subscribe(string instance);
    }
}