using JobPortal.Common.Helpers;
using System.Net;

namespace JobPortal.Common.ServiceResultManager
{
    public class ServiceResult
    {
        public ServiceResult(string message = null, HttpStatusCode? resultCode = null, object data = null) 
        {
            ResultCode = resultCode ?? HttpStatusCode.SeeOther;
            Message = message;
            Data = data;
        }
        public HttpStatusCode ResultCode { get; }
        public string Message { get; }
        public object Data { get; set; }
        public bool IsSuccess()
        {
            return ResultCode == HttpStatusCode.OK;
        }
    }

    public abstract class Result
    {
        public static ServiceResult ReturnAsSuccess(string message = null, object data = null)
        {
            return new ServiceResult(message ?? ResourceHelper.OperationSuccess, HttpStatusCode.OK, data);
        }
        public static ServiceResult ReturnAsFail(string message = null, object data = null)
        {
            return new ServiceResult(message ?? ResourceHelper.OperationFailed, HttpStatusCode.BadRequest, data);
        }
    }
}
