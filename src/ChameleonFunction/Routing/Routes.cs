using System.Collections.Generic;
using System.Linq;
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
            var segments = request.Path
                .Split('/')
                .Where(x => !string.IsNullOrEmpty(x));
            var path = string.Join("/", segments);
            return _routes.ContainsKey(path)
                ? _routes[path].Handle(request.HttpMethod)
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
