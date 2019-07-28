using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using DataStore;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonGetRoomStatusFunction
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var roomCode = request.QueryStringParameters["RoomCode"];
            var personId = request.Headers["X-Chameleon-PersonId"];
            var room = Client.GetRoom(roomCode);

            if(!room.PersonIds.Contains(personId))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 403,
                    Body = $"Person {personId} not in room {roomCode}",
                    Headers = new Dictionary<string, string>{},
                };
            }

            var peopleCount = room.PersonIds.Count;
            var status = new RoomStatus(
                code: roomCode,
                peopleCount: peopleCount,
                chameleonCount: GetChameleonCount(peopleCount),
                state: RoomState.PreGame.ToString(),
                character: "");

            return new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonConvert.SerializeObject(status),
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                },
            };
        }

        private int GetChameleonCount(int peopleCount) => (int)Math.Ceiling((double)(peopleCount / 6));
    }
}
