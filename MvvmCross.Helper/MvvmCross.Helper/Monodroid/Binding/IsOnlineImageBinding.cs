namespace MobileApp.Android.Binding
{
    using System;

    using Cirrious.MvvmCross.Binding;
    using Cirrious.MvvmCross.Binding.Droid.Target;

    using MobileApp.Core.Models;

    using global::Android.Widget;

    /// <summary>
    /// The favorites imageView binding.
    /// </summary>
    public class IsOnlineImageBinding : MvxAndroidTargetBinding
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IsOnlineImageBinding"/> class.
        /// </summary>
        /// <param name="imageView">
        /// The imageView.
        /// </param>
        public IsOnlineImageBinding(IDisposable imageView)
            : base(imageView)
        {
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
                return MvxBindingMode.TwoWay;
            }
        }

        /// <summary>
        /// Gets the target type.
        /// </summary>
        public override Type TargetType
        {
            get
            {
                return typeof(ContactStatus);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the image view.
        /// </summary>
        protected ImageView ImageView
        {
            get
            {
                return (ImageView)this.Target;
            }
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
                var button = this.ImageView;
                if (button != null)
                {
                    button.Dispose();
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
            ContactStatus status;
            var test = value.ToString();
            if (Enum.TryParse(test, true, out status))
            {
                this.SetImageResource(status);
            }
        }

        private void SetImageResource(ContactStatus status)
        {
            var resource = 0;
            switch (status)
            {
                case ContactStatus.EarlyInbound:
                    resource = Resource.Drawable.ic_status_early_incoming;
                    break;
                case ContactStatus.EarlyOutbound:
                    resource = Resource.Drawable.ic_status_early_outgoing;
                    break;
                case ContactStatus.Confirmed:
                    resource = Resource.Drawable.ic_status_in_talk;
                    break;
                case ContactStatus.Online:
                    resource = Resource.Drawable.ic_status_online;
                    break;
                case ContactStatus.Offline:
                    resource = Resource.Drawable.ic_status_offline;
                    break;
            }

            var button = this.ImageView as ImageButton;
            if (button != null)
            {
                button.SetImageResource(resource);
            }
            else
            {
                this.ImageView.SetImageResource(resource);
            }
        }

        #endregion
    }
}