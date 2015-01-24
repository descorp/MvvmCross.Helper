namespace MobileApp.Android.Binding
{
    using System;

    using Cirrious.CrossCore.Droid;
    using Cirrious.CrossCore.Platform;
    using Cirrious.MvvmCross.Binding;
    using Cirrious.MvvmCross.Binding.Droid.Target;
    using Cirrious.MvvmCross.Binding.Droid.Views;

    using global::Android.Widget;

    /// <summary>
    /// Custom adapter for view selected item event
    /// </summary>
    public class MvxAdapterViewSelectedItemTargetBinding : MvxAndroidTargetBinding
    {
        #region Fields

        private readonly MvxListView view;

        private object currentValue;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MvxAdapterViewSelectedItemTargetBinding"/> class. 
        /// Constructor of custom binding
        /// </summary>
        /// <param name="view">
        /// View to bind
        /// </param>
        public MvxAdapterViewSelectedItemTargetBinding(MvxListView view)
            : base(view)
        {
            this.view = view;
            ((ListView)this.view).ItemClick += this.OnItemClick;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Binding mode
        /// </summary>
        public override MvxBindingMode DefaultMode
        {
            get
            {
                return MvxBindingMode.TwoWay;
            }
        }

        /// <summary>
        /// targetType
        /// </summary>
        public override Type TargetType
        {
            get
            {
                return typeof(object);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Value changed
        /// </summary>
        /// <param name="value">
        /// New value
        /// </param>
        public override void SetValue(object value)
        {
            if (value == null || value == this.currentValue)
            {
                return;
            }

            int index = this.view.Adapter.GetPosition(value);
            if (index < 0)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value not found for spinner {0}", value.ToString());
                return;
            }

            this.currentValue = value;
            this.view.SetSelection(index);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="isDisposing">
        /// realy ?
        /// </param>
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                ((ListView)this.view).ItemClick -= this.OnItemClick;
            }

            base.Dispose(isDisposing);
        }

        /// <summary>
        /// Set value
        /// </summary>
        /// <param name="target">
        /// Where to put
        /// </param>
        /// <param name="value">
        /// What to put
        /// </param>
        protected override void SetValueImpl(object target, object value)
        {
            throw new NotImplementedException();
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            var container = this.view.GetItemAtPosition(itemClickEventArgs.Position) as MvxJavaContainer;
            if (container == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Missing MvxJavaContainer in MvxAdapterViewSelectedItemTargetBinding");
                return;
            }

            object newValue = container.Object;
            if (newValue.Equals(this.currentValue))
            {
                return;
            }

            this.currentValue = newValue;
            this.FireValueChanged(newValue);
        }

        #endregion
    }
}