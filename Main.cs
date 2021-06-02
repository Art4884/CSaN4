using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ProxyServer
{
    class MainClass
    {
        private static readonly string _testRequest1 = "GET /legendyfm HTTP/1.1\r\n" +
                                                       "HOST: live.legendy.by:8000\r\n" +
                                                       "Proxy-Connection: keep-alive\r\n" +
                                                       "Cache-Control: max-age=0\r\n" +
                                                       "Upgrade-Insecure-Request: 1\r\n" +
                                                       "User-Agent: Mozila/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, Like Gecko) Chrome/89.0.4389.128 Safari/537.36\r\n" +
                                                       "Accept: text/html.application/xhtml+xml,application/xml;q=0.9,image/avif.image/webp.image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9\r\n" +
                                                       "Accept-Encoding: gzip, deflate\r\n" +
                                                       "Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\r\n" +
                                                       "\r\n";
        private static readonly string _testRequest2 = "GET /legendyfm HTTP/1.1\r\n" +
                                                       "HOST: live.legendy.by:8000\r\n" +
                                                       "Proxy-Connection: keep-alive\r\n" +
                                                       "User-Agent: Mozila/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, Like Gecko) Chrome/89.0.4389.128 Safari/537.36\r\n" +
                                                       "Accept-Encoding: identity:q=1, *;q=0\r\n" +
                                                       "Accept: */*\r\n" +
                                                       "Referer: /legendyfm\r\n" +
                                                       "Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7\r\n" +
                                                       "Range: bytes=0-\r\n" +
                                                       "\r\n";

        public static void Main(String[] args)
        {
            TcpServer tcpServer = new TcpServer(new IPAddress(0x0100007F));

            //Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //server.Connect(new IPEndPoint(Dns.GetHostEntry("live.legendy.by").AddressList[0], 8000));
            //Console.WriteLine("Server connection success.");

            //int size = 0;
            //byte[] buffer = new byte[20 * 1024 * 1024];

            //server.Send(Encoding.UTF8.GetBytes(_testRequest1));
            //size = server.Receive(buffer);

            //Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, size));

            //Console.WriteLine(TcpTunnel.IsConnectedSocket(server));
            //server.Send(Encoding.UTF8.GetBytes(_testRequest2));
            //while (true)
            //{
            //    size = server.Receive(buffer);
            //    Console.WriteLine(TcpTunnel.IsConnectedSocket(server));
            //    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, size));
            //    Console.ReadKey();
            //}

            //server.Close();
            //Console.ReadKey();
        }
    }
}
