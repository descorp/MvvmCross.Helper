namespace MobileApp.Core.ViewModels.Abstract
{
    using System.Windows.Input;

    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets the open close command.
        /// </summary>
        ICommand SelectionCommand { get; }
    }
}