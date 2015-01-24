namespace MobileApp.Monodroid.Views.Files
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Android.Content;
    using Android.Views;
    using Android.Widget;

    public class FileListAdapter : ArrayAdapter<FileSystemInfo>
    {
        private readonly Context _context;

        public FileListAdapter(Context context, IList<FileSystemInfo> fsi)
            : base(context, Resource.Layout.file_picker_list_item, Android.Resource.Id.Text1, fsi)
        {
            this.IsRoot = true;
            this._context = context;
        }

        public bool IsRoot { get; set; }

        /// <summary>
        ///   We provide this method to get around some of the
        /// </summary>
        /// <param name="directoryContents"> </param>
        public void AddDirectoryContents(IEnumerable<FileSystemInfo> directoryContents, FileSystemInfo parent)
        {
            this.Clear();
            // Notify the _adapter that things have changed or that there is nothing 
            // to display.
            if (directoryContents.Any())
            {
                if (this.IsRoot)
                {
                    this.IsRoot = false;                  
                }
                else
                {
                    this.Add(parent);
                }

#if __ANDROID_11__
                // .AddAll was only introduced in API level 11 (Android 3.0). 
                // If the "Minimum Android to Target" is set to Android 3.0 or 
                // higher, then this code will be used.
                AddAll(directoryContents.ToArray());
#else
                // This is the code to use if the "Minimum Android to Target" is
                // set to a pre-Android 3.0 API (i.e. Android 2.3.3 or lower).
                lock (this)
                    foreach (var fsi in directoryContents)
                    {
                        this.Add(fsi);
                    }
#endif

                this.NotifyDataSetChanged();
            }
            else
            {
                this.NotifyDataSetInvalidated();
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var fileSystemEntry = this.GetItem(position);

            FileListRowViewHolder viewHolder;
            View row;
            if (convertView == null)
            {
                row = this._context.GetLayoutInflater().Inflate(Resource.Layout.file_picker_list_item, parent, false);
                viewHolder = new FileListRowViewHolder(
                    row.FindViewById<TextView>(Resource.Id.file_picker_text),
                    row.FindViewById<ImageView>(Resource.Id.file_picker_image));
                row.Tag = viewHolder;
            }
            else
            {
                row = convertView;
                viewHolder = (FileListRowViewHolder)row.Tag;
            }

            if (!this.IsRoot && position == 0)
            {
                viewHolder.Update("..", Resource.Drawable.ic_folder_open_white_36dp);
            }
            else
            {
                viewHolder.Update(
                    fileSystemEntry.Name,
                    fileSystemEntry.IsDirectory() ? Resource.Drawable.ic_folder_white_36dp : Resource.Drawable.ic_insert_drive_file_white_36dp);
            }

            return row;
        }
    }
}
