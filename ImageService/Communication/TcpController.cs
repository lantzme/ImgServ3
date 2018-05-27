using ImageService.Commands;
using ImageService.Configuration;
using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Server;

namespace ImageService.Communication
{
    public class TcpController : ImageController
    {
        /// <summary>
        /// The constructor of the class
        /// </summary>
        /// <param name="modalParameters">IModalParameters object</param>
        /// <param name="loggingService">ILoggingService object</param>
        /// <param name="server">ImageServer object</param>
        #region C'tor
        public TcpController(IModalParameters modalParameters, ILoggingService loggingService, ImageServer server)
            : base(modalParameters, loggingService)
        {
            CommandsDict.Add((int)CommandEnum.CloseDirectoryHandlerCommand, new CloseDirectoryHandlerCommand(server));
            CommandsDict.Add((int)CommandEnum.GetLogsCommand, new GetLogsComand(Logging));
            CommandsDict.Add((int)CommandEnum.GetConfigCommand, new ConfigCommand(server));
        }
        #endregion
    }
}