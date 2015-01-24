namespace MvvmCross.Helper.Core.Services.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MvvmCross.Helper.Core.Interfaces;
    using MvvmCross.Helper.Core.Services.HttpService;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    public abstract class UpdatableStorageBase<T> : IUpdatable<T>
    {
        #region Fields

        protected bool IsSync = false;

        protected Action<Responce> SuccessAction;

        protected IList<T> Storage;

        protected Action<IList<T>> SuccessUpdateCallback;

        protected Type Type;

        private readonly ILogger logger;

        private TaskCompletionSource<IList<T>> taskCompletionSource;

        #endregion

        #region Constructors and Destructors

        protected UpdatableStorageBase(Type type, ILogger logger)
        {
            this.Type = type;
            this.logger = logger;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Logger
        /// </summary>
        public ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        /// <summary>
        /// Gets the contacts.
        /// </summary>
        public virtual IList<T> Snapshot
        {
            get
            {
                var collection = this.Storage ?? new List<T>();
                return collection;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Sync collection
        /// </summary>
        /// <returns></returns> 
        public abstract void SynchronizeRepository();

        /// <summary>
        /// The update contacts.
        /// </summary>
        /// <param name="successAction">
        /// The success action.
        /// </param>
        public void Refresh(Action<Responce> successAction)
        {
            this.SuccessAction = successAction;
            ApiInteraction.Get(this.Success, this.Error, this.Type);
        }

        #endregion

        #region Methods

        protected virtual void Error(Exception exc)
        {
            this.SuccessAction(new Responce { Result = false, Message = exc.Message });

            var callback = this.SuccessUpdateCallback;
            if (callback != null)
            {
                this.taskCompletionSource.SetException(exc);
            }
            else
            {
                this.Logger.Error(exc, this);
            }
        }   

        protected void Success(object obj)
        {
            var list = Responce<T>.ProcessListResponce(obj, this.Logger, this);

            if (list != null)
            {
                this.UpdateStorage(list);
                this.SendCallback(list, Responce.Ok);
            }
            else
            {
                this.SendCallback(null, Responce.Bad);
            }
        }

        protected virtual void UpdateStorage(List<T> list)
        {
            this.Storage = list;
        }

        private void SendCallback(List<T> list, Responce responce)
        {
            if (this.IsSync)
            {
                if (list == null)
                {
                    this.taskCompletionSource.SetCanceled();
                }
                else
                {
                    this.taskCompletionSource.SetResult(list);
                }
            }
            else
            {
                this.SuccessAction(responce);
            }
        }

        #endregion
    }
}