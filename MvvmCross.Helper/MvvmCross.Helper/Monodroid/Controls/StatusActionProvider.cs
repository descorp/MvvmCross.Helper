namespace MobileApp.Android.UI.Controls
{
    using global::Android.Content;
    using global::Android.Provider;
    using global::Android.Support.V4.View;
    using global::Android.Views;
    using global::Android.Widget;

    public class SettingsActionProvider : ActionProvider
    {
        #region Static Fields

        private static readonly Intent sSettingsIntent = new Intent(Settings.ActionSettings);

        #endregion

        #region Fields

        private readonly Context mContext;

        #endregion

        #region Constructors and Destructors

        public SettingsActionProvider(Context context)
            : base(context)
        {
            this.mContext = context;
        }

        #endregion

        #region Public Methods and Operators

        public override View OnCreateActionView()
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this.mContext);
            View view = layoutInflater.Inflate(Resource.Layout.settings_action_provider, null);
            var button = view.FindViewById<ImageButton>(Resource.Id.button);
            // Attach a click listener for launching the system settings.
            button.Click += delegate { this.mContext.StartActivity(sSettingsIntent); };
            return view;
        }

        public override bool OnPerformDefaultAction()
        {
            // This is called if the host menu item placed in the overflow menu of the
            // action bar is clicked and the host activity did not handle the click.
            this.mContext.StartActivity(sSettingsIntent);
            return true;
        }

        #endregion
    }
}