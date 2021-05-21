using System.Net.Http;
using System.Windows;

namespace HTTP_REST_CLient
{
    public static class ResponseChecker
    {
        public static bool CheckResponse(HttpResponseMessage responseMessage, out string msg)
        {
            msg = responseMessage.StatusCode.ToString();
            return responseMessage.IsSuccessStatusCode;
        }
    }
}