using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataStore;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonStartGameFunction
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var roomCode = request.QueryStringParameters["RoomCode"];
            var personId = request.Headers["x-chameleon-personid"];
            var room = Client.GetRoom(roomCode);

            if (!room.PersonIds.Contains(personId))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 403,
                    Body = $"Person {personId} not in room {roomCode}",
                    Headers = new Dictionary<string, string> { },
                };
            }

            Client.StartGame(roomCode);

            return new APIGatewayProxyResponse
            {
                StatusCode = 204,
                Body = "",
                Headers = new Dictionary<string, string>(),
            };
        }
    }
}
