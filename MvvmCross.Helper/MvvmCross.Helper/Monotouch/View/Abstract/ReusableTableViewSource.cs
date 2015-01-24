namespace MobileApp.IOS.UI.Views
{
    using Cirrious.MvvmCross.Binding.Touch.Views;
    using Cirrious.MvvmCross.ViewModels;

    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    /// <summary>
    /// Creates table view with separated initialization
    /// </summary>
    /// <typeparam name="TView">View to use</typeparam>
    /// <typeparam name="TViewModel">View model to use</typeparam>
    public abstract class ReusableTableViewSource<TView, TViewModel> : MvxTableViewSource
        where TView : SlideOnSelectListTableViewCell<TViewModel> where TViewModel : MvxViewModel
    {
        private readonly NSString cellIdentifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactListTableViewSource"/> class.
        /// </summary>
        /// <param name="tableView">
        /// The table view.
        /// </param>
        /// <param name="cellIdentifier"></param>
        protected ReusableTableViewSource(UITableView tableView, NSString cellIdentifier)
            : base(tableView)
        {
            this.cellIdentifier = cellIdentifier;
            tableView.RegisterNibForCellReuse(UINib.FromName(this.cellIdentifier, NSBundle.MainBundle), this.cellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = tableView.DequeueReusableCell(this.cellIdentifier);

            var viewCell = cell as TView;
            if (viewCell != null && item != null)
            {
                viewCell.Update(item as TViewModel);
            }

            return cell;
        }
    }
}