namespace MvvmCross.Helper.Core.Models.DataContracts
{
    using System;

    using Cirrious.MvvmCross.ViewModels;

    public abstract class ModelBase<TViewModel> : IModel<TViewModel>
        where TViewModel : MvxViewModel
    {
        #region Public Properties

        public abstract long Key { get; }

        #endregion

        #region Public Methods and Operators

        public new virtual bool Equals(object obj)
        {
            var other = obj as ModelBase<TViewModel>;
            return other != null && other.GetHashCode().Equals(this.GetHashCode());
        }

        public override int GetHashCode()
        {
            int hashCode = Convert.ToInt32(this.Key);
            return hashCode;
        }

        /// <summary>
        /// Create ViewModel from Model
        /// </summary>
        /// <returns>ViewModel</returns>
        public abstract TViewModel GetViewModel();

        #endregion
    }
}