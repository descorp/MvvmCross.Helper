namespace MvvmCross.Helper.Core.ViewModels.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Cirrious.MvvmCross.Plugins.Messenger;
    using Cirrious.MvvmCross.ViewModels;

    using MvvmCross.Helper.Core.Interfaces;
    using MvvmCross.Helper.Core.Models.DataContracts;
    using MvvmCross.Helper.Core.Models.Messages;
    using MvvmCross.Helper.Core.Services.Repositories;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    using MvvmCross.Helper.Core.Models.DataContracts;

    /// <summary>
    /// Provides abstract table source collection
    /// </summary>
    /// <typeparam name="TViewModel">View model</typeparam>
    /// <typeparam name="TModel"> model</typeparam>
    public abstract class BaseListViewModel<TViewModel, TModel> : AccountAccessViewModel
        where TModel : IModel<TViewModel> where TViewModel : BaseListItemViewModel<TModel>, ISelectable
    {
        #region Fields

        protected MvxSubscriptionToken ContactToken;

        protected MvxSubscriptionToken SelectionToken;

        private readonly MvxCommand<TViewModel> itemSelectedCommand;

        private readonly IViewModelStorage<TModel, TViewModel> viewModelStorage;

        private CustomObservableCollection<TViewModel> collection = new CustomObservableCollection<TViewModel>();

        private TViewModel selectedItem;

        #endregion

        #region Constructors and Destructors

        protected BaseListViewModel(
            INotificationService message,
            IMvxMessenger events,
            ILogger logger,
            IAccountService accountData,
            IViewModelStorage<TModel, TViewModel> viewModelStorage)
            : base(accountData, events, message, logger)
        {
            this.viewModelStorage = viewModelStorage;
            this.itemSelectedCommand = new MvxCommand<TViewModel>(this.RefreshItemsSelection);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of <see cref="ContactViewModel" />. Get and set
        /// </summary>
        public CustomObservableCollection<TViewModel> Context
        {
            get
            {
                return this.collection;
            }
        }

        /// <summary>
        /// Is collection support selection of several items
        /// </summary>
        public bool IsMultiSelect { get; set; }

        /// <summary>
        /// Gets the item selected command.
        /// </summary>
        public ICommand ItemSelectedCommand
        {
            get
            {
                return this.itemSelectedCommand;
            }
        }

        /// <summary>
        /// Item that selected
        /// </summary>
        public TViewModel SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                if (this.selectedItem.Equals(value))
                {
                    return;
                }

                this.selectedItem = value;
                this.RaisePropertyChanged(() => this.SelectedItem);
            }
        }

        #endregion

        #region Properties

        protected abstract Dictionary<string, object> Filters { get; }

        /// <summary>
        /// Get singleton collection of view models
        /// </summary>
        protected IViewModelStorage<TModel, TViewModel> ViewModelStorage
        {
            get
            {
                return this.viewModelStorage;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void CleansAfter()
        {
            base.CleansAfter();

            this.EventHub.Unsubscribe<Message<TViewModel>>(this.SelectionToken);
            this.EventHub.Unsubscribe<Message<TModel>>(this.ContactToken);
        }

        public virtual void ClearContext()
        {
            this.collection.Clear();
            this.ViewModelStorage.Clear();
        }

        /// <summary>
        /// Get all managed objects here
        /// </summary>
        public override void MakeMess()
        {
            base.MakeMess();

            this.SelectionToken = this.EventHub.Subscribe<Message<TViewModel>>(this.OnSelectionChanged, tag: DateTime.Now.ToString("O"));
            this.ContactToken = this.EventHub.Subscribe<Message<TModel>>(this.OnContactUpdated);
        }

        public override void Start()
        {
            base.Start();

            this.InvokeOnMainThread(
                delegate
                    {
                        this.IsLoading = false;
                        this.RaisePropertyChanged(() => this.Context);
                    });
        }

        #endregion

        #region Methods

        protected void OnSelectionChanged(Message<TViewModel> message)
        {
            if (message == null)
            {
                this.ErrorAction(new ArgumentNullException("message"));
            }
            else
            {
                this.ItemSelectedCommand.Execute(message.Data);
            }
        }

        protected virtual void UpdateCollection()
        {
            this.IsLoading = true;

            var task = this.ViewModelStorage.FilteredList(this.Filters);

            this.collection = new CustomObservableCollection<TViewModel>(task.Result);

            this.InvokeOnMainThread(
                delegate
                    {
                        this.IsLoading = false;
                        this.RaisePropertyChanged(() => this.Context);
                    });
        }

        private void OnContactUpdated(Message<TModel> message)
        {
            if (message == null)
            {
                this.ErrorAction(new ArgumentNullException("message"));
            }
            else
            {
                this.ViewModelStorage.AddOrUpdate(message.Data);
            }
        }

        private void RefreshItemsSelection(TViewModel viewModel)
        {
            if (this.Context.Count <= 0)
            {
                return;
            }

            if (this.IsMultiSelect)
            {
                var item = this.Context.FirstOrDefault(n => n.Equals(viewModel));
                if (item != null)
                {
                    item.IsSelected = !item.IsSelected;
                }
            }
            else
            {
                foreach (var item in this.Context)
                {
                    if (item.Equals(viewModel))
                    {
                        item.IsSelected = !item.IsSelected;
                    }
                    else
                    {
                        item.IsSelected = false;
                    }
                }
            }
        }

        #endregion
    }
}