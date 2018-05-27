using System.ComponentModel;
using System.Runtime.CompilerServices;
using ImageService.UI.Communication;
using ImageService.UI.Models.Interfaces;

namespace ImageService.UI.Models
{
    public class MainWindowModel : IMainWindowModel
    {
        #region Properties
        public TcpClientAdapter TcpAdapter { get; set; }
        public bool IsConnected { get; set; }
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        public MainWindowModel()
        {
            TcpAdapter = TcpClientAdapter.Instance;
            IsConnected = TcpAdapter.IsConnected;
        }
        #endregion

        #region Methods
        public void CloseServerConnection()
        {
            TcpAdapter.CloseTcpClient();
        }
        #endregion
    }
}