using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace ProxyServer 
{ 
    public class TcpServer
    {
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private readonly int port = 303;

        private List<TcpTunnel> _tunnels = new List<TcpTunnel>();

        public TcpServer(IPAddress ip)
        {
            Start(ip);
        }

        private void Start(IPAddress ip)
        {
            _socket.Bind(new IPEndPoint(ip, port));
            _socket.Listen(10);

            while (true) 
            {
                try
                {
                    //_connections.Enqueue(_socket.Accept());
                    _tunnels.Add(new TcpTunnel(_socket.Accept()));
                }
                catch (SocketException)
                {
                    throw new Exception("_socket.Accept() failed.");
                }
            }

        }
    }
}