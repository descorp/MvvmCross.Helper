namespace MobileApp.Android.Binding
{
    using System;
    using System.Windows.Input;

    using Cirrious.MvvmCross.Binding;
    using Cirrious.MvvmCross.Binding.Droid.Target;
    using Cirrious.MvvmCross.Binding.Droid.Views;

    using global::Android.Widget;

    /// <summary>
    /// The spinner item selected binding.
    /// </summary>
    public class SpinnerItemSelectedBinding : MvxAndroidTargetBinding
    {
        #region Fields

        private ICommand command;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinnerItemSelectedBinding"/> class.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        public SpinnerItemSelectedBinding(MvxSpinner view)
            : base(view)
        {
            view.ItemSelected += this.ViewOnItemSelected;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the default mode.
        /// </summary>
        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.OneWay;
            }
        }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public override Type TargetType
        {
            get
            {
                return typeof(ICommand);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the view.
        /// </summary>
        protected MvxSpinner View
        {
            get
            {
                return (MvxSpinner)this.Target;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public override void SetValue(object value)
        {
            this.command = value as ICommand;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="isDisposing">
        /// The is disposing.
        /// </param>
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                MvxSpinner view = this.View;
                if (view != null)
                {
                    view.ItemSelected -= this.ViewOnItemSelected;
                }
            }

            base.Dispose(isDisposing);
        }

        /// <summary>
        /// The set value impl.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        protected override void SetValueImpl(object target, object value)
        {
            this.command = value as ICommand;
        }

        private void ViewOnItemSelected(object sender, AdapterView.ItemSelectedEventArgs args)
        {
            if (this.command == null)
            {
                return;
            }

            if (!this.command.CanExecute(null))
            {
                return;
            }

            this.command.Execute(null);
        }

        #endregion
    }
}