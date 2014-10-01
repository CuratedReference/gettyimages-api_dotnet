using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GettyImages.Connect
{
    public class SdkException : Exception
    {
        private const string ErrorMessageProperty1 = "ErrorMessage";
        private const string ErorMessageProperty2 = "error_message";
        private const string XMasheryErrorCodeHeaderKey = "X-Mashery-Error-Code";
        private const string ErrDeveloperOverQpsValue = "ERR_403_DEVELOPER_OVER_QPS";

        internal SdkException(string message, HttpStatusCode? statusCode = null) : base(message)
        {
            StatusCode = statusCode;
        }

        internal SdkException(string message, Exception innerException, HttpStatusCode? statusCode = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode? StatusCode { get; set; }

        internal static void GenerateSdkException(HttpResponseMessage httpResponse, string message = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                var response = JObject.Parse(httpResponse.Content.ReadAsStringAsync().Result);
                JToken errorMessage;
                if (response.TryGetValue(ErrorMessageProperty1, out errorMessage) ||
                    response.TryGetValue(ErorMessageProperty2, out errorMessage))
                {
                    message = errorMessage.Value<string>();
                }
            }
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    if (
                        httpResponse.Headers.GetValues(XMasheryErrorCodeHeaderKey)
                            .Any(v => v == ErrDeveloperOverQpsValue))
                    {
                        throw new OverQpsException(message);
                    }

                    throw new SdkException(message, httpResponse.StatusCode);
                    break;
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(message);
                    break;
                default:
                    throw new SdkException(message, httpResponse.StatusCode);
                    break;
            }
        }
    }
}