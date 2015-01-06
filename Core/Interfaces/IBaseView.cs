namespace MobileApp.Core.Interfaces
{
    #region

    using Cirrious.MvvmCross.Views;

    using MobileApp.Core.ViewModels.Abstract;

    #endregion

    /// <summary>
    /// The BaseView interface.
    /// </summary>
    /// <typeparam name="TViewModel">
    /// TViewModel object
    /// </typeparam>
    public interface IBaseView<TViewModel> : IMvxView
        where TViewModel : AccountAccessViewModel
    {
        // just a marker interface
    }
}