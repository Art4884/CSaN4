using System;
using System.Text;
using System.Net;

namespace ProxyServer
{
    public class HttpRequest
    {
        private string[] _request;

        public HttpRequest(byte[] buffer, int size)
        {
            _request = Encoding.UTF8.GetString(buffer, 0, size).Split('\n');
        }

        public byte[] GetBytes()
        {
            if (_request[0].CompareTo("") == 0)
            {
                return new byte[0];
            }

            string[] method = _request[0].Split(' ');
            string[] uri = method[1].Split('/');

            string[] host = _request[1].Split(' ');
            string endPoint = host[1].Replace('\r', '\0');

            string relativeUri = "";
            foreach (string str in uri)
            {
                if ((str.CompareTo("http:") != 0) && (str.CompareTo(endPoint) != 0))
                {
                    relativeUri += str + "/";
                }
            }
            relativeUri = "/" + relativeUri.Trim('/');

            string result = method[0] + " "  + relativeUri + " " + method[2] + "\n";
            for(int i = 1; i < _request.Length; i++)
            {
                result += _request[i] + "\n";
            }

            return Encoding.UTF8.GetBytes(result);
        }

        public IPEndPoint GetHost()
        {
            char[] separators = { '\r', ':' };

            string[] hostRequest = _request[1].Split(' ');
            string[] host = hostRequest[1].Split(separators);

            if (host.Length > 2) {
                return new IPEndPoint(Dns.GetHostAddresses(host[0])[0], Convert.ToInt32(host[1]));
            } 
            else
            {
                return new IPEndPoint(Dns.GetHostAddresses(host[0])[0], 80);
            }
        }

        public string GetUrl()
        {
            int startIndex = _request[0].IndexOf(' ') + 1;
            int endIndex = _request[0].LastIndexOf(' ') - 1;
            int substringLength = (endIndex - startIndex) + 1;

            return _request[0].Substring(startIndex, substringLength);
        }
    }
}