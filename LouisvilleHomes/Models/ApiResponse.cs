using System.Net;

namespace LouisvilleHomes.Models
{
    public class ApiResponse
    {
        public bool IsSuccessStatusCode { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Content { get; set; }
    }
}
