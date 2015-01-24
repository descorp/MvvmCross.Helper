namespace MobileApp.Android.Binding
{
    using System;

    using Cirrious.MvvmCross.Binding;
    using Cirrious.MvvmCross.Binding.Droid.Target;

    using global::Android.Graphics;
    using global::Android.Widget;

    internal class TextSelectionBinding : MvxAndroidTargetBinding
    {
        public TextSelectionBinding(object target)
            : base(target)
        {
        }

        private TextView TextView
        {
            get
            {
                return (TextView)this.Target;
            }
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
            bool isSelected;

            if (!bool.TryParse(value.ToString(), out isSelected) || this.TextView == null)
            {
                return;
            }

            this.TextView.SetTextColor(Color.ParseColor(isSelected ? "#4c9fcb" : "#7998b0"));
            this.TextView.SetTypeface(Typeface.Default, isSelected ? TypefaceStyle.Bold : TypefaceStyle.Normal);
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
                var button = this.TextView;
                if (button != null)
                {
                    button.Dispose();
                }
            }

            base.Dispose(isDisposing);
        }
    }
}