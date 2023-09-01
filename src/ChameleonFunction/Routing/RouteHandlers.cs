using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.APIGatewayEvents;

namespace ChameleonFunction.Routing
{
    public class RouteHandlers
    {
        private readonly Dictionary<string, Func<APIGatewayProxyResponse>> _handlers
            = new Dictionary<string, Func<APIGatewayProxyResponse>>();

        public RouteHandlers Get(Func<APIGatewayProxyResponse> handler)
        {
            _handlers["GET"] = handler;
            return this;
        }
        
        public RouteHandlers Post(Func<APIGatewayProxyResponse> handler)
        {
            _handlers["POST"] = handler;
            return this;
        }

        public APIGatewayProxyResponse Handle(string method)
        {
            return _handlers.ContainsKey(method)
                ? _handlers[method].Invoke()
                : MethodNotAllowedResponse(_handlers.Keys.ToArray());
        }

        private static APIGatewayProxyResponse MethodNotAllowedResponse(string[] allowedMethods)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 405,
                Body = "",
                Headers = new Dictionary<string, string>
                {
                    {"Allow", string.Join(",", allowedMethods)},
                    {"Access-Control-Allow-Origin", "*"},
                },
            };
        }
    }
}
