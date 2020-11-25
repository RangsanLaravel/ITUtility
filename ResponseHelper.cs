using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITUtility
{
    public class ResponseHelper
    {

        public static BLAResponse<T> MakeSuccessData<T>(T Data) where T : class
        {
            var response = new BLAResponse<T>();
            response.Data = Data;
            response.IsSuccess = true;
            return response;
        }

        public static BLAResponse<object> MakeErrorData(string message, int errorCode)
        {
            var response = new BLAResponse<object>();
            response.IsSuccess = false;
            response.ErrorCode = errorCode;
            response.ErrorMessage = message;
            return response;
        }

        public static BLAResponse<T> MakeErrorData<T>(string message, int errorCode, T errorList) where T : class
        {
            var response = new BLAResponse<T>();
            response.IsSuccess = false;
            response.ErrorCode = errorCode;
            response.ErrorMessage = message;
            response.Data = errorList;
            return response;
        }
    }
    public class BLAResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public T Data { get; set; }

    }
}