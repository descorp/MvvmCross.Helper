namespace MobileApp.Android.Binding
{
    using System;

    using Cirrious.MvvmCross.Binding.Droid.Target;

    using global::Android.Widget;

    internal class PlayButtonStateBinding : MvxAndroidTargetBinding
    {
        public PlayButtonStateBinding(object target)
            : base(target)
        {
        }

        private ImageButton Container
        {
            get
            {
                return (ImageButton)this.Target;
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

        public override Type TargetType
        {
            get
            {
                return typeof(bool);
            }
        }

        protected override void SetValueImpl(object target, object value)
        {
            bool isPlaying;

            if (!bool.TryParse(value.ToString(), out isPlaying) || this.Container == null)
            {
                return;
            }

            this.Container.SetImageResource(isPlaying ? Resource.Drawable.pause_icon : Resource.Drawable.play_icon);
        }
    }
}