namespace MobileApp.Core.Interfaces
{
    using System.Threading.Tasks;

    using MobileApp.Core.Models;

    /// <summary>
    /// Implement all native logic for audio playback
    /// </summary>
    public interface IMediaPlayer
    {
        /// <summary>
        /// Play function
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="name">file name</param>
        /// <returns>is success</returns>
        MediaInfo Play(string path, string name);

        /// <summary>
        /// Change speaker
        /// </summary>
        /// <param name="useLoudSpeaker">use loud speaker</param>
        /// <returns>result</returns>
        bool ChangeSpeaker(bool useLoudSpeaker);

        /// <summary>
        /// Delete file function
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>is success</returns>
        bool Delete(string path);

        /// <summary>
        /// set position of playback
        /// </summary>
        /// <param name="position">specific position in seconds</param>
        /// <returns>result</returns>
        bool SetPosition(double position);

        /// <summary>
        /// Delete file function
        /// </summary>
        /// <returns>is success</returns>
        bool Pause();

        /// <summary>
        /// Delete file function
        /// </summary>
        /// <returns>is success</returns>
        MediaInfo GetStatus(string media);

        /// <summary>
        /// Delegate caching media
        /// </summary>
        /// <param name="binaryes">mp3 file's binary</param>
        /// <param name="name"> name of file to save</param>
        /// <returns> success ?</returns>
        Task<string> CacheMedia(byte[] binaryes, string name);
    }
}