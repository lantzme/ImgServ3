using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ImageService.Logging;

namespace ImageService.UI.ViewModels.Interfaces
{
    public interface ILogsViewModel:INotifyPropertyChanged
    {
        ObservableCollection<LogMessage> VmLogArchive { get; set; }
    }
}