namespace MvvmCross.Helper.Core.Models
{
    /// <summary>
    /// Contains all info about playbac state
    /// </summary>
    public struct MediaInfo
    {
        /// <summary>
        /// Duration of media
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Current position
        /// </summary>
        public double CurrentPosition { get; set; }

        /// <summary>
        /// State of playback
        /// </summary>
        public MediaState State { get; set; }
    }

    /// <summary>
    /// State of playback
    /// </summary>
    public enum MediaState
    {
        Play,
        Stop,
        Pause,
        Error
    }
}