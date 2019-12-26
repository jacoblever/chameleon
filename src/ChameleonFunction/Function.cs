using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonFunction
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var endpoint = string.Join('/', request.Path.Split("/").Skip(2));
            switch (endpoint)
            {
                case "warm-up":
                case "warm-up/":
                    return Respond(request, "GET", CreateWarmUpResponse);
                case "join-room":
                case "join-room/":
                    return Respond(request, "POST", ()
                        => new ChameleonJoinRoomFunction.Function().FunctionHandler(request, context));
                case "room-status":
                case "room-status/":
                    return Respond(request, "GET", ()
                        => new ChameleonGetRoomStatusFunction.Function().FunctionHandler(request, context));
                case "start-game":
                case "start-game/":
                    return Respond(request, "POST", ()
                        => new ChameleonStartGameFunction.Function().FunctionHandler(request, context));
                case "leave-room":
                case "leave-room/":
                    return Respond(request, "POST", ()
                        => new ChameleonLeaveRoomFunction.Function().FunctionHandler(request, context));
                default:
                    return CreateNotFoundResponse(request);
            }
        }

        private static APIGatewayProxyResponse CreateNotFoundResponse(APIGatewayProxyRequest request)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 404,
                Body = $"The path {request.Path} was not found",
                Headers = new Dictionary<string, string>
                {
                    {"Access-Control-Allow-Origin", "*"},
                },
            };
        }

        private static APIGatewayProxyResponse Respond(
            APIGatewayProxyRequest request,
            string method,
            Func<APIGatewayProxyResponse> handler)
        {
            return request.HttpMethod == method ? handler() : CreateMethodNotAllowedResponse(method);
        }

        private static APIGatewayProxyResponse CreateMethodNotAllowedResponse(string allow)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 405,
                Body = "",
                Headers = new Dictionary<string, string>
                {
                    {"Allow", allow},
                    {"Access-Control-Allow-Origin", "*"},
                },
            };
        }

        private static APIGatewayProxyResponse CreateWarmUpResponse()
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = 204,
                Body = "",
                Headers = new Dictionary<string, string>
                {
                    {"Access-Control-Allow-Origin", "*"},
                },
            };
        }
    }
}
