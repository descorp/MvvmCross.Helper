namespace MvvmCross.Helper.Core.Services.UtilityService
{
    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.ViewModels;

    using MvvmCross.Helper.Core.Interfaces;
    using MvvmCross.Helper.Core.ViewModels;

    public class NavigateMyselfService : MvxNavigatingObject, INavigateMyselfService
    {
        /// <summary>
        /// Open Auth page instantly
        /// </summary>
        /// <param name="cause">Why had you had this done ? </param>
        public void Breakout(string cause)
        {
            var credentials = Mvx.Resolve<IAccountService>();

            if (credentials != null)
            {
                var model = credentials.Credentials;
                if (model.HasValue)
                {
                    ShowViewModel<AuthViewModel>(new { BreakoutCause = cause, Instance = model.Value.Instance, UserId = model.Value.UserId, Pin = model.Value.Pin });
                }
            }
            else
            {
                ShowViewModel<AuthViewModel>(new { BreakoutCause = cause });
            }
        }
    }
}
