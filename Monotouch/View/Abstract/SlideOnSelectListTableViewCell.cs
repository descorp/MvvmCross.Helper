namespace MobileApp.IOS.UI.Views
{
    using System;
    using System.Drawing;

    using Cirrious.MvvmCross.Binding.Touch.Views;
    using Cirrious.MvvmCross.ViewModels;

    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    public abstract class SlideOnSelectListTableViewCell<TViewModel> : MvxTableViewCell
        where TViewModel : MvxViewModel
    {
        #region Constructors and Destructors

        public SlideOnSelectListTableViewCell(IntPtr handle)
            : base(handle)
        {
        }

        #endregion

        #region Public Properties

        public abstract PointF ActionMenuClosePosition { get; }

        public abstract PointF ActionMenuOpenePosition { get; }

        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        public abstract bool IsSelected { get; set; }

        public abstract NSString Key { get; }

        public abstract UINib Nib { get; }

        public abstract PointF StatusClosePosition { get; }

        public abstract PointF StatusOpenPosition { get; }

        #endregion

        #region Public Methods and Operators

        public abstract void InitView();

        public abstract void Update(TViewModel viewmodel);

        #endregion

        #region Methods

        protected abstract void InitialiseBindings();

        #endregion
    }
}