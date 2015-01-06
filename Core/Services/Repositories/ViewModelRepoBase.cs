namespace MobileApp.Core.Services.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Models.DataContracts;
    using MobileApp.Core.Services.UtilityService.Logger;
    using MobileApp.Core.ViewModels.Abstract;

    /// <summary>
    /// Abstract view model repository
    /// </summary>
    /// <typeparam name="TModel">model to store</typeparam>
    /// <typeparam name="TViewModel">view model to store</typeparam>
    public abstract class ViewModelRepoBase<TModel, TViewModel> : IViewModelStorage<TModel, TViewModel>
        where TViewModel : BaseListItemViewModel<TModel> where TModel : IModel<TViewModel>
    {
        #region Fields

        protected readonly object Locker = new object();

        private readonly ILogger logger;

        private Dictionary<long, TViewModel> collection;

        #endregion

        #region Constructors and Destructors

        protected ViewModelRepoBase(ILogger logger)
        {
            this.logger = logger;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Storage of view models
        /// </summary>
        public Dictionary<long, TViewModel> Collection
        {
            get
            {
                return this.collection ?? (this.collection = new Dictionary<long, TViewModel>());
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Change or create new ViewModel
        /// </summary>
        /// <param name="model">contact to update</param>
        public void AddOrUpdate(TModel model)
        {
            this.logger.Debug(string.Format("{0} - Model update : {1}", this.GetType().Name, model), this);
            if (this.Collection.ContainsKey(model.Key))
            {
                this.UpdateViewModel(model);
            }
            else
            {
                lock (this.Locker)
                {
                    this.Collection.Add(model.Key, model.GetViewModel());
                }
            }
        }

        /// <summary>
        /// Clear data
        /// </summary>
        public void Clear()
        {
            var viewModels = this.collection;
            if (viewModels != null)
            {
                viewModels.Clear();
            }

            this.logger.Debug(string.Format("{0} - Collection erased", this.GetType().Name), this);
        }

        /// <summary>
        /// Get filtered list on view models
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>resulting collection</returns>
        public abstract Task<List<TViewModel>> FilteredList(Dictionary<string, object> parameters);

        /// <summary>
        /// Update number of contacts
        /// </summary>
        /// <param name="models">collection to update</param>
        public void FullUpdate(IEnumerable<TModel> models)
        {
            this.Clear();

            foreach (var model in models)
            {
                this.AddOrUpdate(model);
            }

            this.logger.Debug(string.Format("{0} - Collection updated : {1}", this.GetType().Name, models.Count()), this);
        }

        /// <summary>
        /// Remove from collection
        /// </summary>
        /// <param name="model">Model to remove</param>
        public virtual void Remove(TModel model)
        {
            this.Remove(model.Key);
        }

        /// <summary>
        /// Remove from collection
        /// </summary>
        /// <param name="key">Model's key to remove</param>
        public void Remove(long key)
        {
            lock (this.Locker)
            {
                this.Collection.Remove(key);
            }
        }

        #endregion

        #region Methods

        protected virtual void UpdateViewModel(TModel model)
        {
            this.Collection[model.Key].UpdateFromModel(model);
        }

        #endregion
    }
}