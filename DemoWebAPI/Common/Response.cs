using System.Net;

namespace DemoWebApi.Common
{
    //Create class for unification server response it makes the interaction more predictable
    public class Response
    {
        public HttpStatusCode Status { get; set; }

        //Human friendly description status message
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}
