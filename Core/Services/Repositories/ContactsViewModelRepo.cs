namespace MobileApp.Core.Services.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MobileApp.Core.Models.DataContracts;
    using MobileApp.Core.Services.UtilityService.Logger;
    using MobileApp.Core.ViewModels;

    /// <summary>
    /// Get access to storage of contacts' view models
    /// </summary>
    public class ContactsViewModelRepo : ViewModelRepoBase<Contact, ContactViewModel>
    {
        #region Constructors and Destructors

        public ContactsViewModelRepo(ILogger logger)
            : base(logger)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get filtered list on view models
        /// </summary>
        /// <param name="parameters">ONLINEONLY - Boolean?; SEARCH - string</param>
        /// <returns>resulting collection</returns>
        public override Task<List<ContactViewModel>> FilteredList(Dictionary<string, object> parameters)
        {
            var onlineOnly = parameters["ONLINEONLY"] as bool?;
            var search = parameters["SEARCH"] as string;
            var own = int.Parse(parameters["OWN"].ToString());
            return Task<List<ContactViewModel>>.Factory.StartNew(
                () =>
                    {
                        IEnumerable<ContactViewModel> items;
                        lock (this.Locker)
                        {
                            // TODO: add deep copy if necessary
                            var viewModels = this.Collection.Values.Where(vm => !vm.UserId.Equals(own));

                            switch (onlineOnly)
                            {
                                case true:
                                    items = viewModels.Where(n => n.IsOnline);
                                    break;
                                case false:
                                    items = viewModels.OrderBy(contact => contact.Title);
                                    break;
                                case null:
                                    items = viewModels.Where(n => !n.IsOnline);
                                    break;
                                default:
                                    items = new List<ContactViewModel>();
                                    break;
                            }

                            if (!string.IsNullOrWhiteSpace(search))
                            {
                                items = items.Where(contact => contact.Title.ToLower().Contains(search.ToLower())).ToList();
                            }
                        }

                        return items.OrderBy(contact => contact.Title).ToList();
                    });
        }

        #endregion
    }
}