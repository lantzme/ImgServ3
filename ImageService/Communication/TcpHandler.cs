using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ImageService.Configuration;
using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Event;
using ImageService.Server;
using Newtonsoft.Json;

namespace ImageService.Communication
{
    public class TcpHandler
    {
        #region Properties
        public IImageController ImageController { get; set; }
        public ILoggingService Logging { get; set; }
        public static Mutex Mutex { get; set; }
        #endregion

        #region Members
        private bool _keepReadingRequests;
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="parameters">IModalParameters object</param>
        /// <param name="logging">ILoggingService object</param>
        /// <param name="server">ImageServer object</param>
        public TcpHandler(IModalParameters parameters, ILoggingService logging, ImageServer server)
        {
            _keepReadingRequests = true;
            ImageController = new TcpController(parameters, logging, server);
            Logging = logging;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Reads new commands from the client.
        /// </summary>
        /// <param name="client">TcpClient object</param>
        /// <param name="clients">list of clients object</param>
        public void HandleClient(TcpClient client, List<TcpClient> clients)
        {
            new Task((() =>
            {
                while (_keepReadingRequests)
                {
                    string commandLine = null;
                    try
                    {
                        var reader = new BinaryReader(client.GetStream());
                        commandLine = reader.ReadString();
                    }
                    catch (Exception ex)
                    {
                        client.Close();
                        lock (clients)
                        {
                            clients.Remove(client);
                        }
                        _keepReadingRequests = false;
                        Logging.Log($"Error while reading commands from client. {ex}", MessageTypeEnum.WARNING);
                    }
                    Logging.Log("ClientHandler received command: " + commandLine, MessageTypeEnum.INFO);
                    try
                    {
                        CommandRecievedEventArgs commandRecievedEventArgs = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        if (IsCloseConnectionCommand(commandRecievedEventArgs))
                        {
                            try
                            {
                                client.Close();
                                lock (clients)
                                {
                                    clients.Remove(client);
                                }
                                Logging.Log("Closed client connection", MessageTypeEnum.INFO);
                                _keepReadingRequests = false;
                            }
                            catch (Exception ex)
                            {
                                client.Close();
                                lock (clients)
                                {
                                    clients.Remove(client);
                                }
                                _keepReadingRequests = false;
                                Logging.Log($"Error occured while closing the connection to the client. error content: {ex}",
                                    MessageTypeEnum.WARNING);
                                Logging.Log("Closed client connection", MessageTypeEnum.INFO);
                            }
                        }
                        else
                        {
                            string result = null;
                            try
                            {
                                result = ExecuteClientsReceivedCommand(commandRecievedEventArgs, client);
                            }
                            catch (Exception ex)
                            {
                                _keepReadingRequests = false;
                                Logging.Log($"Error occured while sending command result: {result}. Error content: {ex}",
                                    MessageTypeEnum.WARNING);
                                Logging.Log("Closed client connection", MessageTypeEnum.INFO);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        client.Close();
                        lock (clients)
                        {
                            clients.Remove(client);
                        }
                        _keepReadingRequests = false;
                        Logging.Log($"Error occured while processing the clients command. error content: {ex}",
                            MessageTypeEnum.WARNING);
                        Logging.Log("Closed client connection", MessageTypeEnum.INFO);
                    }
                }
            })).Start();
        }

        /// <summary>
        /// Return true if the input command is a close command, otherwise false.
        /// </summary>
        /// <param name="args">CommandRecievedEventArgs object</param>
        /// <returns>bool</returns>
        private static bool IsCloseConnectionCommand(CommandRecievedEventArgs args)
        {
            return args.CommandId == (int)CommandEnum.CloseClientConnection;
        }

        /// <summary>
        /// Executes the input command and returns the input client a result response.
        /// </summary>
        /// <param name="commandRecievedEventArgs">CommandRecievedEventArgs object</param>
        /// <param name="client">TcpClient object</param>
        /// <returns>string</returns>
        public string ExecuteClientsReceivedCommand(CommandRecievedEventArgs commandRecievedEventArgs, TcpClient client)
        {
            bool commandResult, responseBack;
            string result = ImageController.ExecuteCommand(commandRecievedEventArgs.CommandId,
                 commandRecievedEventArgs.Args, out commandResult, out responseBack);
            if (responseBack)
            {
                BinaryWriter writer = new BinaryWriter(client.GetStream());
                Mutex.WaitOne();
                writer.Write(result);
                Mutex.ReleaseMutex();
            }
            return result;
        }

        /// <summary>
        /// Initializes the keep reading member.
        /// </summary>
        public void EnableCommandsHandling()
        {
            _keepReadingRequests = true;
        }
        #endregion
    }
}