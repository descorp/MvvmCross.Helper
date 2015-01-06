namespace MobileApp.Core.Models.Messages
{
    using System;

    using Cirrious.MvvmCross.Plugins.Messenger;

    public class Message<T> : MvxMessage
    {
        private int hash;

        #region Constructors and Destructors

        /// <summary>
        /// Create new BLF notification
        /// </summary>
        /// <param name="sender">where message came from</param>
        public Message(object sender )
            : this(sender, default(T))
        {
            this.hash = DateTime.Now.ToString("O").GetHashCode();
        }

        /// <summary>
        /// Create new BLF notification
        /// </summary>
        /// <param name="sender">where message came from</param>
        /// <param name="args">the message</param>
        public Message(object sender, T args )
            : base(sender)
        {
            this.Data = args;
            this.hash = DateTime.Now.ToString("O").GetHashCode();
        }

        /// <summary>
        /// BLF info
        /// </summary>
        public T Data { get; private set;}

        public int Hash
        {
            get
            {
                return this.hash;
            }
        }

        #endregion
    }

    internal class SyncMessage : MvxMessage
    {
        /// <summary>
        /// Create new BLF notification
        /// </summary>
        /// <param name="sender">where message came from</param>
        public SyncMessage(object sender)
            : base(sender)
        {
        }
    }
}