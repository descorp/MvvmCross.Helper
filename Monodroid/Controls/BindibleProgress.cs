namespace MobileApp.Android.Views
{
    using global::Android.App;
    using global::Android.Content;

    /// <summary>
    /// Bpogress for binding
    /// </summary>
    public class BindableProgress
    {
        #region Fields

        private readonly Context context;

        private readonly string text;

        private ProgressDialog dialog;

        #endregion

        #region Constructors and Destructors

        public BindableProgress(Context context, string text)
        {
            this.context = context;
            this.text = text;
        }

        #endregion

        #region Public Properties

        public bool Visible
        {
            get
            {
                return this.dialog != null;
            }

            set
            {
                if (value == this.Visible)
                {
                    return;
                }

                if (value)
                {
                    this.dialog = new ProgressDialog(this.context);
                    this.dialog.SetTitle("FirmTel Operator");
                    dialog.SetMessage(this.text);
                    this.dialog.Show();
                }
                else
                {
                    this.dialog.Hide();
                    this.dialog = null;
                }
            }
        }

        #endregion
    }
}