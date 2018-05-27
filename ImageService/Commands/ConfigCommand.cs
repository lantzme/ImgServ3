using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ImageService.Configuration;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using ImageService.Modal.Event;
using ImageService.Server;
using Newtonsoft.Json;

namespace ImageService.Commands
{
    public class ConfigCommand : ICommand
    {
        #region Members
        private ImageServer _server;
        #endregion

        #region C'tor
        /// <summary>
        /// The Constructor of the class.
        /// </summary>
        /// <param name="server"></param>
        public ConfigCommand(ImageServer server)
        {
            _server = server;
        }
        #endregion

        #region Methods
        public string Execute(string[] args, out bool result, out bool responseBack)
        {
            try
            {
                CommandRecievedEventArgs commandSendArgs = GetCurrentConfig();
                result = true;
                responseBack = true;
                return JsonConvert.SerializeObject(commandSendArgs);
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
        }

        /// <summary>
        /// Returns the current appconfig parameters.
        /// </summary>
        /// <returns>CommandRecievedEventArgs object</returns>
        public CommandRecievedEventArgs GetCurrentConfig()
        {
            ConfigurationParser parser = ConfigurationParser.GetParse();
            IImageConfiguration config = parser.Configuration;
            string[] configArray = new string[5];
            configArray[0] = config.OutputDir;
            configArray[1] = config.SourceName;
            configArray[2] = config.LogName;
            configArray[3] = config.ThumbnailSize.ToString();
            configArray[4] = GetHandlersString();
            CommandRecievedEventArgs configArgs = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, configArray, "");
            return configArgs;
        }

        /// <summary>
        /// Returns a string of concated handlers. 
        /// </summary>
        /// <returns>string</returns>
        private string GetHandlersString()
        {
            IList<IDirectoryHandler> directoryHandlerList = _server.HandlersList;
            List<string> handlerList = directoryHandlerList.Select(directoryHandler => directoryHandler.GetDirectoryPath()).ToList();
            StringBuilder handlersStringBuilder = new StringBuilder();
            foreach (string handler in handlerList)
            {
                handlersStringBuilder.Append(handler);
                handlersStringBuilder.Append(";");
            }
            return handlersStringBuilder.ToString();
        }
        #endregion
    }
}