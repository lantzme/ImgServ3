
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;

namespace ImageService.Logging
{

    public class LoggingService : ILoggingService
    {
        private static LoggingService _loggingService;

        public event NotifyClients NotifyClientsNewLog = null;

        public LoggingService()
        {
            LogArchive = new List<LogMessage>();
        }

        public static LoggingService Instance => _loggingService ?? (_loggingService = new LoggingService());

        #region Events
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        #endregion

        #region Methods

        /// <summary>
        /// This method logs a message to the logger.
        /// </summary>
        /// <param name="message">The message to log in string form.</param>
        /// <param name="type">The type of message we inform the logger about</param>
        public void Log(string message, MessageTypeEnum type)
        {
            var msgEventArgs = new MessageRecievedEventArgs
            {
                Message = message,
                Status = type
            };
            SaveLog(message, type);
            MessageRecieved?.Invoke(this, msgEventArgs);

            InvokeNotification(message, type);

        }

        private void InvokeNotification(string message, MessageTypeEnum type)
        {
            string[] args = new string[2];
            LogMessage newLogEnrty = new LogMessage { Type = Enum.GetName(typeof(MessageTypeEnum), type), Message = message };
            args[0] = newLogEnrty.Type;
            args[1] = newLogEnrty.Message;
            CommandRecievedEventArgs updateObj = new CommandRecievedEventArgs((int)CommandEnum.NewLog, args, null);
            NotifyClientsNewLog?.Invoke(updateObj);
            Thread.Sleep(100);
        }

        private void SaveLog(string message, MessageTypeEnum type)
        {
            LogMessage log = new LogMessage { Type = Enum.GetName(typeof(MessageTypeEnum), type), Message = message };
            LogArchive.Add(log);
        }

        public List<LogMessage> LogArchive { get; set; }
        #endregion
    }
}
