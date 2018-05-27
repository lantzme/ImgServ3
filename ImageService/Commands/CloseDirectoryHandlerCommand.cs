using System;
using System.Configuration;
using System.Text;
using ImageService.Configuration;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using ImageService.Server;
using Newtonsoft.Json;

namespace ImageService.Commands
{
    public class CloseDirectoryHandlerCommand : ICommand
    {
        #region Members
        private ImageServer _server;

        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="server">ImageServer object</param>
        public CloseDirectoryHandlerCommand(ImageServer server)
        {
            _server = server;
        }
        #endregion

        #region Methods
        public string Execute(string[] args, out bool result, out bool responseBack)
        {
            _server.CloseHandler(args[0]);
            ConfigCommand configCommand = new ConfigCommand(_server);
            CommandRecievedEventArgs requestArgs = configCommand.GetCurrentConfig();
            _server.HandlerRemoved(requestArgs);
            result = true;
            responseBack = false;
            return String.Empty;
        }
        #endregion
    }
}