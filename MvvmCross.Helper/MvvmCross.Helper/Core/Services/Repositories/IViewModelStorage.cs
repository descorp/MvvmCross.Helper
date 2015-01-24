namespace MvvmCross.Helper.Core.Services.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Cirrious.MvvmCross.ViewModels;

    using MvvmCross.Helper.Core.Models.DataContracts;

    /// <summary>
    /// Get access to storage of contacts' view models
    /// </summary>
    public interface IViewModelStorage<in TModel, TViewModel>
        where TModel : IModel<TViewModel> where TViewModel : MvxViewModel
    {
        #region Public Methods and Operators

        /// <summary>
        /// Change or create new contact
        /// </summary>
        /// <param name="model">contact to update</param>
        void AddOrUpdate(TModel model);

        /// <summary>
        /// Remove from collection
        /// </summary>
        /// <param name="model"></param>
        void Remove(TModel model);

        /// <summary>
        /// Delete item from collection using key
        /// </summary>
        /// <param name="key">Int64 key for Model/ViewModel</param>
        void Remove(long key);

        /// <summary>
        /// Get filtered list on view models
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>resulting collection</returns>
        Task<List<TViewModel>> FilteredList(Dictionary<string, object> parameters);

        /// <summary>
        /// Update number of contacts
        /// </summary>
        /// <param name="collection">collection to update</param>
        void FullUpdate(IEnumerable<TModel> collection);

        /// <summary>
        /// Clear data from collection
        /// </summary>
        void Clear();

        #endregion
    }
}