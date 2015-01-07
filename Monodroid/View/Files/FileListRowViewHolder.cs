namespace MobileApp.Monodroid.Views.Files
{
    using Android.Widget;

    using Java.Lang;

    /// <summary>
    ///   This class is used to hold references to the views contained in a list row.
    /// </summary>
    /// <remarks>
    ///   This is an optimization so that we don't have to always look up the
    ///   ImageView and the TextView for a given row in the ListView.
    /// </remarks>
    public class FileListRowViewHolder : Object
    {
        public FileListRowViewHolder(TextView textView, ImageView imageView, bool isFirst = false)
        {
            this.TextView = textView;
            this.ImageView = imageView;
            this.IsBackFolder = isFirst;
        }

        public bool IsBackFolder { get; set; }       
        public ImageView ImageView { get; private set; }
        public TextView TextView { get; private set; }

        /// <summary>
        ///   This method will update the TextView and the ImageView that are
        ///   are
        /// </summary>
        /// <param name="fileName"> </param>
        /// <param name="fileImageResourceId"> </param>
        public void Update(string fileName, int fileImageResourceId)
        {
            this.TextView.Text = fileName;
            this.ImageView.SetImageResource(fileImageResourceId);
        }
    }
}
