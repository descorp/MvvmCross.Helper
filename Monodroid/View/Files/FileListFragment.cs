namespace MobileApp.Monodroid.Views.Files
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Android.OS;
    using Android.Support.V4.App;
    using Android.Util;
    using Android.Views;
    using Android.Widget;

    using Cirrious.CrossCore;

    using Newtonsoft.Json;

    using MobileApp.Core.Services;

    /// <summary>
    /// A ListFragment that will show the files and subdirectories of a given directory.
    /// </summary>
    /// <remarks>
    ///     <para> This was placed into a ListFragment to make this easier to share this functionality with with tablets. </para>
    ///     <para>
    /// Note that this is a incomplete example. It lacks things such as the ability to go back up the directory
    /// tree, or any special handling of a file when it is selected.
    ///     </para>
    /// </remarks>
    public class FileListFragment : ListFragment
    {
        #region Static Fields

        public static readonly string DefaultInitialDirectory = "/";

        #endregion

        #region Fields

        private FileListAdapter adapter;

        private DirectoryInfo directory;

        private Stack<string> previousDirectorys = new Stack<string>();

        #endregion

        #region Public Methods and Operators

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.adapter = new FileListAdapter(this.Activity, new FileSystemInfo[0]);
            this.ListAdapter = this.adapter;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            if (position != 0)
            {
                var fileSystemInfo = this.adapter.GetItem(position);

                if (fileSystemInfo.IsFile())
                {
                    // Do something with the file.  In this case we just pop some toast.
                    Log.Verbose("FileListFragment", "The file {0} was clicked.", fileSystemInfo.FullName);

                    var appStateService = Mvx.Resolve<IAppStateService>();
                    appStateService.CurrentDir = fileSystemInfo.FullName;

                    var data = File.ReadAllText(fileSystemInfo.FullName);
                    try
                    {
                        var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                        appStateService.Load(dictionary);
                    }
                    catch (Exception e)
                    {
                        Log.Error("FileListFragment", "Couldn't access the file " + fileSystemInfo.FullName + "; " + e);
                        Toast.MakeText(this.Activity, "Problem retrieving contents of " + fileSystemInfo.FullName, ToastLength.Long).Show();
                    }
                }
                else
                {
                    // Dig into this directory, and display it's contents
                    this.previousDirectorys.Push(this.directory.FullName);
                    this.RefreshFilesList(fileSystemInfo.FullName);
                }
            }
            else
            {
                this.RefreshFilesList(this.previousDirectorys.Count > 0 ?  this.previousDirectorys.Pop() : DefaultInitialDirectory);
            }

            base.OnListItemClick(l, v, position, id);
        }

        public override void OnResume()
        {
            base.OnResume();
            this.adapter.IsRoot = true;
            this.previousDirectorys.Clear();
            this.RefreshFilesList(DefaultInitialDirectory);
        }

        public void RefreshFilesList(string newDirectory)
        {
           
            IList<FileSystemInfo> visibleThings = new List<FileSystemInfo>();
            var dir = new DirectoryInfo(newDirectory);

            try
            {
                foreach (var item in dir.GetFileSystemInfos().Where(item => item.IsVisible()))
                {
                    visibleThings.Add(item);
                }
            }
            catch (Exception ex)
            {
                Log.Error("FileListFragment", "Couldn't access the directory " + this.directory.FullName + "; " + ex);
                Toast.MakeText(this.Activity, "Problem retrieving contents of " + newDirectory, ToastLength.Long).Show();
                return;
            }

            this.directory = dir;
            this.Activity.Title = dir.FullName;
            this.adapter.AddDirectoryContents(visibleThings, dir);

            // If we don't do this, then the ListView will not update itself when then data set 
            // in the adapter changes. It will appear to the user that nothing has happened.
            this.ListView.RefreshDrawableState();

            Log.Verbose("FileListFragment", "Displaying the contents of directory {0}.", newDirectory);
        }

        #endregion
    }
}