using ImageService.UI.Communication;

namespace ImageService.UI.Models.Interfaces
{
    public interface IMainWindowModel
    {
        TcpClientAdapter TcpAdapter { get; set; }
        /// <summary>
        /// Closing the connection with the server.
        /// </summary>
        void CloseServerConnection();
        bool IsConnected { get; set; }
    }
}