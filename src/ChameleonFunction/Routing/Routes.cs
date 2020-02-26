using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;

namespace ChameleonFunction.Routing
{
    public class Routes
    {
        private readonly Dictionary<string, RouteHandlers> _routes = new Dictionary<string, RouteHandlers>();

        public RouteHandlers Path(string path)
        {
            if (!path.StartsWith("/"))
            {
                path = $"/{path}";
            }

            return _routes.ContainsKey(path)
                ? _routes[path]
                : _routes[path] = new RouteHandlers();
        }

        public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request)
        {
            return _routes.ContainsKey(request.Path)
                ? _routes[request.Path].Handle(request.HttpMethod)
                : NotFoundResponse(request.Path);
        }

        private static APIGatewayProxyResponse NotFoundResponse(string path)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 404,
                Body = $"The path {path} was not found",
                Headers = new Dictionary<string, string>
                {
                    {"Access-Control-Allow-Origin", "*"},
                },
            };
        }
    }
}
