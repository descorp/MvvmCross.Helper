namespace MobileApp.Core.ViewModels.Abstract
{
    using System;
    using System.Windows.Input;

    using Cirrious.MvvmCross.Plugins.Messenger;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Services.UtilityService.Logger;

    /// <summary>
    /// Logic of List's items
    /// </summary>
    public abstract class BaseListItemViewModel<TModel> : BaseViewModel, ISelectable
    {
        #region Fields

        private bool isSelected;

        #endregion

        #region Constructors and Destructors

        protected BaseListItemViewModel(IMvxMessenger events, INotificationService message, ILogger logger)
            : base(logger, message, events)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Helps to authenticate object ()
        /// </summary>
        public abstract long Id { get; }

        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                this.isSelected = value;
                this.UpdateSelectedState(value);
                this.RaisePropertyChanged(() => this.IsSelected);
            }
        }

        /// <summary>
        /// Gets the open close command.
        /// </summary>
        public abstract ICommand SelectionCommand { get; }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            return obj != null && this.GetHashCode().Equals(obj.GetHashCode());
        }

        /// <summary>
        /// Get hash code
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override void CleansAfter()
        {
        }

        public override void MakeMess()
        {
        }

        public override string ToString()
        {
            return string.Format("{0} - selected: {1};", this.Id, this.IsSelected);
        }

        /// <summary>
        /// Convert Update fields from Contact
        /// </summary>
        /// <param name="model">External contact</param>
        public abstract void UpdateFromModel(TModel model);

        #endregion

        #region Methods

        /// <summary>
        /// Update UI in implementation
        /// </summary>
        /// <param name="selection">is selected</param>
        protected abstract void UpdateSelectedState(bool selection);

        #endregion
    }
}