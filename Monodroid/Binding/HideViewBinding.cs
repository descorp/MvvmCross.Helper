namespace MobileApp.Android.Binding
{
    using System;

    using Cirrious.MvvmCross.Binding;
    using Cirrious.MvvmCross.Binding.Droid.Target;

    using global::Android.Views;
    using global::Android.Views.Animations;

    internal class HideViewBinding : MvxAndroidTargetBinding
    {
        public HideViewBinding(object target)
            : base(target)
        {
        }

        private ViewGroup Container
        {
            get
            {
                return (ViewGroup)this.Target;
            }
        }

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

        public override Type TargetType
        {
            get
            {
                return typeof(bool);
            }
        }

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
                var button = this.Container;
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
            bool isPlaying;

            if (!bool.TryParse(value.ToString(), out isPlaying) || this.Container == null)
            {
                return;
            }

            if (this.Container.Visibility == ViewStates.Gone && !isPlaying)
            {
                this.Container.StartAnimation(new AlphaAnimation(1f, 0f) { Duration = 0, FillAfter = true });
                this.Container.Visibility = ViewStates.Visible;
                return;
            }

            this.Container.StartAnimation(
                !isPlaying
                    ? new AlphaAnimation(1f, 0f) { Duration = 200, FillAfter = true, StartOffset = 200 }
                    : new AlphaAnimation(0f, 1f) { Duration = 300, FillAfter = true });
        }
    }
}