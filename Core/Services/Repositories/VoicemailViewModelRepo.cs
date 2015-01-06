namespace MobileApp.Core.Services.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using MobileApp.Core.Models.DataContracts;
    using MobileApp.Core.Services.UtilityService.Logger;
    using MobileApp.Core.ViewModels;

    /// <summary>
    /// Do all voice mails' vie model work
    /// </summary>
    public class VoicemailViewModelRepo : ViewModelRepoBase<Voicemail, VoicemailViewModel>
    {
        #region Fields

        private Dictionary<VoicemailViewModel, bool> selectionStorage;

        #endregion

        #region Constructors and Destructors

        public VoicemailViewModelRepo(ILogger logger)
            : base(logger)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Get filtered view models
        /// </summary>
        /// <param name="parameters">list of parameters to filter</param>
        /// <returns>filtered list</returns>
        public override Task<List<VoicemailViewModel>> FilteredList(Dictionary<string, object> parameters)
        {
            var search = parameters["SEARCH"] as string;
            var isNewOnly = (bool)parameters["NEWONLY"];

            return Task<List<VoicemailViewModel>>.Factory.StartNew(
                () =>
                    {
                        var items = new List<VoicemailViewModel>();
                        lock (this.Locker)
                        {
                            if (this.Collection.Any())
                            {
                                items = (isNewOnly ? this.Collection.Values.Where(vm => vm.IsNew) : this.Collection.Values).OrderByDescending(vm => vm.Date).ToList();

                                if (!string.IsNullOrWhiteSpace(search))
                                {
                                    DateTime date;
                                    var myDTFI = CultureInfo.CurrentCulture.DateTimeFormat;
                                    if (DateTime.TryParseExact(search, "MM/dd/yyyy", myDTFI, DateTimeStyles.None, out date)
                                        || DateTime.TryParseExact(search, "M/dd/yyyy", myDTFI, DateTimeStyles.None, out date)
                                        || DateTime.TryParseExact(search, "M/dd/yyyy hh:mm", myDTFI, DateTimeStyles.None, out date)
                                        || DateTime.TryParseExact(search, "MMM dd", myDTFI, DateTimeStyles.None, out date)
                                        || DateTime.TryParseExact(search, "dd MMM", myDTFI, DateTimeStyles.None, out date))
                                    {
                                        items =
                                            this.Collection.Values.Where(
                                                voicemail => (DateTime.Compare(voicemail.Date, date) > 0 ? voicemail.Date - date : date - voicemail.Date).Hours < 24)
                                                .ToList();
                                    }
                                    else if (DateTime.TryParseExact(search, "hh:mm", myDTFI, DateTimeStyles.None, out date)
                                             || DateTime.TryParseExact(search, "hh:mm:ss", myDTFI, DateTimeStyles.None, out date))
                                    {
                                        items =
                                            this.Collection.Values.Where(
                                                voicemail => (DateTime.Compare(voicemail.Date, date) > 0 ? voicemail.Date - date : date - voicemail.Date).Hours < 2)
                                                .ToList();
                                    }
                                    else
                                    {
                                        items = this.Collection.Values.Where(voicemail => voicemail.From.ToLower().Contains(search.ToLower())).ToList();
                                    }
                                }
                            }
                        }

                        return items;
                    });
        }

        #endregion

    }
}