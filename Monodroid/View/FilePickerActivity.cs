namespace MobileApp.Monodroid.Views.Files
{
    using Android.App;
    using Android.OS;

    using Cirrious.MvvmCross.Droid.Fragging;

    using MobileApp.Core.ViewModels;

    [Activity(Label = "@string/app_name", Icon = "@drawable/ic_launcher")]
    public class FilePickerActivity : MvxFragmentActivity
    {
        public new FileLoaderViewModel ViewModel
        {
            get
            {
                return (FileLoaderViewModel)base.ViewModel;
            }

            set
            {
                base.ViewModel = value;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.filepicker);
        }
    }
}
