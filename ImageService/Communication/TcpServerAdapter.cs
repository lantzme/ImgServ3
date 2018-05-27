using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageService.Configuration;
using ImageService.Infrastructure.Events;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Server;
using Newtonsoft.Json;

namespace ImageService.Communication
{
    public class TcpServerAdapter
    {
        #region Properties
        public ILoggingService Logging { get; set; }
        public int Port { get; set; }
        public TcpListener Listener { get; set; }
        public TcpHandler TcpClientHandler { get; set; }
        #endregion

        #region Members
        private List<TcpClient> _clients;
        private bool _isAcceptNewClients;
        private static Mutex _mutex = new Mutex();
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        /// <param name="port">positive port number</param>
        /// <param name="logging">ILoggingService object</param>
        /// <param name="parameters">IModalParameters object</param>
        /// <param name="server">ImageServer object</param>
        public TcpServerAdapter(int port, ILoggingService logging, IModalParameters parameters, ImageServer server)
        {
            Port = port;
            Logging = logging;
            TcpClientHandler = new TcpHandler(parameters, logging, server);
            _clients = new List<TcpClient>();
            _isAcceptNewClients = true;
            Logging.Log("Created tcp handler", MessageTypeEnum.INFO);
            TcpHandler.Mutex = _mutex;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start accepting new clients.
        /// </summary>
        public void Start()
        {
            IPEndPoint endPoint = new
                IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            Listener = new TcpListener(endPoint);
            Logging.Log("Created socket", MessageTypeEnum.INFO);
            Listener.Start();
            Logging.Log("Opened tcp listener", MessageTypeEnum.INFO);
            new Task(() =>
            {
                while (_isAcceptNewClients)
                {
                    try
                    {
                        TcpClient client = Listener.AcceptTcpClient();
                        TcpClientHandler.EnableCommandsHandling();
                        Logging.Log("Received a new connection!", MessageTypeEnum.INFO);
                        lock (_clients)
                        {
                            _clients.Add(client);
                        }
                        TcpClientHandler.HandleClient(client, _clients);
                    }
                    catch (Exception e)
                    {
                        Logging.Log($"Error occured while accepting tcp clients. error content: {e}",
                                MessageTypeEnum.FAIL);
                        _isAcceptNewClients = false;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Notifies the clients when there's an update.
        /// </summary>
        /// <param name="updateArgs">CommandRecievedEventArgs object</param>
        public void NotifySubscribedClientsUpdateRecieved(CommandRecievedEventArgs updateArgs)
        {
            Parallel.ForEach(_clients, (client) =>
            {
                try
                {
                    BinaryWriter writer = new BinaryWriter(client.GetStream());
                    string jsonCommand = JsonConvert.SerializeObject(updateArgs);
                    _mutex.WaitOne();
                    writer.Write(jsonCommand);
                    _mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    lock (_clients)
                    {
                        client.Close();
                        _clients.Remove(client);
                    }
                    Logging.Log($"Error while notifying the client. {ex}", MessageTypeEnum.WARNING);
                    Logging.Log("Closed client connection", MessageTypeEnum.INFO);
                }
            });
        }

        /// <summary>
        /// Stop accepting new clients.
        /// </summary>
        public void Stop()
        {
            Listener.Stop();
        }
        #endregion
    }
}