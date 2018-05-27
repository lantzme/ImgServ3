using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Events;

namespace ImageService.Logging
{
    public delegate void NotifyClients(CommandRecievedEventArgs updateArgs);

    public interface ILoggingService
    {
        List<LogMessage> LogArchive { get; set; }
        event NotifyClients NotifyClientsNewLog;
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        void Log(string message, MessageTypeEnum type);           // Logging the Message

    }
}
