using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ImageService.UI.ViewModels.Interfaces
{
    public interface ISettingsViewModel:INotifyPropertyChanged
    {
        ICommand RemoveDirectoryCommand { get; set; }
        ObservableCollection<string> VmHandlers { get; set; }
        string SelectedItem { get; set; }
        string VmOutputDirectory { get; set; }
        string VmSourceName { get; set; }
        string VmLogName { get; set; }
        string VmThumbnailSize { get; set; }
    }
}