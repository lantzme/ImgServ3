using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using ImageService.Logging;
using ImageService.UI.Communication;
using ImageService.UI.Models.Interfaces;
using Newtonsoft.Json;

namespace ImageService.UI.Models
{
    public class LogsModel : ILogsModel
    {
        #region Propeties
        public TcpClientAdapter TcpAdapter { get; set; }
   
        public ObservableCollection<LogMessage> LogMessegesArchive { get; set; }
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        public LogsModel()
        {
            LogMessegesArchive = new ObservableCollection<LogMessage>();
            TcpAdapter = TcpClientAdapter.Instance;
            if (TcpAdapter.IsConnected)
            {
                TcpAdapter.HandleResp += HandleResponse;
                TcpAdapter.RecieveResponseFromServer();
                FetchLogs();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handling the input command according to its type.
        /// </summary>
        /// <param name="args">CommandRecievedEventArgs object</param>
        private void HandleResponse(CommandRecievedEventArgs args)
        {
            switch (args.CommandId)
            {
                case (int)CommandEnum.GetLogsCommand:
                    InitializeLogsArchive(args);
                    break;
                case (int)CommandEnum.NewLog:
                    UpdateLogsCollection(args);
                    break;
            }
        }

        /// <summary>
        /// Adding a new log to the logs archive collection.
        /// </summary>
        /// <param name="args"></param>
        private void UpdateLogsCollection(CommandRecievedEventArgs args)
        {
            LogMessage newLog = new LogMessage
            {
                Type = args.Args[0],
                Message = args.Args[1]
            };
            Application.Current.Dispatcher.Invoke(() =>
            {
                LogMessegesArchive.Add(newLog);
            });
        }

        /// <summary>
        /// Initialize the logs archive according to the input command parameters.
        /// </summary>
        /// <param name="args">CommandRecievedEventArgs object</param>
        private void InitializeLogsArchive(CommandRecievedEventArgs args)
        {
            var logsJson = ParseJsonToLogsCollection(args);
            try
            {
                foreach (LogMessage log in logsJson)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LogMessegesArchive.Add(log);
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Parses the input command to an ObservableCollection object.
        /// </summary>
        /// <param name="args">CommandRecievedEventArgs object</param>
        /// <returns>ObservableCollection of LogMessages</returns>
        private ObservableCollection<LogMessage> ParseJsonToLogsCollection(CommandRecievedEventArgs args)
        {
            return JsonConvert.DeserializeObject<ObservableCollection<LogMessage>>(args.Args[0]);
        }

        /// <summary>
        /// Sends a 'GetLogs' command to the server.
        /// </summary>
        private void FetchLogs()
        {
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.GetLogsCommand, null, "");
            TcpAdapter.SendCommandToServer(commandRecievedEventArgs);
        }
        #endregion
    }
}