using System.Collections.Generic;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ChameleonFunction.JoinRoom;
using ChameleonFunction.LeaveRoom;
using ChameleonFunction.RoomStatus;
using ChameleonFunction.Routing;
using ChameleonFunction.StartGame;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonFunction
{
    // ReSharper disable once UnusedMember.Global
    public class Function
    {
        // ReSharper disable once UnusedMember.Global
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var routes = new Routes();
            
            routes.Path("warm-up")
                .Get(CreateWarmUpResponse)
                .Post(CreateWarmUpResponse);
            
            routes.Path("join-room")
                .Post(() => new JoinRoomHandler().Handle(request, context));
            
            routes.Path("room-status")
                .Get(() => new RoomStatusHandler().Handle(request, context));
            
            routes.Path("start-game")
                .Post(() => new StartGameHandler().Handle(request, context));
            
            routes.Path("vote")
                .Post(() => new VoteHandler().Handle(request, context));
            
            routes.Path("leave-room")
                .Post(() => new LeaveRoomHandler().Handle(request, context));

            return routes.Handle(request);
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
