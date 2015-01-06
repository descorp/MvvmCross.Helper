namespace MobileApp.Android.Binding
{
    using System;

    using Cirrious.MvvmCross.Binding;
    using Cirrious.MvvmCross.Binding.Droid.Target;

    using global::Android.Widget;

    public class IsVoicemailNewBinding : MvxAndroidTargetBinding
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IsOnlineImageBinding"/> class.
        /// </summary>
        /// <param name="imageView">
        /// The imageView.
        /// </param>
        public IsVoicemailNewBinding(IDisposable imageView)
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
                return typeof(bool);
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
            bool status;
            var test = value.ToString();
            if (Enum.TryParse(test, true, out status))
            {
                this.SetImageResource(status);
            }
        }

        private void SetImageResource(bool status)
        {
            var resource = 0;
            switch (status)
            {
                case true:
                    resource = Resource.Drawable.voicemail_new;
                    break;
                case false:
                    resource = Resource.Drawable.voicemail_saved;
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