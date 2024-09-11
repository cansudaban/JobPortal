using JobPortal.Common.Helpers;
using System.Resources;

namespace JobPortal.Common.Models
{
    public class ResponseModel
    {
        public ResponseModel() { }

        public ResponseModel(string message, object data, int statusCode)
        {
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public string Message { get; set; }
        public object Data { get; set; }
        public int StatusCode { get; set; }
    }
}
