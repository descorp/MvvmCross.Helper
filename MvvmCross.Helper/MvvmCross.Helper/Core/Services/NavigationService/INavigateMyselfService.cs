namespace MvvmCross.Helper.Core.Services.UtilityService
{
    /// <summary>
    ///  Navigation service
    /// </summary>
    public interface INavigateMyselfService
    {
        /// <summary>
        /// Open Auth page instantly
        /// </summary>
        /// <param name="cause">Why had you had this done ? </param>
        void Breakout(string cause);
    }
}