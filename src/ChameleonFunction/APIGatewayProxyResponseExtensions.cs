using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace ChameleonFunction
{
    public static class APIGatewayProxyResponseExtensions
    {
        public static APIGatewayProxyResponse Text(this APIGatewayProxyResponse response, string body, int? status = null)
        {
            response.StatusCode = status ?? (string.IsNullOrEmpty(body) ? 204 : 200);
            response.Headers.Add("Content-Type", "text/html");
            response.Body = body ?? "";
            return response;
        }
        
        public static APIGatewayProxyResponse Json(this APIGatewayProxyResponse response, object obj, int? status = null)
        {
            response.StatusCode = status ?? 200;
            response.Headers.Add("Content-Type", "application/json");
            response.Body = JsonConvert.SerializeObject(obj);
            return response;
        }
    }
}
