using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using ImageService.UI.Annotations;
using ImageService.UI.Communication;
using ImageService.UI.Models.Interfaces;

namespace ImageService.UI.Models
{
    public class SettingsModel : ISettingsModel
    {
        #region Properties
        public TcpClientAdapter TcpAdapter { get; set; }
        public string OutputDirectory { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string ThumbnailSize { get; set; }
        public ObservableCollection<string> Handlers { get; set; }
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        public SettingsModel()
        {
            TcpAdapter = TcpClientAdapter.Instance;
            InitializeSettings();
            if (TcpAdapter.IsConnected)
            {
                TcpAdapter.RecieveResponseFromServer();
                TcpAdapter.HandleResp += HandleResponse;
                FetchConfigFromServer();
            }
        }
        #endregion

        #region Methods
        public void RemoveDirectory(string directory)
        {
            string[] arr = { directory };
            CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseDirectoryHandlerCommand,
                arr, "");
            TcpAdapter.SendCommandToServer(eventArgs);
        }

        /// <summary>
        /// Handling the input command according to its type.
        /// </summary>
        /// <param name="args">CommandRecievedEventArgs object</param>
        private void HandleResponse(CommandRecievedEventArgs args)
        {
            switch (args.CommandId)
            {
                case (int)CommandEnum.GetConfigCommand:
                    ParseConfigResponse(args);
                    break;
            }
        }

        /// <summary>
        /// Parses the servers response.
        /// </summary>
        /// <param name="args">CommandRecievedEventArgs object</param>
        private void ParseConfigResponse(CommandRecievedEventArgs args)
        {
            OutputDirectory = args.Args[0];
            SourceName = args.Args[1];
            LogName = args.Args[2];
            ThumbnailSize = args.Args[3];
            List<string> updatedHandlers = args.Args[4].Split(';').ToList();
            if (updatedHandlers.Contains(""))
            {
                updatedHandlers.Remove("");
            }
            foreach (string handler in updatedHandlers)
            {
                if (!Handlers.Contains(handler))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Handlers.Add(handler);
                    });
                }
            }
            UpdateDirectoryHandlersCollection(updatedHandlers);
        }

        /// <summary>
        /// Updates the Handlers collection.
        /// </summary>
        /// <param name="updatedHandlers"></param>
        private void UpdateDirectoryHandlersCollection(IList<string> updatedHandlers)
        {
            if (updatedHandlers.Count < Handlers.Count)
            {
                for (int i = 0; i < Handlers.Count; i++)
                {
                    if (!updatedHandlers.Contains(Handlers[i]))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Handlers.Remove(Handlers[i]);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the config parameters.
        /// </summary>
        private void InitializeSettings()
        {
            Handlers = new ObservableCollection<string>();
            LogName = string.Empty;
            OutputDirectory = string.Empty;
            SourceName = string.Empty;
            ThumbnailSize = string.Empty;
        }

        private void FetchConfigFromServer()
        {
            CommandRecievedEventArgs request = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, new string[5], "");
            TcpAdapter.SendCommandToServer(request);
            Thread.Sleep(1000);
        }
        #endregion

    }
}