using System;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Collections.Generic;
using GameLogic;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonGetRoomStatusFunction
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var roomCode = request.QueryStringParameters["RoomCode"];
            var personId = GetPersonId(request);

            try
            {
                var status = ChameleonGame.Create().GetRoomStatus(roomCode, personId);
                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = JsonConvert.SerializeObject(status),
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json" },
                        { "Access-Control-Allow-Origin", "*" },
                    },
                };
            }
            catch (PersonNotInRoomException e)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 403,
                    Body = e.Message,
                    Headers = new Dictionary<string, string>
                    {
                        { "Access-Control-Allow-Origin", "*" },
                    },
                };
            }
        }

        private static string GetPersonId(APIGatewayProxyRequest request)
        {
            if (request.Headers.TryGetValue("x-chameleon-personid", out var personId))
            {
                return personId;
            }
            if (request.Headers.TryGetValue("X-Chameleon-Personid", out personId))
            {
                return personId;
            }

            return "";
        }
    }
}
