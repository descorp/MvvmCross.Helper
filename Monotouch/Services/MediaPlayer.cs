namespace MobileApp.iOS.UI.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Models;

    using MonoTouch.AVFoundation;
    using MonoTouch.Foundation;


    /// <summary>
    /// Init media features
    /// </summary>
    internal class MediaPlayer : IMediaPlayer
    {
        #region Fields

        private string currentPath;

        private AVAudioPlayer mediaPlayer;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Play function
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="name">file name</param>
        /// <returns>is success</returns>
        public MediaInfo Play(string path, string name)
        {
            if (path == null)
            {
                return new MediaInfo { State = MediaState.Error };
            }

            if (!path.Equals(this.currentPath))
            {
                if(this.mediaPlayer != null)
                {
                    this.mediaPlayer.Stop();
                }

                this.mediaPlayer = path.Contains("http://") ? AVAudioPlayer.FromUrl(NSUrl.FromString(path)) : AVAudioPlayer.FromUrl(NSUrl.FromFilename(path));

                this.mediaPlayer.PrepareToPlay();

                this.currentPath = path;
                this.mediaPlayer.Play();
            }
            else
            {
                this.mediaPlayer.Play();
            }

            return new MediaInfo
                       {
                           Duration = this.mediaPlayer.Duration,
                           CurrentPosition = this.mediaPlayer.CurrentTime,
                           State = this.mediaPlayer.Playing ? MediaState.Play : MediaState.Stop
                       };
        }

        /// <summary>
        /// Change speaker
        /// </summary>
        /// <param name="useLoudSpeaker">use loud speaker</param>
        /// <returns>result</returns>
        public bool ChangeSpeaker(bool useLoudSpeaker)
        {
            try
            {
                NSError error;
                if (AVAudioSession.SharedInstance().OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out error))
                {
                    return true;
                }

                Console.WriteLine("Playback error {0} : {1}  {2}", error.Domain, error.Description, error.UserInfo);
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return false;
            }
        }

        /// <summary>
        /// Delete file function
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>is success</returns>
        public bool Delete(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// set position of playback
        /// </summary>
        /// <param name="position">specific position in seconds</param>
        /// <returns>result</returns>
        public bool SetPosition(double position)
        {
            try
            {
                this.mediaPlayer.CurrentTime = position;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Delegate caching media
        /// </summary>
        /// <param name="binaryes">mp3 file's binary</param>
        /// <param name="name"> name of file to save</param>
        /// <returns> success ?</returns>
        public Task<string> CacheMedia(byte[] binaryes, string name)
        {
            return Task<string>.Factory.StartNew(
                () =>
                    {
                        var fileName = string.Format("Myfile_{0}.mp3", name);
                        var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        var audioFilePath = Path.Combine(documents, fileName);

                        if (binaryes != null)
                        {
                            File.WriteAllBytes(audioFilePath, binaryes);
                            return audioFilePath;
                        }

                        return string.Empty;
                    });
        }

        /// <summary>
        /// Delete file function
        /// </summary>
        /// <returns>is success</returns>
        public MediaInfo GetStatus(string media)
        {
            try
            {
                if(this.currentPath.Equals(media))
                {
                    var player = this.mediaPlayer;
                    if (player != null)
                    {
                        return new MediaInfo
                                   {
                                       CurrentPosition = player.CurrentTime,
                                       Duration = player.Duration,
                                       State = this.mediaPlayer.Playing ? MediaState.Play : MediaState.Stop
                                   };
                    }
                }

                return new MediaInfo { State = MediaState.Error };
            }
            catch (Exception e)
            {
                return new MediaInfo { State = MediaState.Error };
            }
        }

        /// <summary>
        /// Delete file function
        /// </summary>
        /// <returns>is success</returns>
        public bool Pause()
        {
            try
            {
                if (this.mediaPlayer.Playing)
                {
                    this.mediaPlayer.Pause();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion
    }
}