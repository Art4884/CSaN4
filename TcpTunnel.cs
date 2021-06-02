using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace ProxyServer
{
    public class TcpTunnel
    {
        private Thread _thread;
        private Thread _client;
        private Thread _server;

        private Queue<HttpRequest> _requests = new Queue<HttpRequest>();
        private Queue<byte[]> _responses = new Queue<byte[]>();

        public bool IsAlive => _thread.IsAlive;

        public TcpTunnel(Socket socket)
        {
            _thread = new Thread(new ParameterizedThreadStart(Start));
            _thread.Start(socket);
        }

        ~TcpTunnel()
        {
            Stop();
        }

        private void Start(object socket)
        {
            Socket client = socket as Socket;
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string url = "";

            byte[] buffer = new byte[20 * 1024 * 1024];
            do
            {
                string status = "";
                try
                {
                    if (client.Poll(3000, SelectMode.SelectRead))
                    {
                        int size = client.Receive(buffer);
                        if (size != 0)
                        {
                            HttpRequest request = new HttpRequest(buffer, size);
                            if (!server.Connected)
                            {
                                server.Connect(request.GetHost());
                            }
                            server.Send(request.GetBytes());
                            url = request.GetUrl();
                        }
                    }
                    if (server.Poll(3000, SelectMode.SelectRead))
                    {
                        int size = server.Receive(buffer);
                        if (size != 0)
                        {
                            HttpResponse response = new HttpResponse(buffer, size);
                            client.Send(buffer, size, SocketFlags.None);
                            status = response.GetStatus();
                        }
                    }
                } 
                catch(SocketException)
                {
                    Stop();
                }
                if ((url.Length > 0) && (status.Length > 0))
                {
                    Console.WriteLine(url + ": " + status);
                }
            } while (_thread.IsAlive && IsConnectedSocket(client) && IsConnectedSocket(server));
            //while (_thread.IsAlive && IsConnectedSocket(client))
            //{
            //    try
            //    {
            //        Console.WriteLine("TT");
            //        int size = client.Receive(buffer);
            //        Console.WriteLine("Test size: " + size + "\n" + Encoding.UTF8.GetString(buffer, 0, size));

            //        HttpRequest request = new HttpRequest(buffer, size);
            //        if (!server.Connected || !IsConnectedSocket(server))
            //        {
            //            server.Connect(request.GetHost());
            //        }
            //        server.Send(request.GetBytes());
            //        while (true) {
            //            size = server.Receive(buffer);
            //            client.Send(buffer, size, SocketFlags.None);
            //            Console.WriteLine(IsConnectedSocket(client));
            //            Console.WriteLine("TTT size: " + size + "\n" + Encoding.UTF8.GetString(buffer, 0, size));
            //        }
            //    }
            //    catch (SocketException)
            //    {
            //        Stop();
            //    }
            //}
        }

        private void Stop()
        {
            try
            {
                _client?.Abort();
                _server?.Abort();
                _thread.Abort();
            }
            catch(ThreadStateException)
            {
                Console.WriteLine("Thread has been stopped.");
            }
        }

        public static bool IsConnectedSocket(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}