using System.Net;

namespace HospitalAPI.Models
{
    public class Response
    {
        public HttpStatusCode statuscode { get; set; }
        public string message { get; set; }
    }
}
