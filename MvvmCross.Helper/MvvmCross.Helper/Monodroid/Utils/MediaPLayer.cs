namespace MobileApp.Android.Utils
{
    using System;
    using System.Threading.Tasks;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Models;

    using global::Android.Media;

    using Java.IO;

    using Environment = global::Android.OS.Environment;
    using MediaInfo = MobileApp.Core.Models.MediaInfo;

    internal class MediaPlayerDelegate : IMediaPlayer
    {
        #region Fields

        private readonly MediaPlayer player = new MediaPlayer();

        private string currentPath;

        #endregion

        #region Public Methods and Operators

        public Task<string> CacheMedia(byte[] binaryes, string name)
        {
            return Task<string>.Factory.StartNew(
                () =>
                    {
                        var dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryMusic), "FirmtelVoicemails");
                        if (!dir.Exists())
                        {
                            dir.Mkdirs();
                        }

                        var file = new File(dir, name + ".mp3");
                        if (!file.Exists())
                        {
                            var stream = new FileOutputStream(file);

                            stream.WriteAsync(binaryes).ContinueWith(t => { this.Play(file.AbsolutePath, string.Empty); });
                        }

                        return file.AbsolutePath;
                    });
        }

        public bool ChangeSpeaker(bool useLoudSpeaker)
        {
            try
            {
                this.player.SetAudioStreamType(useLoudSpeaker ? Stream.Music : Stream.VoiceCall);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(string path)
        {
            throw new NotImplementedException();
        }

        public MediaInfo GetStatus(string media)
        {
            try
            {
                return this.player != null
                           ? new MediaInfo
                                 {
                                     CurrentPosition = this.player.CurrentPosition,
                                     Duration = this.player.Duration,
                                     State = this.player.IsPlaying ? MediaState.Play : MediaState.Pause
                                 }
                           : new MediaInfo { State = MediaState.Error };
            }
            catch (Exception)
            {
                return new MediaInfo { State = MediaState.Error };
            }
        }

        public bool Pause()
        {
            try
            {
                if (this.player != null && this.player.IsPlaying)
                {
                    this.player.Pause();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public MediaInfo Play(string path, string name)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new MediaInfo { State = MediaState.Error };
            }

            if (!path.Equals(this.currentPath))
            {
                if (this.player.IsPlaying)
                {
                    this.player.Stop();
                }

                this.player.Reset();
                this.player.SetDataSource(path);
                this.player.Prepare();
                this.currentPath = path;
            }

            this.player.Start();

            return new MediaInfo
                       {
                           Duration = this.player.Duration,
                           CurrentPosition = this.player.CurrentPosition,
                           State = this.player.IsPlaying ? MediaState.Play : MediaState.Pause
                       };
        }

        public bool SetPosition(double position)
        {
            if (this.player.IsPlaying)
            {
                this.player.SeekTo((int)position);
                return true;
            }

            return false;
        }

        #endregion
    }
}