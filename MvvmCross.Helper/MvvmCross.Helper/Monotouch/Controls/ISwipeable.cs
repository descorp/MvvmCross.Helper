namespace MobileApp.IOS.UI.Views
{
    using System.Windows.Input;

    public interface ISwipeable
    {
        ICommand RemoveCommand { get; }
    }
}