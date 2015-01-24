namespace MvvmCross.Helper.Core.Models.DataContracts
{
    using Cirrious.MvvmCross.ViewModels;

    /// <summary>
    /// Represent model logic
    /// </summary>
    /// <typeparam name="TViewModel">Type of ViewModel</typeparam>
    public interface IModel<out TViewModel> where TViewModel : MvxViewModel
    {
        /// <summary>
        /// Convert model into ViewModel
        /// </summary>
        /// <returns>ViewModel</returns>
        TViewModel GetViewModel();

        /// <summary>
        /// Unique key
        /// </summary>
        long Key { get; }
    }
}