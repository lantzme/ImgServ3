using System.Collections.ObjectModel;
using ImageService.UI.Communication;

namespace ImageService.UI.Models.Interfaces
{
    public interface ISettingsModel
    {
        /// <summary>
        /// Removes the input directory handler from the server.
        /// </summary>
        /// <param name="directory">directory string path</param>
        void RemoveDirectory(string directory);
        TcpClientAdapter TcpAdapter { get; set; }
        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        string ThumbnailSize { get; set; }
        ObservableCollection<string> Handlers { get; set; }
    }
}