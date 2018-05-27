using System.Collections.Generic;
using System.Collections.ObjectModel;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;

namespace ImageService.Commands
{
    public class GetLogsComand : ICommand
    {
        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="loggingService">ILoggingService object</param>
        public GetLogsComand(ILoggingService loggingService)
        {
            _logging = loggingService;
        }
        #endregion

        #region Members
        private ILoggingService _logging;
        #endregion

        #region Methods
        public string Execute(string[] args, out bool result, out bool responseBack)
        {
            string[] logsArray = new string[1];
            IList<LogMessage> logsArchive = _logging.LogArchive;
            string jsonLogsArchiveMessages = JsonConvert.SerializeObject(logsArchive);
            logsArray[0] = jsonLogsArchiveMessages;
            CommandRecievedEventArgs commandSendArgs = new CommandRecievedEventArgs((int)CommandEnum.GetLogsCommand, logsArray, "");
            result = true;
            responseBack = true;
            return JsonConvert.SerializeObject(commandSendArgs);
        }

        #endregion

    }
}