using System.Net;

namespace Forum.API.Models
{
    public class CommonResponse
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
    }
}
