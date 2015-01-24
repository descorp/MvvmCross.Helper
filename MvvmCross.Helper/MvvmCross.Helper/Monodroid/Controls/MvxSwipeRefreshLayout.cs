namespace MobileApp.Android.Controls
{
    using System;
    using System.Windows.Input;

    using global::Android.Content;
    using global::Android.Support.V4.Widget;
    using global::Android.Util;

    /// <summary>
    /// Swipe refresh element's layout
    /// </summary>
    public class MvxSwipeRefreshLayout : SwipeRefreshLayout
    {
        #region Constructors and Destructors

        public MvxSwipeRefreshLayout(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            this.Init();
        }

        public MvxSwipeRefreshLayout(Context context)
            : base(context)
        {
            this.Init();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the refresh command.
        /// </summary>
        /// <value>The refresh command.</value>
        public ICommand RefreshCommand { get; set; }

        #endregion

        #region Methods

        private void Init()
        {
            // This gets called when we pull down to refresh to trigger command
            this.Refresh += (object sender, EventArgs e) =>
                {
                    ICommand command = this.RefreshCommand;
                    if (command == null)
                    {
                        return;
                    }

                    command.Execute(null);
                };
        }

        #endregion
    }
}