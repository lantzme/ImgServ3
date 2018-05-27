using System.ComponentModel;
using System.Windows.Input;

namespace ImageService.UI.ViewModels.Interfaces
{
    public interface IMainWindowViewModel:INotifyPropertyChanged
    {
        ICommand CloseWindowCommand { get; set; }
        bool VmIsConnected { get; set; }
    }
}