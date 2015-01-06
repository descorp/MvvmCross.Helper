namespace MobileApp.Android.Views
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Cirrious.CrossCore;
    using Cirrious.CrossCore.Droid.Platform;
    using Cirrious.MvvmCross.Binding.BindingContext;
    using Cirrious.MvvmCross.Binding.Droid.BindingContext;

    using MobileApp.Core.Interfaces;

    using global::Android.App;
    using global::Android.Content;
    using global::Android.Graphics;
    using global::Android.Support.V4.App;
    using global::Android.Views;
    using global::Android.Widget;

    using Java.Util;

    /// <summary>
    /// Display errors
    /// </summary>
    public class ErrorDisplayer
    {
        #region Fields

        private readonly Context applicationContext;

        private static int buttonClickNotificationId = 1000;

        private readonly ConcurrentDictionary<int, NotificationCompat.Builder> messages = new ConcurrentDictionary<int, NotificationCompat.Builder>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDisplayer"/> class. 
        /// Constructor of display
        /// </summary>
        /// <param name="applicationContext">
        /// some context
        /// </param>
        public ErrorDisplayer(Context applicationContext)
        {
            this.applicationContext = applicationContext;

            var source = Mvx.Resolve<IMessageSource>();
            source.MessageReported += (sender, args) => this.ShowToUser(args);
        }

        #endregion

        #region Methods

        private void ShowToUser(MessageEventArgs args)
        {
            var notificationManager = this.applicationContext.GetSystemService(Context.NotificationService) as NotificationManager;
            var icon = 0;
            NotificationCompat.Builder builder;

            switch (args.NotificationType)
            {
                case NotificationType.NotificationInCall:
                case NotificationType.NotificationRinging:
                case NotificationType.Notification:
                    this.ProcessNotification(args, notificationManager, icon);
                    break;

                case NotificationType.CloseNotification:

                    if (notificationManager != null)
                    {
                        notificationManager.Cancel(args.Id);
                    }
                    break;

                default:
                    {
                        var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity as IMvxBindingContextOwner;
                        var layoutView = activity.BindingInflate(Resource.Layout.ErrorMessage, null);
                        var text1 = layoutView.FindViewById<TextView>(Resource.Id.ErrorText1);

                        text1.Text = args.NotificationType == NotificationType.Error ? "Warning!" : "Message";
                        text1.TextSize = 20;

                        var text2 = layoutView.FindViewById<TextView>(Resource.Id.ErrorText2);
                        text2.Text = args.Message;

                        var toast = new Toast(this.applicationContext);
                        toast.SetGravity(GravityFlags.CenterVertical, 0, 0);

                        // TODO: wtf ?? how to costomize toast duration ?
                        toast.Duration = args.NotificationType == NotificationType.LongWarning ? ToastLength.Long : ToastLength.Short;

                        toast.View = layoutView;
                        toast.Show();
                    }
                    break;
            }
        }

        private void ProcessNotification(MessageEventArgs args, NotificationManager notificationManager, int icon)
        {
            NotificationCompat.Builder builder;

            switch (args.NotificationType)
            {
                case NotificationType.NotificationInCall:
                    if (this.messages.TryGetValue(args.Id, out builder))
                    {
                        builder.SetSmallIcon(Resource.Drawable.ic_status_in_talk);
                        if (!string.IsNullOrWhiteSpace(args.Message))
                        {
                            builder.SetSubText(args.Message);
                        }

                        if (notificationManager != null)
                        {
                            notificationManager.Notify(args.Id, builder.Build());
                        }
                    }
                    break;

                case NotificationType.NotificationRinging:
                    icon = Resource.Drawable.ic_status_early_incoming;
                    goto case NotificationType.Notification;

                case NotificationType.Notification:
                    {
                        if (icon == 0)
                        {
                            icon = Resource.Drawable.ic_launcher;
                        }

                        // Set up an intent so that tapping the notifications returns to this app:
                        var intent = new Intent(this.applicationContext, typeof(HomeView));

                        // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
                        var pendingIntent = PendingIntent.GetActivity(this.applicationContext, args.Id, intent, PendingIntentFlags.OneShot);

                        // Build the notification
                        builder = new NotificationCompat.Builder(this.applicationContext).SetAutoCancel(true)
                            // dismiss the notification from the notification area when the user clicks on it
                            .SetContentTitle(string.IsNullOrWhiteSpace(args.LongText) ? "Incoming call" : args.Message) // Set the title
                            .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                            .SetLargeIcon(BitmapFactory.DecodeResource(this.applicationContext.Resources, Resource.Drawable.ic_launcher))
                            .SetSmallIcon(icon) // This is the icon to display
                            .SetContentText(args.Message) // the message to display.
                            .SetStyle(new NotificationCompat.BigTextStyle());

                        // Finally publish the notification
                        if (notificationManager != null)
                        {
                            this.messages.TryAdd(args.Id, builder);
                            notificationManager.Notify(args.Id == 0 ? buttonClickNotificationId : args.Id, builder.Build());
                        }

                        buttonClickNotificationId++;
                    }
                    return;
            }
        }

        #endregion
    }
}