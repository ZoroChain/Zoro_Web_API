using Newtonsoft.Json.Linq;

namespace Zoro_Web_API.RPC
{
    public class JsonPRCresponse
    {
        public string jsonrpc { get; set; }
        public long id { get; set; }
        public JArray result { get; set; }
    }
}
