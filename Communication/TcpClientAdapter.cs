using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using Newtonsoft.Json;

namespace ImageService.UI.Communication
{
    public class TcpClientAdapter
    {


        #region Properties
        public bool IsConnected { get; set; }

        #endregion

        #region Members
        public delegate void HandleRecievedResponse(CommandRecievedEventArgs args);
        public event HandleRecievedResponse HandleResp;
        private static TcpClientAdapter _instance;
        private static readonly Mutex Mutex = new Mutex();
        private TcpClient _client;
        private bool _isStopped;
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        private TcpClientAdapter()
        {
            IsConnected = StartConnection();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the instance of the class.
        /// </summary>
        public static TcpClientAdapter Instance => _instance ?? (_instance = new TcpClientAdapter());

        /// <summary>
        /// Sends the input command to the server.
        /// </summary>
        /// <param name="commandRecievedEventArgs">command object</param>
        public void SendCommandToServer(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            new Task(() =>
            {
                try
                {
                    string jsonCommand = JsonConvert.SerializeObject(commandRecievedEventArgs);
                    BinaryWriter writer = new BinaryWriter(_client.GetStream());
                    lock (writer)
                    {
                        writer.Write(jsonCommand);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    IsConnected = false;
                }
            }).Start();
        }

        /// <summary>
        /// Starts the tcp connection with the server.
        /// </summary>
        /// <returns>True if the connection established successfully, otherwise false</returns>
        private bool StartConnection()
        {
            try
            {
                IPAddress imageServerIp = IPAddress.Parse("127.0.0.1");
                int imageServicePort = 8000;
                IPEndPoint ep = new IPEndPoint(imageServerIp, imageServicePort);
                _client = new TcpClient();
                _client.Connect(ep);
                _isStopped = false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        /// <summary>
        /// Closing the connection to the server.
        /// </summary>
        public void CloseTcpClient()
        {
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseClientConnection, null, "");
            SendCommandToServer(commandRecievedEventArgs);
            Thread.Sleep(100);
            _client.Close();
            _isStopped = true;
        }

        /// <summary>
        /// Read the server's response we got.
        /// </summary>
        public void RecieveResponseFromServer()
        {
            new Task(() =>
            {
                try
                {
                    while (!_isStopped)
                    {
                        NetworkStream stream = _client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        StringBuilder respStringBuilder = new StringBuilder();
                        string resp = String.Empty;
                        //Reading the server response
                        do
                        {
                            Mutex.WaitOne();
                            respStringBuilder.Append(reader.ReadString());
                        } while (stream.DataAvailable);
                        Mutex.ReleaseMutex();
                        CommandRecievedEventArgs respJsonObject = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(respStringBuilder.ToString());
                        HandleResp?.Invoke(respJsonObject);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }).Start();
        }
        #endregion
    }
}