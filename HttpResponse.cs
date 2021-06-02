using System.Text;

namespace ProxyServer
{
    public class HttpResponse 
    {
        private string[] _response;

        public HttpResponse(byte[] buffer, int size)
        {
            _response = Encoding.UTF8.GetString(buffer, 0, size).Split('\n');
        }

        public byte[] GetBytes()
        {
            string result = "";
            for (int i = 0; i < _response.Length; i++)
            {
                if (_response[i].Length > 0) {
                    result += _response[i] + "\n";
                }
            }
            return Encoding.UTF8.GetBytes(result);
        }

        public string GetStatus()
        {
            if (!_response[0].StartsWith("HTTP")) 
            {
                return "0 Undefined";
            }
            return _response[0].Substring(_response[0].IndexOf(' ') + 1);
        }
    }
}