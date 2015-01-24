namespace MvvmCross.Helper.Core.Services.UtilityService.Logger
{
    public interface ILogWriter<in T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Wright new log item
        /// </summary>
        /// <param name="report">new log item</param>
        /// <returns>result</returns>
        bool Report(T report);

        #endregion
    }
}