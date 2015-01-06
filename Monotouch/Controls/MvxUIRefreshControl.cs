namespace MobileApp.IOS.UI.Controls
{
    namespace com.refractored.mvxbindings
    {
        using System.Windows.Input;

        using MonoTouch.Foundation;
        using MonoTouch.UIKit;

        /// <summary>
        /// Mvx user interface refresh control.
        /// </summary>
        public class MvxUiRefreshControl : UIRefreshControl
        {
            #region Fields

            private bool isRefreshing;

            private string message;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="MvxUiRefreshControl" /> class.
            /// </summary>
            public MvxUiRefreshControl()
            {
                this.ValueChanged += (sender, e) =>
                    {
                        var command = this.RefreshCommand;
                        if (command == null)
                        {
                            return;
                        }

                        command.Execute(null);
                    };
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets or sets a value indicating whether this instance is refreshing.
            /// </summary>
            /// <value><c>true</c> if this instance is refreshing; otherwise, <c>false</c>.</value>
            public bool IsRefreshing
            {
                get
                {
                    return this.isRefreshing;
                }
                set
                {
                    this.isRefreshing = value;
                    if (this.isRefreshing)
                    {
                        this.BeginRefreshing();
                    }
                    else
                    {
                        this.EndRefreshing();
                    }
                }
            }

            /// <summary>
            /// Gets or sets the message to display
            /// </summary>
            /// <value>The message.</value>
            public string Message
            {
                get
                {
                    return this.message;
                }
                set
                {
                    this.message = value ?? string.Empty;
                    this.AttributedTitle = new NSAttributedString(this.message);
                }
            }

            /// <summary>
            /// Gets or sets the refresh command.
            /// </summary>
            /// <value>The refresh command.</value>
            public ICommand RefreshCommand { get; set; }

            #endregion
        }
    }
}