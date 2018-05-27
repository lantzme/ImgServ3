using System.Collections.ObjectModel;
using ImageService.Logging;
using ImageService.UI.Communication;

namespace ImageService.UI.Models.Interfaces
{
    public interface ILogsModel
    {
        ObservableCollection<LogMessage> LogMessegesArchive { get; set; }
        TcpClientAdapter TcpAdapter { get; set; }
    }
}